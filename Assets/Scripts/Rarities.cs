using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Rarities")]
public class Rarities : ScriptableObject
{
    public Rarity[] rarities;

    public Rarity GetRandomRarity()
    {
        for(int i = rarities.Length -1; i > 0; i--)
        {
            float roll = Random.Range(0f, 101f);

            if (roll <= rarities[i].rarityChance)
                return rarities[i];
        }

        return rarities[0];
    }

    public Rarity GetRarity(int index)
    {
        if (rarities.Length > index && index >= 0)
            return rarities[index];
        else return null;
    }

    public Color GetRarityColor(RarityType rarity)
    {
        foreach (Rarity _rarity in rarities)
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