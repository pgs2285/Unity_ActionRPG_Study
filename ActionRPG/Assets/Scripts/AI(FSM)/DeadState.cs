using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeadState : State<EnemyController>
{
    private Animator _animator;
    private int _isAliveHash = Animator.StringToHash("IsAlive");
    
    public override void OnInitialized()
    {
        _animator = context.GetComponent<Animator>();
    }
    public override void OnEnter()
    {
        _animator.SetBool(_isAliveHash, false);
    }
    
    public override void Update(float deltaTime)
    {
        if(stateMachine.ElapsedTimeInState > 3.0f)
        {
            GameObject.Destroy(context.gameObject);
        }
    }
}
