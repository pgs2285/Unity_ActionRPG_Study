using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public InventoryObject equipment;

    public EquipmentCombiner combiner;

    private ItemInstances[] itemInstances = new ItemInstances[8];

    public ItemObject[] defaultItemObject = new ItemObject[8];

    private void Awake()
    {
        combiner = new EquipmentCombiner(gameObject);
        for (int i = 0; i < equipment.Slots.Length; i++)
        {
            equipment.Slots[i].onPreUpdate += OnRemoveItem;
            equipment.Slots[i].onPostUpdate += OnEquipItem;
        }
    }

    void Start()
    {
        foreach(InventorySlot slot in equipment.Slots)
        {
            OnEquipItem(slot);
        }
    }

    private void OnEquipItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if (itemObject == null)
        {
            EquipDefaultItem(slot.allowedItems[0]);
            return;
        }

        int index = (int)slot.allowedItems[0];
        switch (slot.allowedItems[0])
        {
            case ItemType.Boots:
            case ItemType.Chest:
            case ItemType.Helmet:
            case ItemType.Gloves:
            case ItemType.Pants:
            case ItemType.Pauldrons:
            case ItemType.RightWeapon:
            case ItemType.LeftWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }
    }

    private ItemInstances EquipSkinnedItem(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            return null;
        }

        Transform itemTransform = combiner.AddLimb((itemObject.modelPrefab), itemObject.boneNames);

        if (itemTransform != null)
        {
            ItemInstances instance = new ItemInstances();
            instance.ItemTransforms.Add((itemTransform));
            return instance;
        }

        return null;
    }
    
    private ItemInstances EquipMeshItem(ItemObject itemObject)
    {
        if (itemObject == null)
        {
            return null;
        }

        Transform[] itemTransforms = combiner.AddMesh(itemObject.modelPrefab);

        if (itemTransforms.Length > 0)
        {
            ItemInstances instance = new ItemInstances();
            instance.ItemTransforms.AddRange(itemTransforms.ToList<Transform>());   // AddRange : 리스트에 배열을 추가한다.
            return instance;
        }

        return null;
    }

    private void OnRemoveItem(InventorySlot slot)
    {
        ItemObject itemObject = slot.ItemObject;
        if (itemObject == null)
        {
            RemoveItemBy(slot.allowedItems[0]);
            return;
        }

        if (slot.ItemObject.modelPrefab != null)
        {
            RemoveItemBy(slot.allowedItems[0]);
        }
    }

    private void RemoveItemBy(ItemType type)
    {
        int index = (int)type;
        if(itemInstances[index] != null)
        {
            itemInstances[index].OnDestroy();
            itemInstances[index] = null;
        }
    }

    private void EquipDefaultItem(ItemType type)
    {
        int index = (int)type;
        ItemObject itemObject = defaultItemObject[index];        
        
        switch (type)
        {
            case ItemType.Boots:
            case ItemType.Chest:
            case ItemType.Helmet:
            case ItemType.Gloves:
            case ItemType.Pants:
            case ItemType.Pauldrons:
            case ItemType.RightWeapon:
            case ItemType.LeftWeapon:
                itemInstances[index] = EquipMeshItem(itemObject);
                break;
        }
    }
    private void OnDestroy()
    {
        foreach (ItemInstances item in itemInstances)
        {
            item.OnDestroy();
        }
    }
}
