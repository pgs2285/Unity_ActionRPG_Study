using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : State<EnemyController>
{
    #region Variables
    private Animator animator;
    private int hashAttack = Animator.StringToHash("Attack");
    private int hashAttackIndex = Animator.StringToHash("AttackIndex");
    private AttackStateController _attackStateController;
    private IAttackable attackable;
    
    #endregion Variables

    #region Methods
    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        _attackStateController = context.GetComponent<AttackStateController>();
        attackable = context.GetComponent<IAttackable>();
    }

    public override void Update(float deltaTime)
    {
        
    }

    public override void OnEnter()
    {
        Debug.Log("AttackState");
        if (attackable == null || attackable.CurrentAttackBehaviour == null)
        {// 만약 공격이 불가능한 객체라 attackable을 상속받지 않았거나, 스킬이 쿨타임이라 공격을 못하면.,
            stateMachine.ChangeState<IdleState>();
            return;
        }

        _attackStateController.EnterAttackStateHandler +=OnEnterAttackState;
        _attackStateController.ExitAttackStateHandler += OnExitAttackState;
        animator?.SetInteger(hashAttackIndex, attackable.CurrentAttackBehaviour.animationIndex);
        animator?.SetTrigger(hashAttack);

    }
    
    public void OnEnterAttackState()
    {
        
    }

    public void OnExitAttackState()
    {
        stateMachine.ChangeState<IdleState>();
    }
    #endregion Methods
}
