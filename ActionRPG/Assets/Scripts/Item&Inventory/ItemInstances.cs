using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInstances
{
    public List<Transform> ItemTransforms = new List<Transform>();

    public void OnDestroy()
    {
        foreach(Transform item in ItemTransforms)
        {
            GameObject.Destroy(item.gameObject);
        }
    }
}
