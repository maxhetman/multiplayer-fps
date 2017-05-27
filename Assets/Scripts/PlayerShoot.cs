using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

    private const string PLAYER_TAG = "Player";
    public PlayerWeapon Weapon;

    [SerializeField] private Camera _cam;
    [SerializeField] private LayerMask _mask;

    void Start()
    {
        if (_cam == null)
        {
            Debug.Log("PlayerShoot: No Camera Found!!");
            this.enabled = false;
        }
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    [Client]
    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out hit, Weapon.range, _mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                CmdPlayerShot(hit.collider.name);
            }
        }
    }

    [Command]
    void CmdPlayerShot(string id)
    {
        Debug.Log("ID has been : " + id);
    }
}
