using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackable    // 컴포넌트로 구현될것이 아니라 Interface로 선언한다.      
{
    AttackBehaviour CurrentAttackBehaviour
    {
        get;
    }   // 현재 공격상태(쿨타임, 공격가능여부 등)를 반환해준다.

    public void OnExecuteAttack(int attackIndex);



}
