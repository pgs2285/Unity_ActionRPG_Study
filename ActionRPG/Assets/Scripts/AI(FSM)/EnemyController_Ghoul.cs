using System.Collections.Generic;
using UnityEngine;


    public class EnemyController_Ghoul : EnemyController, IAttackable, IDamageable
    {
        #region Variables
        [SerializeField]
        public Transform hitPoint;

        public Transform[] waypoints;

        public float maxHealth => 100f;
        private float health;

        private int hitTriggerHash = Animator.StringToHash("HitTrigger");
        private int isAliveHash = Animator.StringToHash("IsAlive");

        #endregion Variables

        #region Proeprties
        

        #endregion Properties

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());

            health = maxHealth;

        }

        private void OnAnimatorMove()
        {
            // Follow NavMeshAgent
            //Vector3 position = agent.nextPosition;
            //animator.rootPosition = agent.nextPosition;
            //transform.position = position;

            // Follow CharacterController
            Vector3 position = transform.position;
            position.y = agent.nextPosition.y;

            animator.rootPosition = position;
            agent.nextPosition = position;

            // Follow RootAnimation
            //Vector3 position = animator.rootPosition;
            //position.y = agent.nextPosition.y;

            //agent.nextPosition = position;
            //transform.position = position;
        }

        #endregion Unity Methods

        #region Helper Methods
        #endregion Helper Methods

        #region IDamagable interfaces

        public bool isAlive => (health > 0);

        public void takeDamage(int damage, GameObject hitEffectPrefab)
        {
            if (!isAlive)
            {
                return;
            }

            health -= damage;



            if (hitEffectPrefab)
            {
                Instantiate(hitEffectPrefab, hitPoint);
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

        #endregion IDamagable interfaces

        #region IAttackable Interfaces

        public GameObject hitEffectPrefab = null;

        [SerializeField]
        private List<AttackBehaviour> attackBehaviours = new List<AttackBehaviour>();

        public AttackBehaviour CurrentAttackBehaviour
        {
            get;
            private set;
        }

        public void OnExecuteAttack(int attackIndex)
        {

        }

        #endregion IAttackable Interfaces
    }
