using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackBehaviour : MonoBehaviour
{   // 추후 상속시켜 줄 것이므로, 여기서는 기본적인 구조와 틀만 잡아준다. 바로 사용하지 못하게 abstract클래스로 해주자
    #region Variables
    #if UNITY_EDITOR
    [Multiline] public string developerDescription = "";  // 이걸 구현한 컴포넌트를 한 게임오브젝트에 여러개 추가할 수 있게 디자인해서, 설명을 달아줄수 있게한다.
    #endif //unity editor
    public int animationIndex;
    public int priority;    // 2개의 공격이 있다고 가정해보았을때 둘다 쿨타임이 돌았다면 어떠한 공격을 우선으로 사용할 것인가.
    public int damage = 10;
    public float range = 3.0f;
    
    [SerializeField]
    private float coolTime = 1.0f;
    protected float calcCoolTime = 0.0f;

    public GameObject effectPrefab;
    public LayerMask targetMask;
    [HideInInspector] public bool isAvailable;
    #endregion Variables
    
    // Start is called before the first frame update
    void Start()
    {
        calcCoolTime = coolTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (calcCoolTime < coolTime)
        {
            calcCoolTime += Time.deltaTime;
            isAvailable = false;
        }else if(calcCoolTime >= coolTime)
        {
            isAvailable = true;
            calcCoolTime = 0.0f;
        }
    }

    public abstract void ExecuteAttack(GameObject target = null, Transform startPoint = null);
    // 범위공격은 타겟이 필요없고, 스타트 포인트가 없는공격또한 있으므로 둘다 Null 로 기본설정
}
