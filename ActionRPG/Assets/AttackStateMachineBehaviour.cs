using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateMachineBehaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<AttackStateController>()?.OnStartOfAttackState();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.GetComponent<AttackStateController>()?.OnEndOfAttackState();
        animator.GetComponent<EnemyController>()?.stateMachine.ChangeState<IdleState>();
        
    }
}
