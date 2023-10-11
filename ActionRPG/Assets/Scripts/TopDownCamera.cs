using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JS.Cameras{
    public class TopDownCamera : MonoBehaviour
    {
        #region Varaibles
        public float height = 5f;
        public float distance = 10f;
        public float angle = 45f;
        public float lookAtHeight = 2f;
        public float smoothSpeed = 0.5f;
        private Vector3 refVelocity;
        public Transform Target;
        #endregion Variables
        
        void LateUpdate()
        {
            HandleCamera();
        }

        public void HandleCamera()
        {
            if(!Target) return;

            // 카메라의 월드포지션 계산.
            Vector3 worldPosition = (Vector3.forward * -distance) + (Vector3.up * height); // 타겟 뒤쪽을 잡기위해 farword의 distance를 곱해주고, 위로 height만큼 올려준다.
            // Debug.DrawLine(Target.position, worldPosition, Color.red);

            Vector3 rotatedVector = Quaternion.AngleAxis(angle, Vector3.up) * worldPosition; // AngleAxis는 각도와 축을 받아서 회전시키는 함수
            // Debug.DrawLine(Target.position, rotatedVector, Color.green);

            Vector3 finalTargetPosition = Target.position;
            finalTargetPosition.y += lookAtHeight; // 캐릭터의 머리를 바라보게 하기 위해 높이를 더해준다.

            Vector3 finalPosition = finalTargetPosition + rotatedVector; 
            // Debug.DrawLine(Target.position, finalPosition, Color.blue);

            // transform.position = Vector3.SmoothDamp(transform.position, finalPosition, ref refVelocity, smoothSpeed); // SmoothDamp는 현재위치, 목표위치, 속도를 받아서 부드럽게 이동시켜주는 함수
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, finalPosition, smoothSpeed);
            transform.position = smoothedPosition;
            transform.LookAt(Target.position);  // 카메라가 타깃의 포지션을 바라보게함
        }



    }
}
