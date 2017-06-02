using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{

    #region Variables
    [SerializeField] private PlayerWeapon _primaryWeapon;
    [SerializeField] private string weaponLayerName = "Weapon";
    [SerializeField] private Transform _weaponHolder;


    private PlayerWeapon _currentWeapon;
    private WeaponGraphics _currentGraphics;
    public bool IsReloading = false;
    #endregion

    void Start()
    {
        EquipWeapon(_primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return _currentGraphics;
    }

    public void Reload()
    {
        if (IsReloading)
            return;
        StartCoroutine(ReloadCoroutine());


    }

    private IEnumerator ReloadCoroutine()
    {
        IsReloading = true;

        CmdOnReload();
        yield return new WaitForSeconds(_currentWeapon.reloadTime);
        _currentWeapon.bullets = _currentWeapon.maxBullets;

        IsReloading = false;
    }

    private void EquipWeapon(PlayerWeapon _newWeapon)
    {
        _currentWeapon = _newWeapon;

        GameObject weaponIns = Instantiate(_newWeapon.graphics, _weaponHolder.position, _weaponHolder.rotation);
        weaponIns.transform.SetParent(_weaponHolder);

        _currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if (_currentGraphics == null)
        {
            Debug.Log("WeaponManager: no weapon graphics component on the object " + weaponIns.name);
        }

        if (isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = _currentGraphics.GetComponent<Animator>();
        if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }

}
