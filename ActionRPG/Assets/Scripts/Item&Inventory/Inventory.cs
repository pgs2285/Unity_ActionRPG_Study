using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Inventory
{
    public InventorySlot[] slots = new InventorySlot[24];

    public void Clear()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.RemoveItem();
        }
    }

    public bool IsContain(ItemObject item)
    {
        return isContain(item.data.id);
        // return Array.Find(slots, i => i.item.id == item.data.id) != null;   // Array.Find : 조건에 맞는 첫번째 요소를 반환한다. foreach문을 사용하지 않고도 조건에 맞는 요소를 찾을 수 있다.
    }

    public bool isContain(int id)
    {
        return slots.FirstOrDefault(i => i.item.id == id) != null;  //FirstOrDefault : 조건에 맞는 첫번째 요소를 반환한다. 없으면 null을 반환한다. 
    }
}
