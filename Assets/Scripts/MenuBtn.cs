using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MenuBtn : MonoBehaviour {
    void Start()
    {
        GetComponent<Button>().onClick.AddListener((() =>
        {
            Player player = GetComponentInParent<PlayerUI>().Player;
            if (player == null)
            {
                Debug.Log("MenuBtn: player is null!");
                return;
            }
            //host
            if (player.isServer && player.isClient)
            {
                NetworkManager.singleton.StopHost();
            }
            else if (player.isClient)
            {
                NetworkManager.singleton.StopClient();
            }      
        }));
    }
}
