using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _mask;



    private PlayerWeapon _currentWeapon;
    private WeaponManager _weaponManager;
    
    void Start()
    {
        if (_cam == null)
        {
            Debug.Log("PlayerShoot: No Camera Found!!");
            this.enabled = false;
        }

        _weaponManager = GetComponent<WeaponManager>();
    }

    void Update()
    {
        _currentWeapon = _weaponManager.GetCurrentWeapon();
        if (_currentWeapon.fireRate <= 0f)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                InvokeRepeating("Shoot", 0f, 1f/_currentWeapon.fireRate);
            } else if (Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot");
            }
        }

    }

    [Client]
    private void Shoot()
    {
        Debug.Log("Shoot is done");
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _currentWeapon.range, _mask))
        {
            Player player = hit.transform.GetComponent<Player>();
            if (player != null)
            {
                CmdPlayerShot(player.ID, _currentWeapon.damage);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string playerId, int damage)
    {
        Debug.Log("ID has been : " + playerId);

        Player player = GameManager.Instance.GetPlayer(playerId);
        player.RpcTakeDamage(damage);
    }
}