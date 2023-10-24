using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;


[RequireComponent(typeof(FieldOfView)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(CharacterController))]
public abstract class EnemyController : MonoBehaviour
{
    #region Variables
    public StateMachine_New<EnemyController> stateMachine;
    public virtual float AttackRange => 6.0f;

    protected NavMeshAgent agent;
    protected Animator animator;

    private FieldOfView fieldOfView;

    #endregion Variables

    #region Properties

    public Transform Target => fieldOfView.NearestTarget;
    public LayerMask TargetMask => fieldOfView.targetMask;

    #endregion Properties

    #region Unity Methods

    // Start is called before the first frame update
    protected virtual void Start()
    {
        stateMachine = new StateMachine_New<EnemyController>(this, new IdleState());

        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;
        agent.updateRotation = true;

        animator = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
    }


    // Update is called once per frame
    protected virtual void Update()
    {
        stateMachine.Update(Time.deltaTime);
        if (!(stateMachine.CurrentState is MoveState) && !(stateMachine.CurrentState is DeadState))
        {
            FaceTarget();
        }
    }

    void FaceTarget()
    {
        if (Target)
        {
            Vector3 direction = (Target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
    }


    #endregion Unity Methods

    #region Helper Methods
    public virtual bool IsAvailableAttack => false;

    public R ChangeState<R>() where R : State<EnemyController>
    {
        return stateMachine.ChangeState<R>();
    }

    #endregion Helper Methods
}
