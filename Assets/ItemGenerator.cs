using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using OuterRimStudios.Utilities;

public class ItemGenerator : MonoBehaviour
{
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI itemInfo;
    public TextMeshProUGUI extraStat;
    public TextMeshProUGUI target;

    [Space]
    public TextMeshProUGUI Power;
    public TextMeshProUGUI Accuracy;
    public TextMeshProUGUI Constituion;
    public TextMeshProUGUI Speed;
    public TextMeshProUGUI Luck;

    public Image icon;

    public void GenerateItem()
    {
        int pow = 0;
        int acc = 0;
        int con = 0;
        int spd = 0;
        int lck = 0;

        Power.gameObject.SetActive(false);
        Accuracy.gameObject.SetActive(false);
        Constituion.gameObject.SetActive(false);
        Speed.gameObject.SetActive(false);
        Luck.gameObject.SetActive(false);

        itemName.text = "Helm of Wrath";

        Rarity rarity = Rarities.GetRandomRarity();

        itemName.color = rarity.color;
        itemInfo.color = rarity.color;

        itemInfo.text = rarity.rarity.ToString() + " Heavy Helmet";

        extraStat.text = "24 Armor";
        target.gameObject.SetActive(false);

        List<int> tempStats = new List<int>() { 1, 2, 3, 4, 5 };
        List<int> stats = CollectionUtilities.GetRandomItems(tempStats, rarity.statCount);

        print(rarity.rarity.ToString());

        int statCount = Random.Range(rarity.statPointMin, rarity.statPointMax + 1);
        for (int i = 0; i < statCount; i++)
        {
            int stat = CollectionUtilities.GetRandomItem(stats);

            switch (stat)
            {
                case 1:
                    pow++;

                    Power.gameObject.SetActive(true);
                    Power.text = pow + " Power";
                    break;
                case 2:
                    acc++;

                    Accuracy.gameObject.SetActive(true);
                    Accuracy.text = acc + " Accuracy";
                    break;
                case 3:
                    con++;

                    Constituion.gameObject.SetActive(true);
                    Constituion.text = con + " Constituion";
                    break;
                case 4:
                    spd++;

                    Speed.gameObject.SetActive(true);
                    Speed.text = spd + " Speed";
                    break;
                case 5:
                    lck++;

                    Luck.gameObject.SetActive(true);
                    Luck.text = lck + " Luck";
                    break;
            }
        }

        if (rarity.rarity == RarityType.Artifact)
            Debug.LogError("FOUND IT");
    }

    void SamGeneration()
    {
        int pow = 0;
        int acc = 0;
        int con = 0;
        int spd = 0;
        int lck = 0;

        Power.gameObject.SetActive(false);
        Accuracy.gameObject.SetActive(false);
        Constituion.gameObject.SetActive(false);
        Speed.gameObject.SetActive(false);
        Luck.gameObject.SetActive(false);

        itemName.text = "Helm of Wrath";
        itemInfo.text = "Rare Heavy Helmet";

        itemName.color = Rarities.GetRarityColor(RarityType.Rare);
        itemInfo.color = Rarities.GetRarityColor(RarityType.Rare);

        extraStat.text = "24 Armor";
        target.gameObject.SetActive(false);

        for (int i = 0; i < 8; i++)
        {
            int stat = Random.Range(1, 6);

            switch (stat)
            {
                case 1:
                    pow++;

                    Power.gameObject.SetActive(true);
                    Power.text = pow + " Power";
                    break;
                case 2:
                    acc++;

                    Accuracy.gameObject.SetActive(true);
                    Accuracy.text = acc + " Accuracy";
                    break;
                case 3:
                    con++;

                    Constituion.gameObject.SetActive(true);
                    Constituion.text = con + " Constituion";
                    break;
                case 4:
                    spd++;

                    Speed.gameObject.SetActive(true);
                    Speed.text = spd + " Speed";
                    break;
                case 5:
                    lck++;

                    Luck.gameObject.SetActive(true);
                    Luck.text = lck + " Luck";
                    break;
            }
        }
    }
}
