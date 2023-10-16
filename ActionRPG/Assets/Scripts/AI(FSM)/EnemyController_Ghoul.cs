using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Ghoul : MonoBehaviour
{
    #region Variables
    protected StateMachine_New<EnemyController_Ghoul> stateMachine;

    public LayerMask targetMask;
    public Transform target;
    public float viewRadius;
    public float AttackRange;

    #endregion Variables

    #region Unity Methods
    void Start()
    {
        stateMachine = new StateMachine_New<EnemyController_Ghoul>(this, new IdleState());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
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
        target = null;

        Collider[] targetInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask); // 캐릭터를 중심으로 viewRadius만큼의 반경안에 있는 targetMsk를rkwls 콜라이더를 모두 가져온다.
        // layerMask 는 검사할때 부하를 줄여주기때문에 사용하는게 좋다.
        if(targetInViewRadius.Length > 0)
        {
            target = targetInViewRadius[0].transform.transform;
        }
        return target;
    }

    #endregion Other Methods

    
    
}
