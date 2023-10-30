using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentCombiner : MonoBehaviour
{
    private readonly Dictionary<int, Transform> rootBoneDictionary = new Dictionary<int, Transform>();

    private readonly Transform transform;

    public EquipmentCombiner(GameObject go)
    {
        transform = go.transform;
        TraversHierarchy(transform);
    }

    public Transform AddLimb(GameObject itemGO, List<string> boneNames)
    {
        Transform limb = ProcessBoneObject(itemGO.GetComponentInChildren<SkinnedMeshRenderer>(), boneNames);
        limb.SetParent(transform);
        return limb;
    }

    private Transform ProcessBoneObject(SkinnedMeshRenderer renderer, List<string> boneNames)
    // 스킨드 메쉬로 구성된 아이템에서 사용하는 본드를 매칭해 새롭게 스킨메쉬 렌더러를 생성 후 부모에 추가해줌 
    {
        Transform boneObject = new GameObject().transform;
        
        SkinnedMeshRenderer meshRenderer = boneObject.gameObject.AddComponent<SkinnedMeshRenderer>();

        Transform[] boneTransforms = new Transform[boneNames.Count];
        for (int i = 0; i < boneNames.Count; i++)
        {
            boneTransforms[i] = rootBoneDictionary[boneNames[i].GetHashCode()];
            // rootBoneDictionary에 저장된 본드 이름을 통해 해당 본드를 찾아서 배열에 저장
            meshRenderer.bones = boneTransforms;
            meshRenderer.sharedMesh = renderer.sharedMesh;
            meshRenderer.materials = renderer.sharedMaterials;
        }

        return boneObject;
    }

    public Transform[] AddMesh(GameObject itemGO)  // 스태틱 메쉬를 추가해주기 위함. (장갑같은것은 2개 리턴할때도 있으므로 Transform[]로 선언
    {
        Transform[] itemTransforms = ProcessMeshObject(itemGO.GetComponentsInChildren<MeshRenderer>());
        return itemTransforms;
    }

    private Transform[] ProcessMeshObject(MeshRenderer[] meshRenderers)
    {
        Debug.Log("ProcessMEsh");
        List<Transform> itemTransforms = new List<Transform>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            if (meshRenderer.transform.parent != null)
            {
                Transform parent = rootBoneDictionary[meshRenderer.transform.parent.name.GetHashCode()];
                GameObject itemGO = Instantiate(meshRenderer.gameObject, parent);
                Debug.Log(itemGO.name);
                
                itemTransforms.Add(itemGO.transform);
            }
        }

        return itemTransforms.ToArray();

    }
    private void TraversHierarchy(Transform root)
    {
        foreach (Transform child in root)
        {
            rootBoneDictionary.Add(child.name.GetHashCode(), child);     // GetHashCode 하면 String을 해시화 시켜 Int형으로 바꿔준다.
            TraversHierarchy(child);    // recursive를 통해 모든 하위 객체를 탐색한다.
        }
    }
}
