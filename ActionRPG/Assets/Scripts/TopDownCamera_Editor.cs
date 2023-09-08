using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace JS.Cameras{
    [CustomEditor(typeof(TopDownCamera))]
    public class TopDownCamera_Editor : Editor
    {
        #region Variables
        private TopDownCamera targetCamera;
        #endregion Variables

        public override void OnInspectorGUI()
        {
            targetCamera = (TopDownCamera)target; // target은 editor제공해주는 변수로 현재 인스펙터에 있는 컴포넌트를 가리킨다.
            base.OnInspectorGUI();
        }

        private void OnSceneGUI()
        {
            if(!targetCamera || !targetCamera.Target) return;

            Transform cameraTarget = targetCamera.Target;   // 타겟의 트랜스폼을 가져온다.
            Vector3 targetPosition = cameraTarget.position; // 타겟의 포지션을 가져온다.
            targetPosition.y = targetCamera.lookAtHeight;   // 타겟의 높이를 가져온다.
            
            Handles.color = new Color(1f,0f,0f,0.15f);    // 핸들의 색상을 설정한다.
            Handles.DrawSolidDisc(targetPosition, Vector3.up, targetCamera.distance);

            Handles.color = new Color(0f,1f,0f,0.75f);    // 핸들의 색상을 설정한다.
            Handles.DrawWireDisc(targetPosition, Vector3.up, targetCamera.distance);
             
            Handles.color = new Color(1f,0f,0f,0.5f);
            targetCamera.distance = Handles.ScaleSlider(targetCamera.distance,
            targetPosition,
            -cameraTarget.forward,
            Quaternion.identity,
            targetCamera.distance,
             0.1f);

            targetCamera.distance = Mathf.Clamp(targetCamera.distance, 2f, float.MaxValue);    // 2f~ float가 가질수 있는 최대값사이의 값으로 제한.

            Handles.color = new Color(0f,1f,0f,0.5f);
            targetCamera.height = Handles.ScaleSlider(targetCamera.height,
            targetPosition,
            Vector3.up,
            Quaternion.identity,
            targetCamera.height,
            0.1f);

            targetCamera.height = Mathf.Clamp(targetCamera.height, 2f, float.MaxValue);    // 2f~ float가 가질수 있는 최대값사이의 값으로 제한.

            GUIStyle labelStyle = new GUIStyle();
            labelStyle.fontSize = 15;
            labelStyle.normal.textColor = Color.white;
            labelStyle.alignment = TextAnchor.MiddleCenter;

            Handles.Label(targetPosition +(-cameraTarget.forward * targetCamera.distance), "Distance", labelStyle);

            labelStyle.alignment = TextAnchor.MiddleRight;
            Handles.Label(targetPosition + (Vector3.up * targetCamera.height),"Height", labelStyle);

            targetCamera.HandleCamera();
        }
    }
}
