using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    private Behaviour[] componentsToDisable;

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
        }

        GetComponent<Player>().Setup();
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
        if (_sceneCamera != null)
        {
            _sceneCamera.gameObject.SetActive(true);
        }

        string id = GetComponent<Player>().ID;
        GameManager.Instance.UnregisterPlayer(id);
    }
}
