using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public string name;
    public int damage;
    public float attackSpeed;
    public int magSize;
    public WeaponType weaponType;
    public bool piercing;
    public float reloadTime;
    public int currentAmmo;

    public enum WeaponType
    {
        NORMAL, SHOTGUN
    }

    public Weapon(string name, int damage, float attackSpeed, int magSize, float reloadTime, WeaponType weaponType, bool piercing)
    {
        this.name = name;
        this.damage = damage;
        this.attackSpeed = attackSpeed;
        this.magSize = magSize;
        this.weaponType = weaponType;
        this.piercing = piercing;
        this.reloadTime = reloadTime;
        this.currentAmmo = magSize;
    }

    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Weapons/" + name + "/" + name);
    }

    public Sprite GetFiringSprite()
    {
        return Resources.Load<Sprite>("Sprites/Weapons/" + name + "/" + name + "_Firing");
    }
}
