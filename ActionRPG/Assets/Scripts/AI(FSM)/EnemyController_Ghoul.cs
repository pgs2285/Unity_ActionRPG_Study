using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class EnemyController_Ghoul : MonoBehaviour
{
    #region Variables
    protected StateMachine_New<EnemyController_Ghoul> stateMachine;
    public StateMachine_New<EnemyController_Ghoul> StateMachine => stateMachine;
    public float AttackRange;
    private FieldOfView fov; 
    public Transform target => fov?.NearestTarget;

    #endregion Variables

    #region Unity Methods
    void Start()
    {
        stateMachine = new StateMachine_New<EnemyController_Ghoul>(this, new IdleState());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());

        fov = GetComponent<FieldOfView>();

    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }
    #endregion Unity Methods

    #region Other Methods

    public bool IsAvailableAttack{  // 이런식으로 전개하는걸 property라고 한다. 아레 코드는 get만 있는 property이다.
            get{    // 이는 get만 있는 읽기 전용이므로 수정할 수 없다.
                if(!target) 
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

    // private void OnDrawGizmos()     // 캐릭터의 범위를 표시해준다.
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.position, viewRadius);
    //     Handles.Label(transform.position + viewRadius * Vector3.up, "캐릭터 인식 범위");
        
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawWireSphere(transform.position, AttackRange);
    //     Handles.Label(transform.position + AttackRange * Vector3.up, "캐릭터 공격 범위");
    // }   
    #endregion Other Methods

    
    
}
