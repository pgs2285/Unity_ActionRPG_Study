using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    public ItemType[] allowedItems = new ItemType[0];
    
    
    //NonSerialized : 직렬화를 하지 않을 변수에 붙이는 특성, 나중에 초기화 해주는 부분이라 직렬화 필요가 없다  
    [NonSerialized]public InventoryObject parent;  // 이 슬롯을 가지고 있는 인벤토리, 추후에 json으로 저장할때는 제외시킨다.
    [NonSerialized]public GameObject slotUI;   // 슬롯의 UI
    //밑 두가지는 슬롯에 아이템이 갱신이 될때 부가적인 처리를 위해서 만든 변수들이다.
    [NonSerialized]public Action<InventorySlot> onPreUpdate; // 슬롯이 업데이트 되기 전에 호출되는 함수. Action은 델리게이트로서 함수를 변수처럼 사용할 수 있다.
    [NonSerialized]public Action<InventorySlot> onPostUpdate; // 슬롯이 업데이트 된 후에 호출되는 함수

    public Item item;
    public int amount;

    public ItemObject ItemObject
    {
        get
        {
            return item.id >= 0 ? parent.database.itemObjects[item.id] : null;
        }
    }
    // => 는 람다식. 매개변수가 없어서 괄호가 없다.
    public InventorySlot() => UpdateSlot(new Item(), 0);
    public InventorySlot(Item item, int amout) => UpdateSlot(item, amout);
    public void RemoveItem() => UpdateSlot(new Item(), 0);
    public void AddAmount(int value) => UpdateSlot(item, amount += value);

    public void UpdateSlot(Item item, int amount)    // 추후에 드래그엔 드롭등 상호작용할때 호출되는 코드
    {
        onPreUpdate?.Invoke(this);  // invoke : 델리게이트를 호출한다. onPreUpdate가 null이 아니면 호출한다.
        this.item = item;
        this.amount = amount;
        onPostUpdate?.Invoke(this);
    }

    public bool CanPlaceInSlot(ItemObject itemObject)
    {
        if(allowedItems.Length <= 0 || itemObject == null || itemObject.data.id < 0)
        {   // 이 조건을 만족하면 아이템이 비어있는것
            return true;
        }

        foreach (ItemType type in allowedItems)
        {
            if (itemObject.type == type)
            {
                return true;
            }
        }
        return false;
    }

}
