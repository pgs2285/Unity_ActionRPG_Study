using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class MouseData
{
    public static InventoryUI interfaceMouseIsOver;
    public static GameObject slotHoveredOver;
    public static GameObject tmpItemBeingDragged;
}

[RequireComponent(typeof(EventTrigger))]
public abstract class InventoryUI : MonoBehaviour
{
    public InventoryObject inventoryObject;
    private InventoryObject previousInventoryObject;
    
    public Dictionary<GameObject, InventorySlot> slotsUI = new Dictionary<GameObject, InventorySlot>();

    private void Awake()
    {
        CreateSlotUIs();

        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            inventoryObject.Slots[i].parent = inventoryObject;
            inventoryObject.Slots[i].OnPostUpdate += OnPostUpdate;  // icon괴 개수에 대한 UI를 갱신해준디..
        
        // Drag & Drop 이벤트가 어디서 발생했는지 알기 위해서
         AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject);});   
         AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject);});   
        
    }

    protected virtual void Start()
    {
        for (int i = 0; i < inventoryObject.Slots.Length; i++)
        {
            inventoryObject.Slots[i].UpdateSlot(inventoryObject.Slots[i].item, inventoryObject.Slots[i].amount);
        }
    }
    
    public abstract void CreateSlotUIs();

    protected void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = go.GetComponent<EventTrigger>();
        if (!trigger)
        {
            Debug.LogWarning("No EventTrigger found on " + go.name + ", adding one.");
            return;
        }

        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);

    }

    public void OnPostUpdate(InventorySlot slot)
    {
        Image img = slot.slotUI.transform.GetChild(0).GetComponent<Image>();
        img.sprite =
            slot.item.id < 0 ? null : slot.ItemObject.icon;
        img.color = 
            slot.item.id < 0 ? new Color(1, 1, 1, 0) : new Color(1, 1, 1, 1);

        slot.slotUI.GetComponentInChildren<TextMeshProUGUI>().text =
            slot.item.id < 0 ? string.Empty : slot.amount.ToString("n0");
        
        
    }

    public void OnEnterInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = go.GetComponent<InventoryUI>();
    }

    public void OnExitInterface(GameObject go)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnEnterSlot(GameObject go)  // 슬롯에 대해 OnEnter이지만, 모두 같은 코드를 공유할것이므로 여기서 구현한다.
    {
        MouseData.slotHoveredOver = go;
    }

    public void OnExitSlot(GameObject go)
    {
        MouseData.slotHoveredOver = null;
    }

    public GameObject CreateDragImage(GameObject go)
    {
        if (slotsUI[go].item.id < 0)
        {
            return null;
        }

        GameObject dragImage = new GameObject();
        RectTransform rectTransform = dragImage.AddComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(50, 50);
        dragImage.transform.SetParent(transform.parent);    // Canvas
        Image image = dragImage.AddComponent<Image>();
        image.sprite = slotsUI[go].ItemObject.icon;
        image.raycastTarget = false;    // 마우스 이벤트를 무시한다.
        dragImage.name = "Drag Image";

        return dragImage;
        
    }
    public void OnStartDrag(GameObject go)
    {
        MouseData.tmpItemBeingDragged = CreateDragImage(go);
    }

    public void OnDrag(GameObject go)
    {
        if (MouseData.tmpItemBeingDragged == null)
        {
            return;
        }

        MouseData.tmpItemBeingDragged.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnExitDrag(GameObject go)
    {
        
    }
}
