using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    public IState curentState { get; private set; }

    public EnemyIdleState idleState;
    public EnemyPatrolState patrolState;

    public event Action<IState> stateChanged; 

    //2. 인스턴스화하는 과정에서 
    public EnemyStateMachine(EnemyController enemy)
    {
        //3. 아이들스테이트를 할당한다.
        idleState = new EnemyIdleState(enemy);
        //6. 패트롤스테이트를 할당한다.
        patrolState = new EnemyPatrolState(enemy);
    }

    //11. 에너미컨트롤러가 ESM을 통해 이니셜라이즈 함수를 호출하는데, 특정 스테이트를 인자로 받는다.
    public void Initialize(IState startingState)
    {
        //ESM은 현재 스테이트에 대한 변수를 갖고있으며,
        curentState = startingState;
        //그 변수에 할당된 스테이트로 진입(enter)한다. 에너미컨트롤러로 돌아가서 어떤 스테이트를 인자로 받는지 확인하자.
        //_enemyStateMachine.Initialize(_enemyStateMachine.idleState);
        //12. 스타팅스테이트는 아이들스테이트이므로 아이들스테이트의 Enter함수를 호출한다. 
        startingState.Enter();
        
       // stateChanged?.Invoke(startingState);
    }

    //18. 트랜지션투함수이다. 커런트스테이트의 exit함수를 호출하고, 인자로 받은 스테이트를 할당한 뒤, 그 스테이트의 엔터함수를 호출한다.
    //enemy._EnemyStateMachine.TransitionTo(enemy._EnemyStateMachine.patrolState);
    //위 코드는 아이들 함수의 업데이트 함수이다. 11과 비슷한 내용으로, 정리하면 패트롤스테이트를 인자로 받았다.
    public void TransitionTo(IState nextState)
    {
        //아이들스테이트의 엑시트함수를 호출하고
        curentState.Exit();
        //커런트스테이트에 패트롤스테이트를 할당하고
        curentState = nextState;
        //패트롤스테이트의 엔터함수를 호출한다.
        //이 후부터는 아이들스테이트의 호출순서와 동일하다.
        nextState.Enter();
        //curentState.Enter(); 왜 이게 아니지? -> 같은 결과값을 가지지만, 직관적으로 봤을 때, '다음 상태로 진입한다' 라는 의미를 더 명확히한다. 
        
     //   stateChanged?.Invoke(nextState);
    }

    //16. 이 부분이다.
    public void Update()
    {
        //11에서 , 커런트스테이트는 아이들스테이트가 할당되어있다. 그러므로 널이 아니다.
        if (curentState != null)
        {
            //커런트스테이트의 업데이트함수를 호출하는데 위에 설명했듯, 커런트스테이트는 아이들스테이트이다.
            //아이들스테이트의 업데이트함수 내용을 확인하러 가자.
            curentState.Update();
        }
    }
}
