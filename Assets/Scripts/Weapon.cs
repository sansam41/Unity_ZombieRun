using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected AudioSource gunAudioPlayer;
    public AudioSource reloadSource;
    public AudioClip shotClip;
    public AudioClip reloadClip;

    public float damage;
    public float upgradedDamage;
    protected float fireDistance;

    public int ammoRemain;
    public int magCapacity;
    public int magAmmo;


    public float timeBetFire;
    public float reloadTime;
    protected float lastFireTime;

    public virtual void Fire()
    {

    }

    public float GetDamage()
    {
        upgradedDamage = damage + 10 * PlayerPrefs.GetInt("Upgrade");
        return upgradedDamage;
    }
}
