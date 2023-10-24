using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPicker : MonoBehaviour
{
    public float surfaceOffset = 1.5f; // 간섭현상을 방지하기 위한 오프셋
    public Transform target = null;

    private void Update()
    {
        if(target)
        {
            transform.position = target.position + Vector3.up * surfaceOffset;
        }
    }

    public void SetPosition(RaycastHit hit)
    {
        target = null;
        transform.position = hit.point + hit.normal * surfaceOffset;    // 법선 벡터를 이용해 오프셋을 적용한 위치로 이동
    }

}
