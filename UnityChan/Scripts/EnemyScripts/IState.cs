using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Enter()
    {
        //상태 진입시 실행되는 코드, 원본코드와 동일
    }

    void Update()
    {
        //프레임당 로직. 새로운 상태로 전환하는 조건 포함, 원본코드에서는 Execute
    }

    void Exit()
    {
        //상태 벗어날 때 실행되는 코드, 원본코드와 동일
    }
}
