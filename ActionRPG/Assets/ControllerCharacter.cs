using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ControllerCharacter : MonoBehaviour
{
    #region Variables

    private CharacterController characterController;
    private bool isGround = false;                  // 땅에 닿아있는지 확인하기 위한 변수
    private Vector3 calcVelocity;
    private NavMeshAgent agent;
    private Camera camera; 
    public LayerMask groundLayerMask;               // raycast를 통해 땅에 닿아있는지 확인하기 위한 변수
    public float groundCheckDistance = 0.3f;
    #endregion Variables

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;           // NavMeshAgent가 자동으로 이동하지 않게함
        agent.updateRotation = true;            // NavMeshAgent가 자동으로 회전하게함

        camera = Camera.main;
    }

    void Update()
    {
        if(Input.GetMouseButtonDown(0)) // 왼쪽 마우스 클릭
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition); // 카메라에서 마우스 위치로 레이를 쏜다.
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit, 100, groundLayerMask))      // physics.raycast 는 물체가 맞았으면 true를 리턴함
            {
                Debug.Log("We hit " + hit.collider.name + " " + hit.point);
                agent.SetDestination(hit.point);    // NavMeshAgent가 이동할 목적지를 설정한다.
            }
        }

        if(agent.remainingDistance > agent.stoppingDistance) // agent.remainingDistance 는 목적지까지 남은 거리를 리턴한다.
        {
            characterController.Move(agent.desiredVelocity * Time.deltaTime); // agent.desiredVelocity 는 목적지까지의 속도를 리턴한다.
        }
        else
        {
            characterController.Move(Vector3.zero);
        }
    }

    private void LateUpdate()
    {
        transform.position = agent.nextPosition; // NavMeshAgent가 이동한 위치를 캐릭터의 위치로 설정한다.
    }

    #region Helper Methods

    private void isGroundedCheck()
    {
        RaycastHit hit;

#if UNITY_EDITOR    // 유니티 에디터에서만 실행
    Debug.DrawLine(transform.position + (Vector3.up * 0.1f), // 시작점
        transform.position + (Vector3.up * 0.1f) + (Vector3.down * groundCheckDistance), // 시작점에서 땅방향으로 + groundCheckDistance(끝점)
        Color.red); // 색상
    
#endif
        
        if(Physics.Raycast(transform.position + (Vector3.up * 0.1f),    // 발에서 살짝 떨어뜨려서 raycast를 쏜다. 추후 발이 뭍히는 지형에서도 원활히 감지하기 위함
            Vector3.down,                                               // 아래 방향으로
            out hit,                                                    // hit에 정보를 담는다.
            groundCheckDistance,                                        // 땅에 어느정도 가까워 졌을때 감지할지
            groundLayerMask                                             // 땅에 대한 레이어마스크
        )) isGround = true;
        else isGround = false;

    
    }

    #endregion Helper Methods
}


