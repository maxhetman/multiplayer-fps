﻿using System;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(WeaponManager))]
[RequireComponent(typeof(Player))]
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

    //is called on the server when player shoots
    [Command]
    void CmdOnShoot()
    {
        RpcDoShootEffect();
    }

    //call for all clients to show player    shooting
    [ClientRpc]
    void RpcDoShootEffect()
    {
        _weaponManager.GetCurrentGraphics().MuzzleFlash.Play();
    }

    //called on server when player hits something
    [Command]
    void CmdOnHit(Vector3 pos, Vector3 normal)
    {
        RpcDoHitEffect(pos, normal);
    }

    //call for all clients to show when player hit something
    [ClientRpc]
    void RpcDoHitEffect(Vector3 pos, Vector3 normal)
    {
        GameObject hitEffect = Instantiate(_weaponManager.GetCurrentGraphics().HitEffectPrefab, pos, Quaternion.LookRotation(normal));
        Destroy(hitEffect, 2f);
    }

    [Client]
    private void Shoot()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        //call onshoot method on the server
        CmdOnShoot();

        Debug.Log("Shoot is done");
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, _currentWeapon.range, _mask))
        {
            Player player = hit.transform.GetComponent<Player>();
            if (player != null)
            {
                CmdPlayerShot(player.ID, _currentWeapon.damage);
            }

            //Call onhit method on server when we hit something
            CmdOnHit(hit.point, hit.normal);
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