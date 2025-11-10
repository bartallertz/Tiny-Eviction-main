using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentSlotUI : MonoBehaviour
{
    [SerializeField] EquipmentItemIcon icon = null;

    PlayerInventory playerInventory;
    int index;

    public void EquipItem(PlayerInventory inventory, int index)
    {
        this.playerInventory = inventory;
        this.index = index;
        icon.SetItem(playerInventory.GetItemInSlot(index));
    }

    // Aan het begin van een level kijk of je al een item hebt ge-equipt
    public bool CheckForEquippedItem()
    {
        bool itemEquipped = false;
        this.playerInventory = PlayerInventory.GetPlayerInventory();
        for (int i = 0; i < playerInventory.activeInventory.slots.Count; i++)
        {
            if (playerInventory.GetItemInSlot(i).equipped)
            {
                playerInventory.EquipItem(playerInventory.GetItemInSlot(i), i);
                itemEquipped = true;
            }
        }
        return itemEquipped;
    }

    public void UnequipItem()
    {
        playerInventory.UnequipItem(playerInventory.GetItemInSlot(index), index);
        icon.SetItem(null);

    }
}
