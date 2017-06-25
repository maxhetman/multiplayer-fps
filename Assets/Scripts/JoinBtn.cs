using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class JoinBtn : MonoBehaviour {
    string HostName = "127.0.0.1";

    [SerializeField] Text _inputName;
    [SerializeField] GameObject _enterNameLabel;

    void Start()
    {
        InitButtonListener();
    }

    private void InitButtonListener()
    {


        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (_inputName.text.Length == 0)
            {
                _enterNameLabel.SetActive(true);
                return;
            }

            PlayerPrefs.SetString("Player_name", _inputName.text);
            NetworkManager.singleton.networkAddress = HostName;
            NetworkManager.singleton.networkPort = 7777;
            NetworkManager.singleton.StartClient();
        });

    }

}
