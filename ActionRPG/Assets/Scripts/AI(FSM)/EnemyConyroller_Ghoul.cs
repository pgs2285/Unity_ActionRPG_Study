using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConyroller_Ghoul : MonoBehaviour
{
    #region Variables
    protected StateMachine_New<EnemyConyroller_Ghoul> stateMachine;
    #endregion Variables

    #region Unity Methods
    void Start()
    {
        stateMachine = new StateMachine_New<EnemyConyroller_Ghoul>(this, new IdleState());
        stateMachine.AddState(new MoveState());
        stateMachine.AddState(new AttackState());
    }

    private void Update()
    {
        stateMachine.Update(Time.deltaTime);
    }
    #endregion Unity Methods
    
}
