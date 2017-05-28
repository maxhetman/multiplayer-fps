using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    private Behaviour[] componentsToDisable;

    [SerializeField] private string dontDrawLayerName = "DontDraw";
    [SerializeField] private string weaponLayerName = "Weapon"; 

    [SerializeField] private GameObject playerGraphics;
    [SerializeField] private GameObject playerUIPrefab;
    private GameObject playerUIInstance;

    private const string _remoteLayerName = "RemotePlayer";

    private const string PLAYER_ID_PREFIX = "Player";

    private Camera _sceneCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            _sceneCamera = Camera.main;
            if (_sceneCamera != null)
            {
                _sceneCamera.gameObject.SetActive(false);
            }

            //disable player graphics for local player
            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;
        }

        GetComponent<Player>().Setup();
    }

    private void SetLayerRecursively(GameObject obj, int newLayer)
    {
        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public override void OnStartClient()
    {
        string networkId = GetComponent<NetworkIdentity>().netId.ToString();
        string playerId = PLAYER_ID_PREFIX + networkId;
        Player player = GetComponent<Player>();
        player.ID = playerId;
        GameManager.Instance.RegisterPlayer(playerId, player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(_remoteLayerName);
    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }

    void OnDisable()
    {
        Destroy(playerUIInstance);
        if (_sceneCamera != null)
        {
            _sceneCamera.gameObject.SetActive(true);
        }

        string id = GetComponent<Player>().ID;
        GameManager.Instance.UnregisterPlayer(id);
    }
}
