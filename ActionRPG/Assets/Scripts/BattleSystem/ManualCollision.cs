using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualCollision : MonoBehaviour
{

    public Vector3 boxSize = new Vector3(2,2,2);
    public Vector3 Offset = Vector3.zero;
    public Collider[] CheckOverlapBox(LayerMask layermask)
    {
        Debug.Log( Physics.OverlapBox(transform.position + Offset, boxSize * 0.5f, transform.rotation, layermask));
        return Physics.OverlapBox(transform.position + Offset, boxSize * 0.5f, transform.rotation, layermask);// boxSize * 0.5f 는 boxSize 의 절반 크기를 의미한다.
        
    }
    private void OnDrawGizmos() {
        Gizmos.matrix = transform.localToWorldMatrix; // 로컬 좌표계를 월드 좌표계로 변환한다.
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero + Offset, boxSize);// 중심점이 0,0,0 이고 크기가 boxSize 인 큐브를 그린다.
    }
}
