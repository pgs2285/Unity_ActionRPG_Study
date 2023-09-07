# 목차
[프로젝트 설명](#프로젝트-설명)

[1-1.rigidbody 이동점프 및 대쉬](#1-1-rigidbody-이동점프-및-대쉬)  
[1-2.charactercontroller를 이용한 이동](#1-2-charactercontroller를-이용한-이동)  
[1-3.character controller 에 navmesh 결합하기(최종 이동)](#1-3-character-controller-에-navmesh-결합하기)  
  
[2. 캐릭터 모델링 및 애니메이션 구현](#캐릭터-모델링-및-애니메이션-구현)  
  
[공부내용](#공부내용)
[1. 정적오브젝트](#1-정적-오브젝트)  
[2. vector-transform](#2-vector--transform)  
[3. update, fixedUpdate, lateUpdate](#3-update-fixedupdate-lateupdate)  
[4. requirecomponent](#4-requirecomponent)  


# Unity_ActionRPG
유니티로 ActionRPG(Diable등) 게임 제작하기

지금까지 유니티를 개인적으로 공부해왔지만, 하나같이 프로젝트를 직접 만들어보면서 해본것이라 간단한 컴포넌트 조차 제대로 짚고 넘어갈 기회가 없었다.
예를들어 rigidbody컴포넌트는 그동안 셀수없이 써봤지만 그 안에 Is Kinematic같은것은 정확히 어떤역할을 하는지 몰랐다.
이 참에 그동안 간과했던 하나하나 짚고 여기에 적어보려 한다. 

__프로젝트 설명__  에는 프로젝트를 만들면서 사용한 기술, 코드의 큰 맥락등을 분석한것을 적을것이며,  
__공부내용__  에는 내가 그동안 무심코 지나치며 적용했던 것들을 적어보며 복습한다.

## 프로젝트 설명

움직임은 2가지 모두 구현해 놓았다  

    1. rigidbody를 통한 움직임 [rigidbodyCharacter.cs](./ActionRPG/Assets/RigidBodyCharacter.cs)  
    
    2. characterController를 통한 움직임
        [characterController.cs](./ActionRPG/Assets/ControllerCharacter.cs)  

    3. NavMeshAgent를 이용한 캐릭터 제작 (+ CharacterController 와 조합해 확장)
        
보통의 게임에서는 간단한 플레이어 움직임은 물리엔진을 사용하지 않고 2번방식을 자주 사용한다.  
1,2 번은 키보드에 적합한 방식이고 Click & Move 방식에 적당한 방법은 3번 NavMeshAgent이다.  
우리는 디아블로같은 click & move 이동방식을 구현할것이라 3번으로 한다. 결과만 보려면 **1-3 Character Controller 에 NavMesh 결합하기**
만 확인한다.



---
### __1-1 rigidbody 이동,점프 및 대쉬__  
    
rigidbody 컴포넌트 구성은 다음 링크를 참조한다  
[rigidbody 컴포넌트 구성](https://docs.unity3d.com/kr/2021.3/Manual/class-Rigidbody.html)   
  
설명에 앞서 사용하는 변수명 및 초기값은 아래와 같다.
```csharp
    #region Variables
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float dashDistance = 5f;
    private Rigidbody rb;
    private Vector3 inputDirection = Vector3.zero;  // 사용자의 입력에 대한 방향성을 계산하기위한 변수
    private bool isGround = false;                  // 땅에 닿아있는지 확인하기 위한 변수
    public LayerMask groundLayerMask;               // raycast를 통해 땅에 닿아있는지 확인하기 위한 변수
    public float groundCheckDistance = 0.3f;
    #endregion Variables
```
__이동__ :
``` csharp
    <--------- update 문 중 일부 --------->
    inputDirection = Vector3.zero;      // 초기화
    inputDirection.x = Input.GetAxis("Horizontal"); // 좌우에 대한 입력값
    inputDirection.z = Input.GetAxis("Vertical");   // 앞뒤에 대한 입력값
    if(inputDirection != Vector3.zero)
    {
        transform.forward = inputDirection; // transform.forward 는 앞쪽을 바라보는 방향으로 로컬 좌표계를 회전시킴.
    }
    <--------- update 문 종료 --------->
    
    <---------FixedUpdate문 중 일부 --------->
    rb.MovePosition(rb.position + inputDirection * speed * Time.fixedDeltaTime);
    <---------FixedUpdate 종료 ---------->
    
```
이동에 대한 설명은 밑쪽 공부내용 - 3,4번 항목을 참조한다  
__점프__:
rigidbody에서 jump는 Vector3.up 방향으로 점프값에 physics.gravity.y에 -2f를 곱해줬다 (physics.gravity.y 는 음수값) 게임마다 구현하는 방식이 다르므로 참조만 한다.
 
``` csharp
    <--------- update 문 중 일부 --------->
    isGroundedCheck();
    if(Input.GetButtonDown("Jump") && isGround)
    {
        Vector3 JumpVelocity = Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y); // 점프공식이다. 게임마다 구현방식이 다르다.
        rb.AddForce(JumpVelocity, ForceMode.VelocityChange); // 점프를 위한 힘을 가한다. VelocityChange는 힘을 가하는 방식이다.
    }
    <-------- update 문 종료 -------->
```
isGroundedCheck()는 아래와 같다.
isGroundedCheck()에서는 특정 LayerMask를 검출해 그것에 raycast가 닿으면 true반환, 아니면 false반환 하는 방식을 사용했다.


``` csharp

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
```
__대시__:
대시에 대한 함수는 다음과 같다.
``` csharp
    <--------- update 문 중 일부 --------->
    if(Input.GetButtonDown("Dash"))
    {
        Vector3 dashVelocity = Vector3.Scale(transform.forward,
            dashDistance * new Vector3(Mathf.Log(1f/(Time.deltaTime * rb.drag + 1 ))/ -Time.deltaTime, 
            0,
            (Mathf.Log(1f/(Time.deltaTime * rb.drag + 1 ))/ -Time.deltaTime)
            )
        );
        rb.AddForce(dashVelocity, ForceMode.VelocityChange);
    }
    <--------- update 문 종료 --------->
```
여기선 dashVelocity를 구하는데 Vector3.Scale를 통해서 방향벡터를 스케일링 했다.
Vector3.Scale은 두개의 벡터를 받아서 각 구성요소를 곱한후 반환하는 것으로, 여기선 첫번째 인자로, transform.forward(사용자가 바라보는 방향)를 받아왔고, 이를 두번째 인자에 곱해 스케일링한다.
두번째는 스케일링할 벡터를 생성해 주었다. 스케일링할 벡터는 log를 분모로 하고 저항값을 가져와 1을 더한다거기에 deltaTime을 곱해줬다. 대시는 처음에 빨랐다가, 느려지는게 좋을거 같아서
저런식으로 구현했다. 다만 대시또한 게임마다 구현하는 방법이 다르므로 다른자료를 참조만 했다.  

AddForce 의 두번째 인자는 ForceMode이다. 여기서는 ForceMode.VelocityChange를 사용했는데 이는 순간적으로 뒤에서 밀듯이 힘을 붙여주는것이다.   
이밖에도 엑셀을 밟듯 가속을하는 ForceMode.Force   
똑같이 가속을하지만 무게를 무시하는 ForceMode.Accelertion  
순간적으로 힘을주는 ForceMode.Impulse  
순간적으로 힘을주지만 무게를 무시하는 ForceMode.VelocityChange  

---

### 1-2. CharacterController를 이용한 이동
CharacterController 에 대한 컴포넌트 구성은 다음 링크를 참조한다  

[CharacterController 컴포넌트 구성](https://docs.unity3d.com/kr/2021.3/Manual/class-CharacterController.html)  

__ CharacterController 이동,점프 및 대쉬__  

설명에 앞서 사용하는 변수명 및 초기값은 아래와 같다.
```csharp
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
```

이전 rigidbody와 다른점은 gravity 와 drags 를 직접 계산해 줘야한다는 것이다.
이러한 것들때문에 이동 점프 대시가 조금 구현이 rigidbody와 달라진다

__이동__
``` csharp

<---------- update 문 일부----------->
      isGround = characterController.isGrounded;      // raycast가 아닌 characterController의 isGrounded를 사용한다.  
        if(isGround && calcVelocity.y < 0) // 땅에있을때 더이상 중력값의 영향을 받지 않게함
        {
            calcVelocity.y = 0;
        }
        // Process Inputs

        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")); 
        characterController.Move(move * speed * Time.deltaTime); // 캐릭터 컨트롤러를 이용한 이동
	(생략)
        calcVelocity.y += gravity * Time.deltaTime; // 중력값을 계산한다.

        calcVelocity.x /= 1 + drags.x * Time.deltaTime; // x축으로 이동할때마다 drags.x의 값만큼 속도를 줄인다.
        calcVelocity.z /= 1 + drags.z * Time.deltaTime; // z축으로 이동할때마다 drags.z의 값만큼 속도를 줄인다
        calcVelocity.y /= 1 + drags.y * Time.deltaTime; // y축으로 이동할때마다 drags.y의 값만큼 속도를 줄인다.

        characterController.Move(calcVelocity * Time.deltaTime); // 캐릭터 컨트롤러를 이용한 이동
 
```

characterController.isGrounded 라는게 있긴하지만, 아무래도 정밀도가 상당히 '많이' 떨어진다. 여기서는 학습용으로 사용했지만 다시
위 rigidbody에서 사용했던것처럼 raycast로 바꿀예정이다.  

__ 점프 __  
``` csharp
        if(Input.GetButtonDown("Jump") && isGround)
        {
            calcVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravity); // 점프공식이다. 게임마다 구현방식이 다르다.
        }
```

간단하다. 이동부분 마지막줄에 characterController.Move를 한번 더 해주는데 여기서 대시 및 점프를 해준다.  

__ 대시 __

``` csharp
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

```  
이것또한 세부 식은 rigidbody의 대시와 같기때문에 설명은 생략한다. 다른것이 있다면 , drags의 값을 public 변수로 받아서 사용한다는점,
calcVelocity에 더해준후 마지막에 이 값을 Move를 통해 처리해준다는 점이다. 

---

###  1-3. Character Controller 에 NavMesh 결합하기 

결합하기에 앞서 Window -> AI -> Navigation 을 눌러 아래 사진과 같은 세팅을 CharacterController 와 동일하게 변경해준다  
![nav](./githubImage/navMesh.png)
그 후 Object를 눌러 빌드를 할 오브젝트를 선택해준다  
![build](./githubImage/objectSettings.png)  

Generate OffMeshLinks : 점프, 순간이동들을 사용할수 있나 체크해줌. 도랑, 울타리등 특정조건이 있어야 지나갈 수 있을떄 체크해준다(일단 체크해제)  
Navigation Area : 영역을 설정하는 곳. 이동불가능한 부분(벽)은 Not Walkable로 해준다  

이 후 bake를 선택해주고 , bake를 눌러준다  
더 세부내용은 추후 진행  
여기까지가 기본세팅이다. 그 후 캐릭터에 NavMeshAgent 컴포넌트를 추가해준다.  
NavMeshAgent의 컴포넌트 구성은 다음 링크를 참조한다.  
[NavMeshAgent 컴포넌트 세부구성](https://docs.unity3d.com/kr/2021.3/Manual/class-NavMeshAgent.html)  

이제 우리는 키보드 입력이 아닌 Click & Move방식으로 구현할 것이다. 일단 사용할 변수는 다음과 같다.  

```csharp
    #region Variables

    private CharacterController characterController;
    private bool isGround = false;                  // 땅에 닿아있는지 확인하기 위한 변수
    private Vector3 calcVelocity;
    private NavMeshAgent agent;
    private Camera camera; 
    public LayerMask groundLayerMask;               // raycast를 통해 땅에 닿아있는지 확인하기 위한 변수
    public float groundCheckDistance = 0.3f;
    #endregion Variables

```
이제 점프등을 구현할 필요 가 없어졌으므로 gravity, drag등을 제거했다.
NavMeshAgent를 사용한다. 먼저 Start에서 다음과 같이 세팅해준다

``` csharp
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<NavMeshAgent>();
        agent.updatePosition = false;           // NavMeshAgent가 자동으로 이동하지 않게함
        agent.updateRotation = true;            // NavMeshAgent가 자동으로 회전하게함

        camera = Camera.main;
    }
```

agent.SetDestination을 하면 자동으로 이동하지만... 우리는 이동처리는 CharacterController 로 할것이기 때문에 updatePosition을 false로 해준다.
Update문은 다음과 같이 변경해준다.  
```csharp
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

```
구조는 되게 단순해졌다. 카메라에서 ray를 쏴서 ground 레이어에 닿으면 검출후 그곳을 목적지로 선택한다(위에서 updatePosition =true 하면 자동이동).
우리는 updatePosition 을 꺼놓았으므로, characterController.Move를 사용한다.

왜 자동이동을 꺼놨냐 하면 추후 장애물과 NPC를 피하면서 다른 충돌체와 작용할때 이것이 더 유리하다.

마지막으로 정확한 좌표값으로 이동해주기위해
``` csharp
    private void LateUpdate()
    {
        transform.position = agent.nextPosition; // NavMeshAgent가 이동한 위치를 캐릭터의 위치로 설정한다.
    }
```
LateUpdate에서 클릭한 위치로 적용해준다.
결과는 다음과 같다  

![navMeshResult](./githubImage/navMeshResult.gif)


---

### 캐릭터 모델링 및 애니메이션 구현  

먼저 [Mixamo](https://mixamo.com) 에서 무료 3D모델링을 다운받아준다.  
일단 Ybot Model과 Idle 애니메이션 3개, 이동 애니메이션 하나를 받았다.  
다운을 받고 난후, 프로젝트에 import 해준후, 기본 ybot을 클릭하면 Rig라는게 있는데,
![ybot](./githubImage/ybot.png) 여기서 사람의 형태면 humanoid, 아니면 generic으로 설정해준다. Rig Tab에 대한 구성 설명은 아래 링크를 참조한다.  
[Rig Tab에 대해](https://docs.unity3d.com/kr/2021.3/Manual/FBXImporter-Rig.html)  
여기서 모바일게임과 같이 과부하가 적게 걸려야한다면 skin weight를 줄여주면 좋다. 이는 애니메이션을 할때 각 vertex가 주변의 몇개의 뼈에 영향을 받을것인가 설정해주는것이므로, 많을수록 과부하가 높다.  
그리고 Import Setting -> animation -> apply 를 눌러주면 적용이 된다.  

그 이후 나머지 Idle과 Walk는 아래같이
![animations Setting](./githubImage/animationSetting.png)  
** Avartar Definition 을 copy from other avatar을 설정해주고, source는 ybot 기본모델에 있는 ybotAvatar을 사용해준다. ** <- 이게 중요하다. 애니메이션을 붙여줄 아바타(모델)을 해준다.   
아래같이 붙인 아바타에따라 애니메이션이 적용되는 대싱이 달라진다.
![remmyTest](./githubImage/testRemmy.gif)<- Test      
위 gif와 같이 Humanoid 에 아바타 내부 구조만 같으면 다른 모델에 Ybot애니메이션을 적용해도 잘 작동한다.

유니티 아바타에 대해선 아래를 참조하되 [유니티 아바타(영문)](https://docs.unity3d.com/Manual/ConfiguringtheAvatar.html) 아무래도 제대로 이해하려면 직접 해봐야 할거같으므로
이번 프로젝트 말미쯤에 따로 다뤄보자

이제 다운받은 모델 & 애니메이션을 적용해보자.
구조는 다음 두장의 이미지와 같다.  
![baseLayer](./githubImage/baseLayer.png) ![sub-state_Layer](./githubImage/Sub-StateMachine.png)

Base Layer애 Sub-State Layer 와 MoveAnimation을 추가해줬다.  
Sub-State Layer에서는 3가지의 Idle을 랜덤 출력해준다. 3가지중 Entry로 들어오는 Animation에서 Add Behaviour를 눌러서 스크립트를 추가해준다.  
사용하는 변수는 아래와 같다

```csharp
    #region Variables
    public int numberOfStates = 3; // 기본상태를 제외한 상태의 개수
    public float minStateTime = 0f;
    public float maxStateTime = 5f;
    public float randomNormalTime;
    readonly int hashRandomIdle = Animator.StringToHash("RandomIdle");  // 스트링보다 빠른 해시값을 사용한다.
    #endregion Variables
```
이 변수들은 먼저 OnStateEnter에서 애니메이션 탈출시간을 Random으로 설정해준다.

```csharp
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        randomNormalTime = Random.Range(minStateTime, maxStateTime);
    }

```  

이 랜덤으로 설정해준 randomNormalTime은 아래에서 OnStateUpdate에서 사용해준다
``` csharp
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
```

특이사항은 animator.SetInteger해줄때 첫번째 인자에 string을 넣어주지 않고, 위에서 선언한 readonly int hashRandomIdle 을 넣어줬는데 이는, string으로 처리하는것보다 int해시 값으로  
변환해서 하는것이 처리속도 측면에서 훨신 빠르기 때문이다.   

움직이는 함수는 기존 CharacterController에 SetBool만 추가해준다.
```csharp
    <--------- 변수추가 --------->
    private Animator animator;
    readonly int moveHash = Animator.StringToHash("Move");
    <-------------------------->
    
    <---------CharacterController Update ---------->
    if(agent.remainingDistance > agent.stoppingDistance) // agent.remainingDistance 는 목적지까지 남은 거리를 리턴한다.
    {
        characterController.Move(agent.desiredVelocity * Time.deltaTime); // agent.desiredVelocity 는 목적지까지의 속도를 리턴한다.
        animator.SetBool(moveHash, true);
    }
    else
    {
        characterController.Move(Vector3.zero);
        animator.SetBool(moveHash, false);
    }
    <---------------------------------------------->
```

해당 결과는 아래와 같다

![resultAnime](./githubImage/moveAndIdleAnimation.gif)  

## 공부내용.

#### 1. 정적 오브젝트

![staticBox](./githubImage/static.png)
ground 와 같이 움직이지 않는것들은 static을 표기해주는 것이 좋다. 이는 정적 오브젝트가 포지션 변화로 인해 무효화 될일이 없다는것을 체크해주는것으로,
여러 정적 오브젝트들을 배치(batch) 라는 하나의 큰 오브젝트로 결합시킴으로서 렌더링을 최적화 할수 있다.(정적 최적화)
필요에 따라 static옆에 화살표를 눌러 원하는 정적 상태를 (활성화/비활성화) 시킬수 있다. 이는 아래 링크 참조.

[정적 게임 오브젝트, 정적 설정에 대한 설명](https://docs.unity3d.com/kr/530/Manual/StaticObjects.html)

---

#### 2. Vector & transform
보통 우리가 사용하는 Vector3.forward 와 transform.forward를 예시로 든다. 굳이 forward에 국한되지 않고 .up, .back등에도 쓴다.
Vector3.forward는 new Vector(0,0,1) 이 기본이다. 이것은 Read-Only Value기 때문에 바꿀수 없다.
transform.forward는 현재 오브젝트를 기준으로 한다. 보통 3D에서 물체가 바라보는 방향을 바꿔주고싶으면 
```csharp
        inputDirection = Vector3.zero;      // 초기화
        inputDirection.x = Input.GetAxis("Horizontal"); // 좌우에 대한 입력값
        inputDirection.z = Input.GetAxis("Vertical");   // 앞뒤에 대한 입력값
        if(inputDirection != Vector3.zero)
        {
            transform.forward = inputDirection; // transform.forward 는 앞쪽을 바라보는 방향으로 로컬 좌표계를 회전시킴.
        }
```
와 같이 사용하곤한다.
즉 사용 용도가 완전히 다르다.

---
#### 3. Update, FixedUpdate, LateUpdate
~ Update - 매 프레임마다 처리되는 작업이다. 때문에 그래픽 랜더링 속도에 따라 느려지거나 빨라지고 있어서, 원하지않은 물리적 충돌이 발생할 수 있다.
~ FixedUpdate - 물리엔진 위에서 동작한다. 즉 고정된 시간마다 실행하는데 이때문에 보통 이동,회전,힘에서 사용한다.
~ LateUpdate - Update문 호출이되고 가장 마지막에 호출되는 문이다

보통 이 3가지 조합을 합쳐서 
입력은 Update, 이동처리는 FixedUpdate, 카메라 움직임은 LateUpdate에 구현해주면 보다 부드럽게 구현 할 수 있다.

---

#### 4. RequireComponent  
스크립트에서 유용하게 사용할 수 있는 기능이다. 만약 한 GameObject에 스크립트를 추가한것 만으로 필요한 컴포넌트등을 추가해주고 싶다면,
```csharp  
[RequireComponent(typeof(CharacterController)), RequireComponent(typeof(NavMeshAgent)), RequireComponent(typeof(Animator))]
public class ControllerCharacter : MonoBehaviour {...}
```  
이렇게 해주면 나중에 ControllerCharacter만 불러오더라도 , RequireComponent에 적은 모든 컴포넌트를 불러올 수 있다.


