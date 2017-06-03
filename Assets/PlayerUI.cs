using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{

    [SerializeField] private RectTransform _thrusterFuelFill;
    [SerializeField] private GameObject EscapeMenu;
    [SerializeField] private RectTransform _healthBarFill;
    [SerializeField] private Text _ammoText;

    private PlayerController _controller;
    private Player _player;
    private WeaponManager _weaponManager;

    void Update()
    {
        SetFuelAmount(_controller.GetThrusterFuelAmount());
        SetHealthAmount(_player.GetHealthPct());
        SetAmmoAmount(_weaponManager.GetCurrentWeapon().bullets);
    }
    public void ToggleEscapeMenu()
    {
        EscapeMenu.SetActive(!EscapeMenu.activeSelf);
        GameManager.Instance.IsMenuOpened = EscapeMenu.activeSelf;
    }

    public Player GetPlayer()
    {
        return _player;
    }

    public void SetPlayer(Player player)
    {
        _player = player;
        _controller = player.GetComponent<PlayerController>();
        _weaponManager = player.GetComponent<WeaponManager>();
    }

    private void SetAmmoAmount(int number)
    {
        _ammoText.text = number.ToString();
    }

    private void SetHealthAmount(float amount)
    {
        _healthBarFill.localScale = new Vector3(1, amount, 1);
    }

    private void SetFuelAmount(float amount)
    {
        _thrusterFuelFill.localScale = new Vector3(1, amount, 1);
    }
}
