using System;
using UnityEngine;

public class PlayerWeapon: MonoBehaviour
{
    public string name = "Rifle";
    public int damage = 10;
    public float range = 200f;
    public GameObject graphics;
    public float fireRate = 0f;
    public int maxBullets = 20;
    public float reloadTime = 1f;
    public AudioClip shootSound;
    public ParticleSystem MuzzleFlash;

    public int bullets;

    public PlayerWeapon()
    {
        Debug.Log("here");
        bullets = maxBullets;
        Debug.Log("bullets : " + bullets);
    }

    void Update()
    {
        Debug.Log("bullets : " + bullets);
    }
    void Awake()
    {
        Debug.Log("parent : " + gameObject.name);
    }
}
