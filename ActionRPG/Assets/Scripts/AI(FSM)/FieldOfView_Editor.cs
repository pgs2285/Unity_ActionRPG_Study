using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FieldOfView))]
public class FieldOfView_Editor : Editor
{
    private void OnSceneGUI()
    {
        FieldOfView fov = (FieldOfView)target;  // FieldOfView가 붙은 오브젝트를 fov에 저장한다.
        
        Handles.color = Color.white;
        Handles.DrawWireArc(fov.transform.position, Vector3.up, Vector3.forward, 360, fov.viewRadius);
        // up,forward 는 수직인 원호, 즉 평면인 원을 그린다.

     
        
        float x = Mathf.Sin(fov.viewRadius / 2 * Mathf.Deg2Rad) * fov.viewRadius;
        float z = Mathf.Cos(fov.viewRadius / 2 * Mathf.Deg2Rad) * fov.viewRadius;
        // 바라보는 시야를 삼각형이라 생각해보면, 이미지에서 (x/2)/빗변(반지름) 이므로, sin값에 반지름을 곱해주면x 가 나온다. z도 같은원리.
        
        //  여기선 x,z 값을 사용하지 않고 벡터 자체를 가져올것이므로 Field Of View에서 구현한 DirFromAngles을 호출한다..
        Vector3 viewAngleA = fov.DirFromAngle(-fov.viewAngle / 2, false);
        Vector3 viewAngleB = fov.DirFromAngle(fov.viewAngle / 2, false);
        // readme에 참조한 그림처럼 양 옆 끝 direction vector를 가져온다.
        
        // 이제 구해온 viewAngle값들에 viewRadius 를 곱해주어서 그린다
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleA * fov.viewRadius);
        Handles.DrawLine(fov.transform.position, fov.transform.position + viewAngleB * fov.viewRadius);

        Handles.color = Color.red;
        foreach (Transform visibleTarget in fov.VisibleTargets)
        {
            Handles.DrawLine(fov.transform.position, visibleTarget.position);
        }

    }
    
    
    
}
