using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyIdleState : IState
{
    private EnemyController enemy;

    //4. 아이들스테이트를 인스턴스화한다.
    public EnemyIdleState(EnemyController _enemy)
    {
        //5. 이때 받는 인자를 설정한다.
        enemy = _enemy;
    }

    //13. ESM은 이니셜라이즈 함수를 통해 아이들스테이트의 엔터함수를 호출했다.
    public void Enter()
    {
        //디버그로그를 남긴다.
        //Debug.Log("Idle enter");
    }

    //17. 아이들스테이트의 업데이트함수이다.
    public void Update()
    {
        //디버그를 남기고 
        //Debug.Log("Idle update");
        //에너미컨트롤러를 할당한 에너미변수의 ESM을 통해서 트랜지션투함수를 호출한다. 트랜지션투함수를 확인하러간다.
        enemy._EnemyStateMachine.TransitionTo(enemy._EnemyStateMachine.patrolState);
        
    }

    public void Exit()
    {
        //Debug.Log("Idle exit");
    }

}
