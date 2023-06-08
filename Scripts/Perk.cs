using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk
{
    public string name;
    public string description;
    public int maxLevel;

    public Perk(string name, string description, int maxRank)
    {
        this.name = name;
        this.description = description;
        this.maxLevel = maxRank;
    }

    public Sprite GetSprite()
    {
        return Resources.Load<Sprite>("Sprites/Perks/" + name);
    }

    public override string ToString()
    {
        return "[" + name + "]" + " [" + description + "]" + " [" + maxLevel + "]";
    }
}
