using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private RectTransform _thrusterFuelFill;
    [SerializeField] private GameObject EscapeMenu;

    private PlayerController _controller;
    private Player _player;

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }

    void Update()
    {
        SetFuelAmount(_controller.GetThrusterFuelAmount());
    }

    public void ToggleEscapeMenu()
    {
        EscapeMenu.SetActive(!EscapeMenu.activeSelf);
        GameManager.Instance.IsMenuOpened = EscapeMenu.activeSelf;
    }

    public void SetController(PlayerController controller)
    {
        _controller = controller;
    }

    private void SetFuelAmount(float amount)
    {
        _thrusterFuelFill.localScale = new Vector3(1, amount, 1);
    }
}
