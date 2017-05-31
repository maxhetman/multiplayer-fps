using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour {

    #region Variables
    public string ID { get; set; }

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;
    [SerializeField] private GameObject _deathEffect;
    [SerializeField] private GameObject[] _disableGameObjectsOnDeath;
    [SerializeField] private GameObject _spawnEffect;

    private bool _firstSetup = true;
    [SyncVar]
    private bool _isDead = false;

    public bool IsDead {
        get { return _isDead; }
    }

    [SyncVar]
    private int _currentHealth;
    #endregion

    void Update()
    {
        if (!isLocalPlayer)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // RpcTakeDamage(99999);
            ShowEscapeWindow();

        }
    }

    private void ShowEscapeWindow()
    {
        GetComponent<PlayerSetup>().playerUIInstance.GetComponent<PlayerUI>().EscapeMenu.SetActive(true);
    }

    public void SetupPlayer()
    {

        if (isLocalPlayer)
        {
            //Switch from scene to player camera    
            GameManager.Instance.SetSceneCameraState(false);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);
        }

        CmdBroadcastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadcastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }

    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        Debug.Log("RPC SETUP CALLED");

        if (_firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            _firstSetup = false;
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        _isDead = false;

        _currentHealth = _maxHealth;

        //Enable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        //Enable gameObjects
        for (int i = 0; i < _disableGameObjectsOnDeath.Length; i++)
        {
            _disableGameObjectsOnDeath[i].SetActive(true);
        }

        //Enable collider
        Collider coll = GetComponent<Collider>();
        if (coll != null)
        {
            coll.enabled = true;
        }

        //Create spawn effect
        GameObject spawnEffect = Instantiate(_spawnEffect, transform.position, Quaternion.identity);
        Destroy(spawnEffect, 3f);
    }

    [ClientRpc]
    public void RpcTakeDamage(int amount)
    {
        if (_isDead)
        {
            return;
        }
        _currentHealth -= amount;

        Debug.Log(ID + " now has health : " + _currentHealth + " health");

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _isDead = true;

        //Disable components
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        //Disable gameObjects
        for (int i = 0; i < _disableGameObjectsOnDeath.Length; i++)
        {
            _disableGameObjectsOnDeath[i].SetActive(false);
        }

        //Disable collider
        Collider coll = GetComponent<Collider>();
        if (coll != null)
        {
            coll.enabled = false;
        }

        //Spawn death explosion
        GameObject explosion = Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Destroy(explosion, 3f);

        //Switch camera to scene 
        if (isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraState(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }
        Debug.LogFormat("Player with id {0} is dead", ID);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.Instance.MatchSettings.RespawnTime);

        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        SetupPlayer();

    }
}
