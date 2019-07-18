using UnityEngine;

[System.Serializable]
public class Item
{
    [Header("Item Values")]
    public string itemName;
    public RarityType rarity;
    public int maxStackCount;
    public int price;

    public InventoryItem inventoryItem;
}
