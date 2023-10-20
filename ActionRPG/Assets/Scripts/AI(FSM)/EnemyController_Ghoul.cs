using System.Collections.Generic;
using UnityEngine;

public class EnemyController_Ghoul : MonoBehaviour, IAttackable, IDamageable
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
    public Transform ProjoctileTransform;
    public Transform hitTransform;
    [SerializeField] private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();
    public LayerMask TargetMask;
    public int maxHealth;
    public int health
    {
        get;
        private set;
    }

    private Animator animator;
    private int hitTriggerHash = Animator.StringToHash("Hit");
    
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
        stateMachine.AddState(new DeadState());

        fov = GetComponent<FieldOfView>();
        InitAttackBehaviours();
        health = maxHealth;
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        CheckAttackBehaviour();
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

    #region IAttackable Interface

    public AttackBehaviour CurrentAttackBehaviour
    {
        get;
        private set;
    }

    public void OnExecuteAttack(int attackIndex)
    {
        if (CurrentAttackBehaviour != null && target != null)
        {
            CurrentAttackBehaviour.ExecuteAttack(target.gameObject, ProjoctileTransform);
        }
    }
    #endregion IAttackable Interface

    #region IDamageable Interface

    public bool isAlive => health > 0;

    public void takeDamage(int damage, GameObject hitEffectPrefab)
    {
        if (!isAlive) return;
        health -= damage;
        if (hitEffectPrefab)
        {
            Instantiate(hitEffectPrefab, hitTransform.position, Quaternion.identity);
        }

        if (isAlive)
        {
            animator?.SetTrigger(hitTriggerHash);
        }
        else
        {
            stateMachine.ChangeState<DeadState>();
        }
    }
    
    #endregion IDamageable Interface
    
    #region Helper Methods

    private void InitAttackBehaviours()
    {
        foreach(AttackBehaviour behaviour in attackBehaviours)
        {
            if (CurrentAttackBehaviour != null)
            {
                CurrentAttackBehaviour = behaviour;
            }

            behaviour.targetMask = TargetMask;
        }
    }

    private void CheckAttackBehaviour()
    {
        if (CurrentAttackBehaviour != null || !CurrentAttackBehaviour.isAvailable)
        {
            CurrentAttackBehaviour = null;
            foreach(AttackBehaviour behaviour in attackBehaviours)
            {
                if (behaviour.isAvailable)
                {
                    if ((CurrentAttackBehaviour == null) || (CurrentAttackBehaviour.priority < behaviour.priority))
                    {
                        CurrentAttackBehaviour = behaviour;
                    }
                }
            }
        }
    }
    #endregion Helper Methods
    
    
}
