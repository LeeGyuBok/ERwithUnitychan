using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class CharacterScript : MonoBehaviour
{
    [SerializeField] private Transform characterBody;

    private Rigidbody characterRigidbody;
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float decelerationFactor;

    [FormerlySerializedAs("characterInputScripts")] [SerializeField] private CharacterInput characterInput;
    /*
    [SerializeField] private float currentSpeed;*/

    [SerializeField] private Transform cameraLocation;

    private Animator characterAnimator;

    private Vector3 moveDir;
    private bool IsWalk;
    private bool IsRun;

    private void Awake()
    {
        characterInput = GetComponent<CharacterInput>();//Awake와 동시에 할당되요
        characterAnimator = characterBody.GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        moveDir = Vector3.zero;
        IsWalk = false;
        IsRun = false;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        MoveDir();
    }
    
    void Look()
    {
        //우리가 보는 화면은 2차원 평면이다. 마우스는 평면안에 있기 때문에, 그 위치를 좌표평면상에 나타낼 수 있다.
        Vector2 mouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        
        //쿼터니언을 오일러각으로 변환하여 x,y,z축에 대한 회전으로 분해하여 표현.
        //카메라포지션의 현재 회전을 오일러각으로 변환하여 저장.
        Vector3 camAngle = cameraLocation.rotation.eulerAngles;
        
        //2. 이 코드가 회전각도를 제한
        float x = camAngle.x - mouseDelta.y;
        if (x < 180f) 
        {
            x = Mathf.Clamp(x, -1f, 70f);
        }
        else
        {
            x = Mathf.Clamp(x, 335f, 361f);
        }
        
        //1. 이 코드만 존재하면 회전각도를 제한하지 않음
        //마우스의 x좌표(mouseDelta.x)가 바뀐다는 것은 Y축(camAngle.y)을 기준으로 바뀐다.
        //마우스의 y좌표(mouseDelta.y)가 바뀐다는 것은 X축(camAngle.x)을 기준으로 바뀐다.
        cameraLocation.rotation = Quaternion.Euler(x, camAngle.y + mouseDelta.x, camAngle.z);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (IsWalk)
        {
            characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, moveDir * moveSpeed, Time.fixedDeltaTime*moveSpeed);    
        }

        if (IsWalk && IsRun)
        {
            characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, moveDir * moveSpeed * 2f, Time.fixedDeltaTime*moveSpeed);
        }
        
    }

    private void MoveDir()
    {
        //wasd를 눌러 이동명령을 받는다.
        Vector2 moveInput = characterInput.MoveInput;
        //카메라기준 이동벡터 형성
        Vector3 lookForward = new Vector3(cameraLocation.forward.x, 0f, cameraLocation.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraLocation.right.x, 0f, cameraLocation.right.z).normalized;
        //이동벡터 계산하고
        Vector3 moveDirection = lookForward * moveInput.y + lookRight * moveInput.x;
        //대각이동시 이동속도를 일정하게 하기위해 노멀라이즈드 == 방향벡터만 추출
        moveDir = moveDirection.normalized;

        Walk();
    }
      
    void Walk()
    {
        //입력받은 벡터3가 제로벡터가 아니라면 == 인풋이 있다면
        if (moveDir != Vector3.zero)
        {
            //Vector3.RotateTowards 함수는 현재 방향 벡터를 목표 방향 벡터로 일정한 각속도로 회전시키는 기능을 제공
            //회전한다.
            characterBody.forward = Vector3.RotateTowards(characterBody.forward, moveDir,
                rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 1000f);

            RotateForward(moveDir);
            characterAnimator.SetBool("IsWalk", true);
            IsWalk = true;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                Run();
            }
            
            //fixedUpdate로 이사가야함
            /*characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, _moveDir * moveSpeed, Time.fixedDeltaTime*moveSpeed);*/
        }
        else//제로 벡터라면 == 인풋이 없다면
        {
            characterAnimator.SetBool("IsRun", false);
            IsRun = false;
            characterAnimator.SetBool("IsWalk", false);
            IsWalk = false;
        }
    }

    void Run()
    {
        //케릭터가 movedir방향을 보면서 움직이면 좋겠따.
        if (IsWalk)
        {
            //Vector3.RotateTowards 함수는 현재 방향 벡터를 목표 방향 벡터로 일정한 각속도로 회전시키는 기능을 제공
            //회전한다.
            characterBody.forward = Vector3.RotateTowards(characterBody.forward, moveDir,
                rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 1000f);
            
            RotateForward(moveDir);
            characterAnimator.SetBool("IsRun", true);
            IsRun = true;
            
            //fixedUpdate로 이사가야함
            /*characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, _moveDir * moveSpeed * 2f, Time.fixedDeltaTime*moveSpeed);*/
        }
        //Debug.DrawRay(cameraLocation.position, new Vector3(cameraLocation.forward.x, 0f, cameraLocation.forward.z).normalized, Color.red);
    }
    
    void RotateForward(Vector3 _moveDir)
    {
        //현재 바라보고 있는 방향과, 이동하려는 방향의 각도차이를 계산한다.
        float angleDifference = Vector3.Angle(characterBody.forward, _moveDir);

        if (angleDifference < 115.0f)//회전끝
        {
            characterAnimator.SetBool("IsRotating", false);
        }
        else//회전중
        {
            //나 회전중이야
            characterAnimator.SetBool("IsRotating", true);
            //느려질게?
            characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, _moveDir * moveSpeed / 4f, Time.fixedDeltaTime*moveSpeed);
        }
    }
}
