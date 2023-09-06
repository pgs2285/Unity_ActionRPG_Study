using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharacter : MonoBehaviour
{
    #region Variables
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float dashDistance = 5f;
    private CharacterController characterController;
    private bool isGround = false;                  // 땅에 닿아있는지 확인하기 위한 변수
    public float gravity = -9.81f;
    public Vector3 drags;
    private Vector3 calcVelocity;
    #endregion Variables

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGround = characterController.isGrounded;      // raycast가 아닌 characterController의 isGrounded를 사용한다.
        if(isGround && calcVelocity.y < 0) // 땅에있을때 더이상 중력값의 영향을 받지 않게함
        {
            calcVelocity.y = 0;
        }
        // Process Inputs

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
        characterController.Move(move * speed * Time.deltaTime); // 캐릭터 컨트롤러를 이용한 이동
 
        if(move != Vector3.zero)
        {
            transform.forward = move; // transform.forward 는 앞쪽을 바라보는 방향으로 로컬 좌표계를 회전시킴.
        }

        // Process Jump
        if(Input.GetButtonDown("Jump") && isGround)
        {
            calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity); // 점프공식이다. 게임마다 구현방식이 다르다.
        }
        // Process Dash
        if(Input.GetButtonDown("Dash"))
        {
            Vector3 dashVelocity = Vector3.Scale(transform.forward,
                dashDistance * new Vector3(Mathf.Log(1f/(Time.deltaTime * drags.x + 1 ))/ -Time.deltaTime, 
                0,
                (Mathf.Log(1f/(Time.deltaTime * drags.z + 1 ))/ -Time.deltaTime)
                )
            );
            calcVelocity += dashVelocity;
        }

        calcVelocity.y += gravity * Time.deltaTime; // 중력값을 계산한다.

        calcVelocity.x /= 1 + drags.x * Time.deltaTime; // x축으로 이동할때마다 drags.x의 값만큼 속도를 줄인다.
        calcVelocity.z /= 1 + drags.z * Time.deltaTime; // z축으로 이동할때마다 drags.z의 값만큼 속도를 줄인다
        calcVelocity.y /= 1 + drags.y * Time.deltaTime; // y축으로 이동할때마다 drags.y의 값만큼 속도를 줄인다.

        characterController.Move(calcVelocity * Time.deltaTime); // 캐릭터 컨트롤러를 이용한 이동
    }


}
