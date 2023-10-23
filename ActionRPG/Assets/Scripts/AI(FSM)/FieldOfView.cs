using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{

    #region Variables

    public float viewRadius = 5f;
    [Range(0,360)]                      // viewAngle은 0~360도 사이의 값만 가질 수 있게 한다.
    public float viewAngle = 90f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;    // 장애물 뒤에 있는 타겟은 볼 수 없으므로, 장애물 레이어를 추가한다.
    
    private List<Transform> visibleTargets = new List<Transform>();      // 시야에 들어온 타겟을 리스트에 추가한다.
    public List<Transform> VisibleTargets => visibleTargets;
    
    private Transform nearestTarget = null;    // 가장 가까운 타겟을 저장한다.
    public Transform NearestTarget => nearestTarget;
    private float distanceToTarget = 0f;      // 가장 가까운 타겟과의 거리를 저장한다.

    public float delay = 0.2f;

    #endregion Variables
    
    #region Methods

    IEnumerator DelayFindVisibleTarget(float delay)
    {
        while (true)
        {
            
            yield return new WaitForSeconds(delay);
            findVisibleTarget();
        }
    }
    void findVisibleTarget() // 시야에 들어온 타겟을 찾는 함수.
    {
        visibleTargets.Clear();
        nearestTarget = null;
        distanceToTarget = 0f;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); // 시야 반경 내에 있는 타겟을 배열에 저장한다.
        // Debug.Log("감지범위 : "+targetInViewRadius.Length);
        for (int i = 0; i < targetInViewRadius.Length; ++i)
        {
            Transform target = targetInViewRadius[i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized; // 타겟과 자신의 위치를 빼고, 정규화하면, 자신과 타겟 사이의 방향 벡터가 된다.
            // 방항벡터값 = 목표벡터 - 현재벡터
            // Debug.Log("Angle : " + Vector3.Angle(transform.forward, dirToTarget));
            if (Vector3.Angle(transform.forward, dirToTarget) < (viewAngle / 2)) // 현재 캐릭터가 바라보는 방향벡터와, 위에서 타겟까지의 방향벡터의 각도를 구하고, 그것이 시야각의  / 2 보다 작다면 
            // 나누기 2 하는 이유는 기준으로 좌우 절대값으로 구하려 하기 때문이다.
            {
                // Debug.Log("FOV 범위 : "+targetInViewRadius.Length);
                float distToTarget = Vector3.Distance(transform.position, target.position); // 자신과 타겟 사이의 거리를 구한다.
                if (!Physics.Raycast(transform.position, target.position, distToTarget, obstacleMask)) // 내 위치에서 타겟까지 distToTarget만큼의 Ray를 쐈을때 장애물이 검출되지 않으면,
                { // 이 조건을 최종 통과하면 타겟은 자신의 시야안에 존재한다.
                    visibleTargets.Add(target);
                    if (nearestTarget == null || (distToTarget < distanceToTarget))
                    {                       
                        nearestTarget = target;
                    }
                    distanceToTarget = distToTarget;
                   
                }
            }
        }
    }
    #endregion Methods
    
    #region Unity Methods

    void Start()
    {
        StartCoroutine(DelayFindVisibleTarget(delay));
    }
    #endregion Unity Methods
    
    #region etc
    public Vector3 DirFromAngle(float angleInDegree, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegree += transform.eulerAngles.y; 
            // 글로벌 값을 사용할때는 y값을 그냥 두고, 로컬값을 사용할땐 y값을 더해준다.
        }

        return new Vector3(Mathf.Sin( angleInDegree * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegree * Mathf.Deg2Rad));
    }

    #endregion etc
}
