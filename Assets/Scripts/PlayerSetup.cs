using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{

    [SerializeField]
    private Behaviour[] componentsToDisable;

    [SerializeField] private string dontDrawLayerName = "DontDraw";

    [SerializeField] private GameObject playerGraphics;
    [SerializeField] private GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;

    private const string _remoteLayerName = "RemotePlayer";

    private const string PLAYER_ID_PREFIX = "Player";

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            //disable player graphics for local player
            Util.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //create player UI
            playerUIInstance = Instantiate(playerUIPrefab);
            playerUIInstance.name = playerUIPrefab.name;

            //configure player ui
            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("PlayerSetup: no ui controller on player ui");
                return;
            }
            ui.SetController(GetComponent<PlayerController>());
            ui.Player = GetComponent<Player>();

            GetComponent<Player>().SetupPlayer();

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

        Debug.Log("On disable called by : " + GetComponent<Player>().ID);

        if (isLocalPlayer)
        {
            GameManager.Instance.SetSceneCameraState(true);
        }

        string id = GetComponent<Player>().ID;
        GameManager.Instance.UnregisterPlayer(id);
    }

}
