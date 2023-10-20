using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State<EnemyController>

{
    #region Variables
    private Animator animator;
    private CharacterController characterController;
    private UnityEngine.AI.NavMeshAgent agent;

    private int moveHash = Animator.StringToHash("Move");
    private int moveSpeedHash = Animator.StringToHash("MoveSpeed");

    #endregion Variables

    #region Methods
    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        characterController = context.GetComponent<CharacterController>();
        agent = context.GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    public override void OnEnter()
    {
        Debug.Log("MoveState");
        agent?.SetDestination(context.Target.position);
        animator?.SetBool(moveHash, true);
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.Target;
        if(enemy)
        {
            agent.SetDestination(enemy.position);

            if(agent.remainingDistance > agent.stoppingDistance)
            {
                characterController.Move(agent.velocity * deltaTime);
                animator.SetFloat(moveSpeedHash, agent.velocity.magnitude / agent.speed , .1f, deltaTime); // magnitude 는 벡터의 크기를 리턴한다.
                // SetFloat 가 매개변수를 4개 받으면 SetFloat(string name, float value, float dampTime, float deltaTime) 이다.
                // dampTime 은 애니메이션의 부드러운 정도를 나타내는데, 이는 0~1 사이의 값이다.
                return;
            }


            stateMachine.ChangeState<IdleState>();
            
        }
    }

    public override void OnExit()
    {
        animator?.SetBool(moveHash, false);
        agent.ResetPath();  // agent의 목적지를 초기화한다.
    }
    #endregion Methods


}
