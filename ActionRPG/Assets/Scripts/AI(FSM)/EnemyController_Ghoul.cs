using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyController_Ghoul : MonoBehaviour
{
    #region Variables

    public bool isPatrol;
    protected StateMachine_New<EnemyController_Ghoul> stateMachine;
    public StateMachine_New<EnemyController_Ghoul> StateMachine => stateMachine;
    public float AttackRange;
    private FieldOfView fov;
    public Transform target => fov?.NearestTarget;
    public Transform[] waypoints;
    [HideInInspector] public Transform wayPointTarget;
    private int wayPointIndex;

    #endregion Variables

    #region Unity Methods

    void Start()
    {
        stateMachine = new StateMachine_New<EnemyController_Ghoul>(this, new MoveToWayPoints());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
        IdleState idleState = new IdleState();
        idleState.isPatrol = true;
        stateMachine.AddState(new IdleState());

        fov = GetComponent<FieldOfView>();

    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }

    #endregion Unity Methods

    #region Other Methods

    public bool IsAvailableAttack
    {
        // 이런식으로 전개하는걸 property라고 한다. 아레 코드는 get만 있는 property이다.
        get
        {
            // 이는 get만 있는 읽기 전용이므로 수정할 수 없다.
            if (!target)
            {
                return false;
            }

            float distance = Vector3.Distance(transform.position, target.position);
            return (distance <= AttackRange);
        }
    }

    public Transform SearchEnemy()
    {
        return target;
    }

    public Transform FindNextWayPoint()
    {
        wayPointTarget = null;
        if(waypoints.Length > 0)
        {
            wayPointTarget = waypoints[wayPointIndex];
        }

        wayPointIndex = (wayPointIndex + 1) % waypoints.Length;  //cycling
        return wayPointTarget;
    }

#endregion Other Methods

    
    
}
