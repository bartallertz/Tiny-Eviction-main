using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventoryItemSO : ScriptableObject
{
    public int itemID = 0;
    public bool equipped = false;
    public Sprite icon = null;

    static Dictionary<string, InventoryItemSO> itemLookupCache;
}
