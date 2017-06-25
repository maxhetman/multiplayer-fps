
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HostBtn : MonoBehaviour {

    [SerializeField] Text _inputName;
    [SerializeField] GameObject _enterNameLabel;

    void Start ()
	{
	    InitButtonListener();
    }

    private void InitButtonListener()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //TODO: fix hard coding port

            if (_inputName.text.Length == 0)
            {
                _enterNameLabel.SetActive(true);
                return;
            }

            PlayerPrefs.SetString("Player_name", _inputName.text);
            NetworkManager.singleton.networkPort = 7777;
            NetworkManager.singleton.maxConnections = 50;
            NetworkManager.singleton.StartHost();
        });

    }
	

}
