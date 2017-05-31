using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JoinBtn : MonoBehaviour {
    string HostName = "127.0.0.1";

    void Start()
    {
        InitButtonListener();
    }

    private void InitButtonListener()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            NetworkManager.singleton.networkAddress = HostName;
            NetworkManager.singleton.networkPort = 7777;
            NetworkManager.singleton.StartClient();
        });

    }

}
