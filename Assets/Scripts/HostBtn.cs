
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HostBtn : MonoBehaviour {

	void Start ()
	{
	    InitButtonListener();
    }

    private void InitButtonListener()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //TODO: fix hard coding port
            NetworkManager.singleton.networkPort = 7777;
            NetworkManager.singleton.maxConnections = 50;
            NetworkManager.singleton.StartHost();
        });

    }
	

}
