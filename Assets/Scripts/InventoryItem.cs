using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public Image icon;
    public ItemDescription itemDescription;

    private void Start()
    {
        Transform itemDescriptions = GameObject.Find("OverlayCanvas").transform;
        itemDescription.transform.SetParent(itemDescriptions);
        itemDescription.transform.localPosition = transform.localPosition;
    }

    public void SetActive(bool state)
    {
        itemDescription.gameObject.SetActive(state);
    }
}
