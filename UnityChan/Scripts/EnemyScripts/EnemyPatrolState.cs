using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IState
{
    private EnemyController enemy;

    //7. 패트롤 스테이트를 할당하는 과정에서
    public EnemyPatrolState(EnemyController _enemy)
    {
        //8. 이때받는 인자를 설정한다.
        enemy = _enemy;
    }

    public void Enter()
    {
        //Debug.Log("Patrol enter");
    }

    public void Update()
    {
        //Debug.Log("Patrol update");
        enemy._EnemyStateMachine.TransitionTo(enemy._EnemyStateMachine.idleState);
        
    }

    public void Exit()
    {
        //Debug.Log("Patrol exit");
    }
}
