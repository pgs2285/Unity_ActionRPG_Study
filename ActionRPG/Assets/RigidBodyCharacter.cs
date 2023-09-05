using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyCharacter : MonoBehaviour
{
    #region Variables
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float dashDistance = 5f;
    private Rigidbody rb;
    private Vector3 inputDirection = Vector3.zero;  // 사용자의 입력에 대한 방향성을 계산하기위한 변수
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // Process Inputs
        inputDirection = Vector3.zero;      // 초기화
        inputDirection.x = Input.GetAxis("Horizontal"); // 좌우에 대한 입력값
        inputDirection.z = Input.GetAxis("Vertical");   // 앞뒤에 대한 입력값
    }
}
