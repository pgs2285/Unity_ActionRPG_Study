using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestItems : MonoBehaviour
{
    public ItemDatabase database;
    public InventoryObject inventory;

    public void AddNewItem()
    {
        if(database.ItemObjects.Length > 0)
        {
            ItemObject item = database.ItemObjects[Random.Range(0, database.ItemObjects.Length)];
            Item newItem = new Item(item);

            inventory.AddItem(newItem, 1);
        }
    }

    public void ClearInventory()
    {
        inventory?.Clear();
    }
}
