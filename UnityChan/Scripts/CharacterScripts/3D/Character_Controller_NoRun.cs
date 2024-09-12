using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller_NoRun : MonoBehaviour
{
    //컴포넌트들
    //플레이어의 입력을 받는 스크립트
    private CharacterInput _characterInput;
    private Rigidbody characterRigidbody;
    
    //애니메이터
    private Animator characterAnimator;
    
    //컴포넌트끝

    
    //플레이어가 입력한 값이 유효한지?
    private bool IsMoveInput
    {
        get { return !Mathf.Approximately(_characterInput.MoveInput.sqrMagnitude, 0f); }
    }
    
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform characterBody;
    

    
    private Vector3 moveDirection;

    private bool isRotate;
    public bool IsRotate
    {
        get { return isRotate;}
        set { isRotate = value; }
    }

    private bool isMove;
    public bool IsMove
    {
        get { return isMove;}
        set { isMove = value; }
    }

    private const float groundDeceleration = 0.9f;

    //트랜지션 파라미터
    private readonly int hashIsWalk = Animator.StringToHash("IsMove");
    
    //카메라제어
    [SerializeField] private Transform cameraLocation;
    private GameObject myCamera;
    private Vector3 direction;

    private RaycastHit hitInfo;
    private Vector3 initialLocalCameraPosition;
    
    
    //카메라제어 끝

    private void Awake()
    {
        characterAnimator = characterBody.GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();
        myCamera = GameObject.FindWithTag("MainCamera");
        initialLocalCameraPosition = myCamera.transform.localPosition;
    }

    // Start is called before the first frame update
    void Start()
    {
        _characterInput = CharacterInput.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
        PseudoCameraCollider();
        CalculateMove();
    }
    
    // FixedUpdate is called once per end of frame
    void FixedUpdate()
    {
        Translate();
    }

    private void CalculateMove()
    {
        //Debug.Log(IsMoveInput);
        //wasd를 눌러 이동명령을 받는다.
        Vector2 moveInput = _characterInput.MoveInput;
        //카메라기준 이동벡터 형성
        Vector3 lookForward = new Vector3(cameraLocation.forward.x, 0f, cameraLocation.forward.z).normalized;
        Vector3 lookRight = new Vector3(cameraLocation.right.x, 0f, cameraLocation.right.z).normalized;
        //이동벡터 계산하고
        Vector3 direction = lookForward * moveInput.y + lookRight * moveInput.x;
        //대각이동시에도 이동속도를 일정하게 하기위해 노멀라이즈드 == 방향벡터만 추출
        moveDirection = direction.normalized;

        if (IsMoveInput)
        {
            //Vector3.RotateTowards 함수는 현재 방향 벡터를 목표 방향 벡터로 일정한 각속도로 회전시키는 기능을 제공
            //회전한다.
            /*characterBody.forward = Vector3.RotateTowards(characterBody.forward, moveDirection,
                rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 1000f);*/
            Vector3 rotateTowards = Vector3.RotateTowards(characterBody.forward, moveDirection,
                rotateSpeed * Mathf.Deg2Rad * Time.fixedDeltaTime, 1000f);

            characterBody.forward = rotateTowards == Vector3.zero ? Vector3.forward : rotateTowards;


            RotateForward(moveDirection);
            IsMove = true;
            characterAnimator.SetBool(hashIsWalk, true);
        }
        else
        {
            IsMove = false;
            characterAnimator.SetBool(hashIsWalk, false);
        }
    }
    
    void RotateForward(Vector3 _moveDir)
    {
        //현재 바라보고 있는 방향과, 이동하려는 방향의 각도차이를 계산한다.
        float angleDifference = Vector3.Angle(characterBody.forward, _moveDir);

        if (angleDifference < 115.0f)//회전끝
        {
            IsRotate = false;
        }
        else//회전중
        {
            //나 회전중이야
            IsRotate = true;
        }
    }

    void Translate()
    {
        if (IsMove)
        {
            //walk
            characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, moveDirection * moveSpeed, Time.fixedDeltaTime*moveSpeed);
            //Debug.Log(isRun + "CharController");
            //run, when player press leftShift
        }
        else
        {
            characterRigidbody.velocity *= groundDeceleration;
            if (characterRigidbody.velocity.magnitude < 0.1f)
            {
                characterRigidbody.velocity = Vector3.zero;
            }
        }
        if (IsRotate)
        {
            //회전하는 동안느려질게?
            characterRigidbody.velocity = Vector3.Lerp(characterRigidbody.velocity, moveDirection * moveSpeed / 4f, Time.fixedDeltaTime*moveSpeed);    
        }
    }
    
    void Look()
    {
        if (!CharacterInput.Instance.Input_Block)
        {
            //우리가 보는 화면은 2차원 평면이다. 마우스는 평면안에 있기 때문에, 그 위치를 좌표평면상에 나타낼 수 있다.
            Vector2 mouseDelta = _characterInput.CameraInput;
        
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
    }

    /*//지형지물 디버그용
     private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.transform.parent != null)
        {
            Debug.Log(other.gameObject.transform.parent.gameObject.name);    
        }
        else
        {
            Debug.Log(other.gameObject.name);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
    }*/

    private void PseudoCameraCollider()
    {
        direction = myCamera.transform.position - cameraLocation.transform.position;
        float distance = 1.0f;
        //레이캐스트 가시화
        Debug.DrawRay(cameraLocation.transform.position, direction * distance, Color.red);
        
        if (Physics.Raycast(cameraLocation.transform.position, direction, out hitInfo, distance * 3.0f))
        {
            /*Debug.Log(Vector3.Distance(myCamera.transform.position, cameraLocation.transform.position));*/
            /*Vector3 offset = direction * 0.1f;*/
            StartCoroutine(MoveCameraForward());
            //myCamera.transform.position = hitInfo.point/* - offset*/;
            /*if (Vector3.Distance(myCamera.transform.position, cameraLocation.transform.position) > minDistance)
            {
                myCamera.transform.position = hitInfo.point;
            }*/
        }
        else
        {
            StartCoroutine(MoveCameraBack());
            /*myCamera.transform.localPosition = initialLocalCameraPosition;*/
        }
    }
    
    private IEnumerator MoveCameraBack()
    {
        //두 지점의 거리차이가 일정 이상인 동안
        while (Vector3.Distance(myCamera.transform.localPosition, initialLocalCameraPosition) > 0.1f)
        {
            //부드럽게 이동시킨다.
            myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, initialLocalCameraPosition, Time.deltaTime * 1.0f);
            yield return null; // 다음 프레임까지 대기
        }
        // 정확하게 초기 위치로 설정
        myCamera.transform.localPosition = initialLocalCameraPosition;
    }

    private IEnumerator MoveCameraForward()
    {
        //부드럽게 이동시킨다.
        myCamera.transform.position =
            Vector3.Lerp(myCamera.transform.position, hitInfo.point, Time.deltaTime * 2.5f);
        yield return null; // 다음 프레임까지 대기
    }
    //캐릭터가 너무 가까워지면 캐릭터를 그리지 않는다.
}
