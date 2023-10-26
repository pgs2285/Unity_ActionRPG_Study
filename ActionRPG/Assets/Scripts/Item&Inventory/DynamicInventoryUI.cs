using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInventoryUI : InventoryUI
{
    [SerializeField]
    protected GameObject slotPrefab;

    [SerializeField]
    protected Vector2 start;
    
    [SerializeField]
    protected Vector2 size;
    
    [SerializeField]
    protected Vector2 space;

    [Min(1), SerializeField]
    protected int numberOfColumns = 4;

    public override void CreateSlotUIs()
    {
        slotsUI = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventoryObject.Slots.Length; ++i)
        {
            GameObject go = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);//  4번쨰 인자는 부모를 지정하는 것
            go.GetComponent<RectTransform>().anchoredPosition = CaculatePosition(i);

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

    public Vector3 CaculatePosition(int i)
    {
        float x = start.x + (space.x + size.x) * (i % numberOfColumns);
        float y = start.y + (-(space.y + size.y)) * (i / numberOfColumns);
        return new Vector3(x,y,0);
    }
}

