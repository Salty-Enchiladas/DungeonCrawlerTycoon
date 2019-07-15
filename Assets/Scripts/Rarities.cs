using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rarities : MonoBehaviour
{
    public static Rarities Instance;
    public Rarity[] rarities;

    private void Awake()
    {
        Instance = this;
    }

    public static Color GetRarityColor(RarityType rarity)
    {
        foreach (Rarity _rarity in Instance.rarities)
        {
            if (rarity == _rarity.rarity)
                return _rarity.color;
        }
        return Color.white;
    }
}

[System.Serializable]
public class Rarity
{
    public RarityType rarity;
    public Color color;
}

public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Artifact
}