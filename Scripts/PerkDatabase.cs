using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerkDatabase : MonoBehaviour
{
    public static List<Perk> perks = new List<Perk>();
    private Player? player;

    void Awake()
    {
        BuildDatabase();
        if (GameObject.Find("Player") != null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
    }

    public static Perk GetWeapon(string name)
    {
        BuildDatabase();
        return perks.Find(item => item.name == name);
    }

    public static Perk GetRandomPerk()
    {
        BuildDatabase();
        return perks[Random.Range(0, perks.Count)];
    }

    public static List<Perk> GetAllPerks()
    {
        BuildDatabase();
        return perks;
    }

    static void BuildDatabase()
    {
        perks = new List<Perk>()
        {
            new Perk("Gnome slayer","Increases overall damage",10),
            new Perk("Fast hands","Increases reload speed",5),
            new Perk("Quick trigger","Increases attack speed",8),
            new Perk("Bigger mag","Increases magazine capacity",5),
        };
    }

    private static List<Perk> GetAvailablePerks()
    {
        Player player = GameObject.Find("Player").GetComponent<Player>();
        List<Perk> availablePerks = new List<Perk>();
        List<Perk> allPerks = GetAllPerks();
        for (int i = 0; i < allPerks.Count; i++)
        {
            if (player.GetPerkCount(allPerks[i].name) < allPerks[i].maxLevel)
            {
                availablePerks.Add(allPerks[i]);
            }
        }
        foreach (Perk perk in availablePerks) { Debug.Log(perk.ToString()); }
        return availablePerks;
    }

    public static List<Perk> GetPerks()
    {
        List<Perk> availablePerks = GetAvailablePerks();
        /*
        foreach (Perk p in availablePerks)
        {
            if (player.GetPerkCount(p.name) >= p.maxLevel) { availablePerks.Remove(p); }
        }
        */

        List<Perk> randomPerks = new List<Perk>();
        while (randomPerks.Count < Mathf.Clamp(availablePerks.Count,0,3))
        {
            Perk p = GetRandomPerk();
            if (!perkExistsInList(randomPerks, p.name) && perkExistsInList(availablePerks, p.name))
            {
                randomPerks.Add(p);
            }
        }
        return randomPerks;
    }

    private static bool perkExistsInList(List<Perk> list, string perkName)
    {
        foreach (Perk p in list)
        {
            if (p.name.Equals(perkName)) { return true; }
        }
        return false;
    }
}
