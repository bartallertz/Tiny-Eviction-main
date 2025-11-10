using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    InventorySlotUI inventoryItemPrefab;

    public InventorySO inventory;

    PlayerInventory playerInventory;

    private void Awake()
    {
        playerInventory = PlayerInventory.GetPlayerInventory();
        playerInventory.inventoryUpdated += Redraw;
        inventoryItemPrefab = GetComponentInChildren<InventorySlotUI>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Redraw();
    }

    public void Redraw()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        if (inventoryItemPrefab == null)
        {
            inventoryItemPrefab = GetComponentInChildren<InventorySlotUI>();
        }

        for (int i = 0; i < 4; i++)
        {
            InventorySlotUI itemUI = Instantiate(inventoryItemPrefab, transform);
            itemUI.SetUp(playerInventory, i);
        }
    }

}
