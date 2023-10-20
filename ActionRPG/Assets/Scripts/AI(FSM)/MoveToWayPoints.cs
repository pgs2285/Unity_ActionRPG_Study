using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToWayPoints : State<EnemyController_Ghoul>
{
    private Animator animator;
    private CharacterController controller;
    private NavMeshAgent agent;

    protected int hasMove = Animator.StringToHash("Move");
    protected int hasMoveSpeed = Animator.StringToHash("MoveSpeed");
    public override void OnInitialized()
    {
        animator = context.GetComponent<Animator>();
        controller = context.GetComponent<CharacterController>();
        agent = context.GetComponent<NavMeshAgent>();
    }

    public override void OnEnter()
    {
        if(context.wayPointTarget == null) context.FindNextWayPoint();
        if (context.wayPointTarget)
        {
            agent?.SetDestination(context.wayPointTarget.position);
            animator?.SetBool("hasMove", true);
        }
    }

    public override void Update(float deltaTime)
    {
        Transform enemy = context.SearchEnemy();    // 적을 찾는다.
        if (enemy != null)
            // 적을 찾아보고 공격이 가능하다면 -> Attack,
            // 적을 찾아보고 공격이 불가능하다면 -> Move,
        {
            if (context.IsAvailableAttack)
            {
                stateMachine.ChangeState<AttackState>();
            }
            else
            {
                stateMachine.ChangeState<MoveState>();
            }
        }
        else
        {
            if (!agent.pathPending && (agent.remainingDistance <= agent.stoppingDistance))
            {   //pathPending = NavMeshAgent가 이동해야할 경로가 존재하는지 아닌지 반환해준다.
                Transform nextDest = context.FindNextWayPoint();
                agent?.SetDestination(nextDest.position);
                stateMachine.ChangeState<IdleState>();
            }
            else
            {
                controller.Move(agent.velocity * deltaTime);
                animator.SetBool(hasMove, true);
                animator.SetFloat(hasMoveSpeed, agent.velocity.magnitude / agent.speed, .1f, deltaTime);
            }
        }

    }

    public override void OnExit()
    {
        animator?.SetBool(hasMove, false);
        agent.ResetPath();  // agent의 목적지를 초기화한다.
    }
}
