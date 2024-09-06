using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Controller2D : MonoBehaviour
{
    private CharacterInput2D characterInput;
    private Rigidbody characterRigidbody;
    
    //애니메이터
    private Animator characterAnimator;
    
    private bool IsMoveInput
    {
        get { return !Mathf.Approximately(characterInput.MoveInput.sqrMagnitude, 0f); }
    }
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private Transform characterBody;
    [SerializeField] private Transform cameraLocation;

    private Vector2 moveDirection;
    
    private bool isRotate;
    
    public bool IsRotate
    {
        get { return isRotate;}
        set { isRotate = value; }
    }
    
    private readonly int hashIsWalk = Animator.StringToHash("IsMove");

    private void Awake()
    {
        characterAnimator = characterBody.GetComponent<Animator>();
        characterRigidbody = GetComponent<Rigidbody>();
    }

    // Start is called before the first frame update
    void Start()
    {
        characterInput = CharacterInput2D.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        characterAnimator.SetBool(hashIsWalk, true);
        characterRigidbody.velocity = Vector2.Lerp(characterRigidbody.velocity,
            characterInput.MoveInput.normalized * moveSpeed, Time.fixedDeltaTime);
    }

    
}
