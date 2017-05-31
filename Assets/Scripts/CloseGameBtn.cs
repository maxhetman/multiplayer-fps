using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class CloseGameBtn : MonoBehaviour {

    void Start()
    {
        GetComponent<Button>().onClick.AddListener((() =>
        {
            Application.Quit();
        }));
    }
}
