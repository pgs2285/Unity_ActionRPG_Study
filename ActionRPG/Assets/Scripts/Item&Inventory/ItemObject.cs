using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType : int
{
    Helmet = 0,
    Chest = 1,
    Pants = 2,
    Boots = 3,
    Pauldrons = 4,
    Gloves = 5,
    LeftWeapon = 6,
    RightWeapon = 7,
    Food,
    Default
}


[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Items/New Item")]
public class ItemObject : ScriptableObject  // ScriptableObject : 클래스 인스턴스와는 별도로 대량의 데이터를 저장하는 데 사용할 수 있는 데이터 컨테이너. 값의 사본이 생성되지 않는다.
{
    public ItemType type;
    public bool stackable;

    public Sprite icon;
    public GameObject modelPrefab;

    public Item data = new Item();
    public List<string> boneNames = new List<string>();
    
    [TextArea(15,20)]   // TextArea(15,20) : 15줄 20칸의 텍스트 영역을 만들어준다.
    public string description;

    void OnValidate()   // 데이터가 변경될때 호출된다.
    {
        boneNames.Clear();
        if (modelPrefab == null || modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>() == null) // SkinnedMeshRenderer : 뼈대를 가진 메쉬를 렌더링하는 컴포넌트
        {
            return;
        }
        SkinnedMeshRenderer renderer = modelPrefab.GetComponentInChildren<SkinnedMeshRenderer>();
        Transform[] bones = renderer.bones;
        
        foreach (Transform t in bones)
        {
            boneNames.Add(t.name);
        }
    }

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
}
