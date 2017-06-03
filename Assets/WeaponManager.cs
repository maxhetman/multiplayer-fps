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

        GameObject weaponIns = Instantiate(_newWeapon.graphics, _weaponHolder.position, _weaponHolder.rotation);
        _currentWeapon = weaponIns.GetComponent<PlayerWeapon>();

        weaponIns.transform.SetParent(_weaponHolder);



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
        Animator anim = _currentWeapon.GetComponent<Animator>();
            if (anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }

}
