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
        // public bool isAvailableAttack = true;

        #endregion Variables

        #region Proeprties
            
        public override bool IsAvailableAttack
        {
            get
            {
                if (!Target)
                {
                    return false;
                }

                float distance = Vector3.Distance(transform.position, Target.position);
                return (distance <= AttackRange);
            }
        }

    

        #endregion Properties

        #region Unity Methods

        protected override void Start()
        {
            base.Start();

            stateMachine.AddState(new MoveState());
            stateMachine.AddState(new AttackState());
            stateMachine.AddState(new DeadState());

            health = maxHealth;
            InitAttackBehaviour();

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
        if(CurrentAttackBehaviour != null)
            {
                CurrentAttackBehaviour.ExecuteAttack(Target.gameObject, hitPoint);
            }
            
        }

        private void InitAttackBehaviour()
        {
            foreach (AttackBehaviour behaviour in attackBehaviours)
            {
                if (CurrentAttackBehaviour == null)
                {
                    CurrentAttackBehaviour = behaviour;
                }

                behaviour.targetMask = TargetMask;
            }
        }

        private void CheckAttackBehaviour()
        {
            if (CurrentAttackBehaviour == null || !CurrentAttackBehaviour.isAvailable)
            {
                CurrentAttackBehaviour = null;

                foreach (AttackBehaviour behaviour in attackBehaviours)
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

        #endregion IAttackable Interfaces
    }
