using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] InventoryItemIcon icon = null;

    PlayerInventory playerInventory;
    int index;

    public void SetUp(PlayerInventory inventory, int index)
    {
        this.playerInventory = inventory;
        this.index = index;
        icon.SetItem(playerInventory.GetItemInSlot(index));
    }

    public void EquipItem()
    {
        playerInventory.EquipItem(playerInventory.GetItemInSlot(index), index);
    }
}
