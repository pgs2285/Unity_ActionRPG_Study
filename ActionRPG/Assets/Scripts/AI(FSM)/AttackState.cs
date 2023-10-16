using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController_Ghoul>
{
    #region Variables
    private Animator animator;
    private int hashAttack = Animator.StringToHash("Attack");
    private int hashAttackIndex = Animator.StringToHash("AttackIndex");
    #endregion Variables

    #region Methods
    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
    }

    public override void Update(float deltaTime)
    {
        
    }

    public override void OnEnter()
    {
        if(context.IsAvailableAttack)
        {
            animator?.SetTrigger(hashAttack);
        }else{
            stateMachine.ChangeState<IdleState>();
        }
    }
    #endregion Methods
}
