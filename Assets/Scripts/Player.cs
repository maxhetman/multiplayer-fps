using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour {

    #region Variables
    public string ID { get; set; }

    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private Behaviour[] disableOnDeath;
    [SerializeField] private bool[] wasEnabled;

    [SyncVar]
    private bool _isDead = false;

    public bool IsDead {
        get { return _isDead; }
    }

    [SyncVar]
    private int _currentHealth;
    #endregion

    //void Update()
    //{
    //    if (!isLocalPlayer)
    //    {
    //        return;
    //    }
    //    if (Input.GetKey(KeyCode.Escape))
    //    {
    //        RpcTakeDamage(99999);
    //    }
    //}

    public void Setup()
    {
        wasEnabled = new bool[disableOnDeath.Length];
        for (int i = 0; i < wasEnabled.Length; i++)
        {
            wasEnabled[i] = disableOnDeath[i].enabled;
        }
        SetDefaults();
    }

    public void SetDefaults()
    {
        _isDead = false;

        _currentHealth = _maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        Collider coll = GetComponent<Collider>();
        if (coll != null)
        {
            coll.enabled = true;
        }
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

        Collider coll = GetComponent<Collider>();
        if (coll != null)
        {
            coll.enabled = false;
        }

        Debug.LogFormat("Player with id {0} is dead", ID);

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.Instance.MatchSettings.RespawnTime);

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }
}
