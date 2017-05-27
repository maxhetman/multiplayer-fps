using System;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerShoot : NetworkBehaviour
{

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
            Player player = hit.transform.GetComponent<Player>();
            if (player != null)
            {
                CmdPlayerShot(player.ID, Weapon.damage);
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