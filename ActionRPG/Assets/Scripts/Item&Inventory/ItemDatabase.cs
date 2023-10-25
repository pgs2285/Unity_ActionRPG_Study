using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemDatabase" , menuName = "Inventory System/Items/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{

   
    public ItemObject[] ItemObjects;

    public void OnValidate()
    { 
        for(int i =0; i < ItemObjects.Length; i++)
        {
            ItemObjects[i].data.id = i;
        }
    }
}
