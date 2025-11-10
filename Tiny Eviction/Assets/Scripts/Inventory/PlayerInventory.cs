using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public InventorySO actualInventory;
    public InventorySO fullInventory;
    [SerializeField] InventoryUI inventoryUI;
    [SerializeField] EquipmentSlotUI equipmentSlotUI;
    [SerializeField] InventorySlotUI inventorySlotUI;

    PlayerMovement playerMovement;

    [HideInInspector] public InventorySO activeInventory;
    private bool inventoryStateFromPlayerPrefs;

    private void Start()
    {
        // Preload both inventories
        actualInventory.Load();
        fullInventory.Load();

        assignInventory();
        playerMovement = GetComponent<PlayerMovement>();
        EquippedItem();
    }

    private void Update()
    {
        // Reload new inventory in case settings have been changed
        if (inventoryStateFromPlayerPrefs != PlayerPrefs.HasKey("hasAllItemsUnlocked")){
            assignInventory();
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            activeInventory.Save();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("PowerUp"))
        {
            InventoryItem item = collision.GetComponent<InventoryItem>();
            if (item)
            {
                AddItem(item.item);
                bool itemEquipped = equipmentSlotUI.CheckForEquippedItem();
                if (!itemEquipped)
                {
                    EquipItem(item.item, activeInventory.GetItemID(item.item));
                }
                inventoryUpdated?.Invoke();
                Destroy(collision.gameObject);
            }
        }
    }

    public event Action inventoryUpdated;

    public static PlayerInventory GetPlayerInventory()
    {
        GameObject player = GameObject.FindWithTag("Player");
        return player.GetComponent<PlayerInventory>();
    }

    public void AddItem(InventoryItemSO item)
    {
        activeInventory.AddItem(item);
    }

    public InventoryItemSO GetItemInSlot(int index)
    {
        if (activeInventory.slots.Count > index)
        {
            return activeInventory.slots[index].item;
        }
        return null;
    }

    public void EquipItem(InventoryItemSO item, int index)
    {
        for (int i = 0; i < activeInventory.slots.Count; i++)
        {
            activeInventory.slots[i].item.equipped = false;
        }
        item.equipped = true;
        playerMovement.setPowerUp(item.itemID);
        equipmentSlotUI.EquipItem(GetPlayerInventory(), index);
        inventoryUpdated?.Invoke();
    }

    public void UnequipItem(InventoryItemSO item, int index)
    {
        item.equipped = false;
        playerMovement.setPowerUp(-1);
        inventoryUpdated?.Invoke();
    }

    // Checks which power ups have equipped set to true then checks whether said power up is still in inventory
    // If not it sets equipped to false, otherwise it equips it
    public void EquippedItem()
    {
        activeInventory.CheckForLostEquippedItem();
        equipmentSlotUI.CheckForEquippedItem();
    }

    // Assign correct inventory depending wether cheat is active or not
    private void assignInventory(){
        inventoryStateFromPlayerPrefs = PlayerPrefs.HasKey("hasAllItemsUnlocked");
        if (inventoryStateFromPlayerPrefs) {
            activeInventory = fullInventory;
            inventoryUpdated?.Invoke();
        } else {
            activeInventory = actualInventory;
            inventoryUpdated?.Invoke();
        }
    }
}
