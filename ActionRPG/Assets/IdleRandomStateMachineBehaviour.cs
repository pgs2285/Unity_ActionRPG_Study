using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleRandomStateMachineBehaviour : StateMachineBehaviour
{
    #region Variables
    public int numberOfStates = 3; // 기본상태를 제외한 상태의 개수
    public float minStateTime = 0f;
    public float maxStateTime = 5f;
    public float randomNormalTime;
    readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");  // 스트링보다 빠른 해시값을 사용한다.
    #endregion Variables

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        randomNormalTime = Random.Range(minStateTime, maxStateTime);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(animator.IsInTransition(0) && animator.GetCurrentAnimatorStateInfo(0).fullPathHash == stateInfo.fullPathHash)
       // 만약 base layer(0번레이어)에 있거나, 현재 재생 중인 애니메이션 상태의 fullPathHash가 stateInfo로 지정된 애니메이션 상태의 fullPathHash와 동일하다면
       // fullPathHash 는 고유 식별자이다.
       // 즉 베이스레이어에 있으며 현재상태의 이름과 (0번 레이어)의 경로가 같으면 들어와있지 않은것
       {
            animator.SetInteger(hashRandomIdle, -1);
       }
        
       if(stateInfo.normalizedTime * 2> randomNormalTime && !animator.IsInTransition(0))
       {
            // Debug.Log(stateInfo.normalizedTime);    // 보니까 이거 실제 시간이 아니라 애니메이션의 정규화된 시간이다.
            animator.SetInteger(hashRandomIdle, Random.Range(0,numberOfStates));
       }
    }

}
