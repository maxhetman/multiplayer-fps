using UnityEngine;
using UnityEngine.Networking;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField] private PlayerWeapon _primaryWeapon;
    [SerializeField] private string weaponLayerName = "Weapon";
    [SerializeField] private Transform _weaponHolder;


    private PlayerWeapon _currentWeapon;

    void Start()
    {
        EquipWeapon(_primaryWeapon);
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return _currentWeapon;
    }

    private void EquipWeapon(PlayerWeapon _newWeapon)
    {
        _currentWeapon = _newWeapon;

        GameObject weaponIns = Instantiate(_newWeapon.graphics, _weaponHolder.position, _weaponHolder.rotation);
        weaponIns.transform.SetParent(_weaponHolder);

        if (isLocalPlayer)
        {
            weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);
        }
    }

}
