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

    public static Rarity GetRandomRarity()
    {
        for(int i = Instance.rarities.Length -1; i > 0; i--)
        {
            float roll = Random.Range(0f, 101f);

            if (roll <= Instance.rarities[i].rarityChance)
                return Instance.rarities[i];
        }

        return Instance.rarities[0];
    }

    public static Rarity GetRarity(int index)
    {
        if (Instance.rarities.Length > index && index >= 0)
            return Instance.rarities[index];
        else return null;
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
    public int statCount;
    public int statPointMin;
    public int statPointMax;
    public float rarityChance;
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