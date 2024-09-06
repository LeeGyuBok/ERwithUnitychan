using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    //이 스크립트가 붙으면 벌어지는 일
    private EnemyStateMachine _enemyStateMachine;

    public EnemyStateMachine _EnemyStateMachine
    {
        get { return _enemyStateMachine; }
    }

    private void Awake()
    {
        //1. 멤버변수 _에너미스테이트머신에 클래스 에너미스테이트머신을 인스턴스화하여 할당(이 변수는 이하 ESM.)
        _enemyStateMachine = new EnemyStateMachine(this);
        //9. 이제, ESM은, 아이들스테이트인스턴스와 패트롤스테이트인스턴스를 갖게되었다.
    }

    // Start is called before the first frame update
    void Start()
    {
        //10. 이제 에너미컨트롤러가 잠에서 깨어나 할일을 시작한다. 아래는 첫 번째 할 일이다.
        //    첫번째 할 일은 ESM의 이니셜라이즈 함수를 호출하는 것이다. 호출한다.
        //    인자로 받은 스테이트는 아이들스테이트이다. 다시 ESM클래스로 가자(12).
        _enemyStateMachine.Initialize(_enemyStateMachine.idleState);
        //14. 정리: 결국 에너미컨트롤러는 ESM을 통해 이니셜라이즈 함수를 호출했고,
        //         이니셜라이즈 함수는 인자로 받은 스테이트의 엔터함수를 호출한다.
    }

    // Update is called once per frame
    //15. 스타트함수가 끝났으니 업데이트 함수를 호출한다.
    void Update()
    {
        //ESM의 업데이트 함수를 호출한다.
        _enemyStateMachine.Update();
    }
}
