using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemIcon : MonoBehaviour
{

    public void SetItem(InventoryItemSO item)
    {
        Image iconImage = GetComponent<Image>();
        Button button = GetComponent<Button>();
        if (item == null || item.equipped)
        {
            iconImage.enabled = false;
            button.enabled = false;
        }
        else
        {
            iconImage.enabled = true;
            button.enabled = true;
            iconImage.sprite = item.icon;
        }
    }
}
