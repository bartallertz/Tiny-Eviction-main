using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryDatabaseSO : ScriptableObject, ISerializationCallbackReceiver
{
    public InventoryItemSO[] items;
    public Dictionary<InventoryItemSO, int> GetId = new Dictionary<InventoryItemSO, int>();
    public Dictionary<int, InventoryItemSO> GetItem = new Dictionary<int, InventoryItemSO>();

    public void OnAfterDeserialize()
    {
        GetId = new Dictionary<InventoryItemSO, int>();
        GetItem = new Dictionary<int, InventoryItemSO>();
        for (int i = 0; i < items.Length; i++)
        {
            GetId.Add(items[i], i);
            GetItem.Add(i, items[i]);
        }
    }

    public void OnBeforeSerialize()
    {
        
    }
}
