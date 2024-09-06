using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SDChracterScripts : MonoBehaviour
{
    [SerializeField] private GameObject _followObject;
    private CharacterInput _followObjectInput;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private float _maxDistance;
    private float currentDistance;
    private Vector3 direction;
    [SerializeField] private float speed;
    
    private readonly int hashIsMove = Animator.StringToHash("IsMove");

    private bool isMove;
    
    private bool needRun;

    public bool IsMove
    {
        get { return isMove; }
        set { isMove = value; }
    }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        _maxDistance = 3f;
        IsMove = false;
        direction = (_followObject.transform.position - transform.position).normalized;
        transform.LookAt(direction);
    }

    // Start is called before the first frame update
    void Start()
    {
        //트라이겟컴포넌트 함수 용법.
        //out ~ 에서 ~에 해당하는 변수의 타입과 일치하는 컴포넌트를 가져오려고 시도한다.
        //이때, 가져올 수 있으면(해당 타입의 컴포넌트가 있다면) true를 반환하고, 해당 컴포넌트를 out이하 변수에 할당한다.
        //그렇지 않으면  false를 반환하고 null을 할당한다.
        //_followObject.TryGetComponent(out _followObjectInput);
        //순서 - 1. 캐릭터인풋인스턴스(싱글톤)을 할당받는다(start함수, 왜냐하면 인풋스크립트에서 싱글톤을 awake에서 만들어 할당하는 순서를 보장하기위해).
        _followObjectInput = CharacterInput.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        currentDistance = Vector3.Distance(_followObject.transform.position, transform.position);
        direction = (_followObject.transform.position - transform.position).normalized;
        //Debug.Log(currentDistance);
        if (currentDistance > 1.2f)
        {
           
            transform.forward = Vector3.RotateTowards(transform.forward, direction,
                300f * Mathf.Deg2Rad * Time.fixedDeltaTime, 600f);
        }
        
        //현재거리가 최대거리보다 커지면 이동을 준비한다. 현재거리 > 2f
        if (currentDistance > _maxDistance)
        {
            //이동 시작!
            IsMove = true;
            _animator.SetBool(hashIsMove, IsMove);
            //Debug.Log(currentDistance);
        }
        else if(currentDistance < 1.5f)
        {
            IsMove = false;
            _animator.SetBool(hashIsMove, IsMove);
            //Debug.Log(IsMove);
        }
    }

    private void FixedUpdate()
    {
        if (_animator.GetBool(hashIsMove))
        {
            Move();
        }
    }

    void Move()
    {
        //Debug.Log("Fast!!!!");
        
        //2. 캐릭터인풋인스턴스의 IsRun프로퍼티를 통해 lefshift를 눌렀는지 안눌렀는지 '지속적으로' 확인한다. -> 확인할 값을 update나 fixedupdate에서 '지속적으로' 할당한다.
        needRun = _followObjectInput.IsRun;
        //Debug.Log(needRun + "SDChar");
        _rigidbody.velocity = needRun ? Vector3.Lerp(_rigidbody.velocity, direction * (speed * 2f), Time.fixedDeltaTime * speed) : Vector3.Lerp(_rigidbody.velocity, direction * speed, Time.fixedDeltaTime * speed);
        /*if (currentDistance > 1.5f) 팔로우오브젝트와 일정거리가 될 때 까지쫓아간다. == 팔로우오브젝트의 포지션과 이 스크립트를 갖는 오브젝트의 포지션이 일정거리 이하가 될 때까지
        {
            //Debug.Log("Move!!!!");
        }*/
        if (_animator.GetBool(hashIsMove) && currentDistance > 10f)
        {
            Debug.Log("TooFar");
            transform.position = _followObject.transform.position - new Vector3(1.0f, 0, 1.0f);
        }
    }
    
}
