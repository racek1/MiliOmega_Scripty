using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDatabase : MonoBehaviour
{
    public static List<Weapon> weapons = new List<Weapon>();

    void Awake()
    {
        BuildDatabase();
    }

    public static Weapon GetWeapon(string name)
    {
        BuildDatabase();
        return weapons.Find(item => item.name == name);
    }

    public static Weapon GetRandomWeapon()
    {
        BuildDatabase();
        return weapons[Random.Range(0, weapons.Count)];
    }

    public static List<Weapon> GetAllWeapons()
    {
        BuildDatabase();
        return weapons;
    }

    static void BuildDatabase()
    {
        weapons = new List<Weapon>()
        {
            new Weapon("Pistol",25,0.35f,8,1,Weapon.WeaponType.NORMAL,false),
            new Weapon("Smg",10,0.1f,25,1.5f,Weapon.WeaponType.NORMAL,false),
            new Weapon("Ar",15,0.15f,30,1.25f,Weapon.WeaponType.NORMAL,false),
            new Weapon("Shotgun",12,0.75f,6,1.5f,Weapon.WeaponType.NORMAL,false),
            new Weapon("SniperRifle",35,0.75f,5,1f,Weapon.WeaponType.NORMAL,true),
            new Weapon("Revolver",25,0.45f,6,0.65f,Weapon.WeaponType.NORMAL,true),
        };
    }
}
