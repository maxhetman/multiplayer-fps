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
}
