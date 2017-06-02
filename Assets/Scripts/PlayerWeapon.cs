using System;
using UnityEngine;

[Serializable]
public class PlayerWeapon
{
    public string name = "Deagle";
    public int damage = 10;
    public float range = 200f;
    public GameObject graphics;
    public float fireRate = 0f;
    public int maxBullets = 20;
    public float reloadTime = 1f;

    [HideInInspector] public int bullets;

    public PlayerWeapon()
    {
        bullets = maxBullets;
    }
}
