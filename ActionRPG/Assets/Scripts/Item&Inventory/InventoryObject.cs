using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InterfaceType{
    Inventory,
    Equipment,
    QuickSlot,
    Box,

}

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    public ItemDatabase database;
    public InterfaceType type;

    [SerializeField]
    private Inventory container = new Inventory();
    public InventorySlot[] Slots => container.slots;

    public int EmptySlotCount{
        get{
            int count = 0;
            foreach(InventorySlot slot in Slots)
            {
                if(slot.item.id <= -1)
                {
                    count++;
                }
            }
            return count;
        }
    }

    public bool AddItem(Item item, int amount)
    {
        if(EmptySlotCount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindItemInInventory(item);
        if(!database.ItemObjects[item.id].stackable || slot == null)    // 아이템이 겹쳐지지 않거나 슬롯에 없다면
        {
            GetEmptySlot().AddItem(item, amount);
        }else{
            slot.AddAmount(amount);
        }
        return true;
    }

    public InventorySlot FindItemInInventory(Item item) {
        return Slots.FirstOrDefault(x => x.item.id == item.id); // firstOrDefault : 조건에 맞는 첫번째 요소를 반환한다. 없으면 null을 반환한다.
    }

    public InventorySlot GetEmptySlot()
    {
        return Slots.FirstOrDefault(x => x.item.id <= -1); 
    }

    public bool IsContainItem(ItemObject item)
    {
        return Slots.FirstOrDefault(x=>x.item.id == item.data.id) != null;
    }
    public void SwapItems(InventorySlot a, InventorySlot b)
    {
        if(a == b)
        {
            return ;
        }
        if(b.CanPlaceInSlot(a.ItemObject) && a.CanPlaceInSlot(b.ItemObject))
        {
            InventorySlot tmp = new InventorySlot(b.item, b.amount);
            b.UpdateSlot(a.item, a.amount);
            a.UpdateSlot(tmp.item, tmp.amount);
        }
    }
}
