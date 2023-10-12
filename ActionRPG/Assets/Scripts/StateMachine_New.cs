using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class State<T> // generic 클래스로 C++에서의 템플릿과 비슷하다.
{
    protected StateMachine_New<T> stateMachine; // StateMachine에 접근하기 위한 변수
    protected T context; // 이 상태의 소유자에 대한 context. MonoBehaviour 의 update를 호출받기 때문에 T에는 Monobehaviour이 들어올것.

    public State()
    {

    }
    internal void SetStateMachineAndContext(StateMachine<T> stateMachine, T context) // internal은 같은 어셈블리 즉 namespace 내에서만 접근 가능하다.
    {
        this.stateMachine = stateMachine;
        this.context = context;

        OnInitialized(); // 상태가 초기화 되었을 때 호출되는 함수
    }
    public virtual void OnInitialized() { } // virtual은 상속받은 클래스에서 재정의 할 수 있다는 의미이다.
    public virtual void OnEnter() { }
    public abstract void Update(float deltaTime) { } // 반드시 구현해야함.
    public virtual void OnExit() {}
}
public sealed class StateMachine_New<T> // 더이상 변형이 없도록 sealed사용
{
    private T context;
    private State<T> currentState;
    public State<T> CurrentState => currentState; // C# 6.0부터 추가된 속성. get만 있고 set이 없는 속성을 의미한다.
    private State<T> previousState;
    public State<T> PriviousState => previousState;
    private float elapsedTimeInState = 0.0f; // 상태가 변화한 후 경과한 시간을 체크해줌. 앞으로 AI를 구현할때 많은 도움이 된다.
    public float ElapsedTimeInState => elapsedTimeInState;
    private Dictionary<System.Type, State<T>> states = new Dictionary<System.Type, State<T>>(); // 상태를 저장할 딕셔너리

    public StateMachine_New(T context, State<T> initialState)
    {
        this.context = context;
        AddState(initialState); // 초기 상태를 추가해준다.
        currentState = initialState;
        currentState.OnEnter(); // 초기상태가 바로 진행되도록 한다.
    }

    public void AddState(State<T> state)
    {
        state.SetStateMachineAndContext(this, context); // 상태를 추가할 때마다 상태머신과 context를 넘겨준다.
        states[state.GetType()] = state; // 딕셔너리에 상태를 추가해준다. GetType()은 해당 클래스의 타입을 반환해준다.
    }

    public void Update(float deltaTime)
    {
        elapsedTimeInState += deltaTime;
        currentState.Update(deltaTime);
    }


    public R ChangeState<R>() where R : State<T> // R은 State<T>를 상속받은 클래스만 가능하다는 의미이다.
    // 상태가 전환되었을때를 구현할것 이므로 ,이전상태에 현재상태를 넣어주고, 현재상태를 덮어씌운다.
    {
        var newType = typeof(R);
        if(currentState.GetType() == newType)
        {
            return currentState as R;
        }
        if(currentState != null)
        {
            currentState.OnExit();
        }
        previousState = currentState;
        currentState = states[newType];
        currentState.OnEnter();
        elapsedTimeInState = 0.0f;

        return currentState as R;
    }
}
