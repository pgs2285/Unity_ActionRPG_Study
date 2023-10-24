using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class facingCamera : MonoBehaviour
{
    #region Variables

    public bool reverseFace = false; // 카메라가 보는 방향과 반대로 보여줄 것인가?
    private Camera referenceCamera;

    public enum Axis
    {
        up,
        down,
        left,
        right,
        forward,
        back
    };   //어느 축을 기준으로 할 것인가
    public Axis axis= Axis.up; // 기본값은 up

    #endregion Variables

    public Vector3 GetAxis(Axis refAxis)
    {
        switch (refAxis)
        {
            case Axis.up:
                return Vector3.up;
            case Axis.down:
                return Vector3.down;
            case Axis.left:
                return Vector3.left;
            case Axis.right:
                return Vector3.right;
            case Axis.forward:
                return Vector3.forward;
            case Axis.back:
                return Vector3.back;
        }
        return Vector3.up;
    }

    private void Awake()
    {
        if (!referenceCamera)
        {
            referenceCamera = Camera.main;
        }
    }

    private void LateUpdate()
    {
        Vector3 targetPos = transform.position + referenceCamera.transform.rotation * (reverseFace ? Vector3.forward : Vector3.back); 
        // 현재 위치에, 카메라의 회전값을 곱해준다.(좌우 대칭여부) 이때, reverseFace 가 true라면 Vector3.forward를 곱해주고, false라면 Vector3.back을 곱해준다.
        
        Vector3 targetOrientation = referenceCamera.transform.rotation * GetAxis(axis); // 카메라의 회전값을 곱해준다. 이때, axis에 따라 다른 축을 곱해준다.
        transform.LookAt(targetPos, targetOrientation); // targetPos를 바라보고, targetOrientation을 up벡터로 설정.
    }
}
