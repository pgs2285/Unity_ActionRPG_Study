using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackStateController : MonoBehaviour
{
    public delegate void OnEnterAttackState();// 델리게이트 : 함수를 변수처럼 사용할 수 있게 해준다. 이러면 매개변수가 없는 함수를 할당 할 수 있다.
    public delegate void OnExitAttackState();
    
    public OnEnterAttackState EnterAttackStateHandler;  
    public OnExitAttackState ExitAttackStateHandler;
    // Start is called before the first frame update
    public bool isAttackState
    {
        get;
        private set;
    }
    void Start()
    {
        EnterAttackStateHandler = new OnEnterAttackState(EnterAttackState);
        ExitAttackStateHandler = new OnExitAttackState(ExitAttackState);
    }

    void Update()
    {
        Debug.Log(ExitAttackStateHandler);
    }
    #region Helper Methods

    public void OnStartOfAttackState()
    {
        isAttackState = true;
        EnterAttackStateHandler();
    }

    public void OnEndOfAttackState()
    {
        isAttackState = false;
        ExitAttackStateHandler();
    }
    private void EnterAttackState()
    {
        
    }

    private void ExitAttackState()
    {

    }

    public void OnCheckAttackCollider(int attackIndex)
    {   
        GetComponent<IAttackable>().OnExecuteAttack(attackIndex); // 공격 애니메이션 도중에 어떠한 형태로 적을 검출해 입힐것인지 결정하는 함수
    }
    #endregion Helper Methods
}
