using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInventoryUI : InventoryUI
{
    public GameObject[] staticSlots = null;

    public override void CreateSlotUIs()
    {
        slotsUI = new Dictionary<GameObject, InventorySlot>();
        for(int i = 0; i < inventoryObject.Slots.Length;i++)
        {
            GameObject go = staticSlots[i];
            AddEvent(go, EventTriggerType.PointerEnter, delegate { OnEnterSlot(go); });
            AddEvent(go, EventTriggerType.PointerExit, delegate { OnExitSlot(go); });
            AddEvent(go, EventTriggerType.BeginDrag, delegate { OnStartDrag(go); });
            AddEvent(go, EventTriggerType.EndDrag, delegate { OnExitDrag(go); });
            AddEvent(go, EventTriggerType.Drag, delegate { OnDrag(go); });

            inventoryObject.Slots[i].slotUI = go;
            slotsUI.Add(go, inventoryObject.Slots[i]);

            go.name += " : " + i;
        }
    }
}
