using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject, ISerializationCallbackReceiver
{
    public string savePath;
    private InventoryDatabaseSO inventoryDatabase;
    public List<InventorySlot> slots = new List<InventorySlot>();

    private void OnEnable()
    {
#if UNITY_EDITOR
        inventoryDatabase = (InventoryDatabaseSO)AssetDatabase.LoadAssetAtPath("Assets/Resources/Database.asset", typeof(InventoryDatabaseSO));
#else
        inventoryDatabase = Resources.Load<InventoryDatabaseSO>("Database");
#endif
    }

    public void AddItem(InventoryItemSO item)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].item == item)
            {
                return;
            }
        }
        slots.Add(new InventorySlot(inventoryDatabase.GetId[item], item));
    }

    public void Save()
    {
        string saveData = JsonUtility.ToJson(this, true);
        BinaryFormatter BF = new BinaryFormatter();
        FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        BF.Serialize(file, saveData);
        file.Close();
    }

    public void Load()
    {
        if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        {
            BinaryFormatter BF = new BinaryFormatter();
            FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
            JsonUtility.FromJsonOverwrite(BF.Deserialize(file).ToString(), this);
            file.Close();
        }
    }

    public int GetItemID(InventoryItemSO item)
    {
        return inventoryDatabase.GetId[item];
    }

    // Zet item.equipped terug naar false voor items die je ge-equipt hebt maar daarna verloren hebt omdat je een level niet gehaald hebt
    public void CheckForLostEquippedItem()
    {
        for (int i = 0; i < inventoryDatabase.items.Length; i++)
        {
            bool inInventory = false;
            if (inventoryDatabase.items[i].equipped)
            {
                for (int j = 0; j < slots.Count; j++)
                {
                    if (slots[j].item == inventoryDatabase.items[i])
                    {
                        inInventory = true;
                    }
                }
                if (!inInventory)
                {
                    inventoryDatabase.items[i].equipped = false;
                }
            }
        }
    }

    public void OnAfterDeserialize()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].item = inventoryDatabase.GetItem[slots[i].ID];
        }
    }

    public void OnBeforeSerialize()
    {
    }
}

[System.Serializable]
public class InventorySlot
{
    public int ID;
    public InventoryItemSO item;

    public InventorySlot(int id, InventoryItemSO item)
    {
        this.ID = id;
        this.item = item;
    }
}
