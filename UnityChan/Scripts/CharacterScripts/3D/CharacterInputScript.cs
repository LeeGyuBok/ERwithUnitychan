using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput : MonoBehaviour
{
    public static CharacterInput Instance
    {
        get { return _characterInput;}
    }

    private static CharacterInput _characterInput;

    private Vector2 move;
    public Vector2 MoveInput
    {
        get {return move;} 
    }
    
    public bool IsRun => Input.GetKey(KeyCode.LeftShift) ? true : false;

    private Vector2 camera;

    public Vector2 CameraInput
    {
        get { return camera; }
    }
    
    //플레이어의 인풋을 막아요
    public bool Input_Block { get; set; }

    private void Awake()
    {
        if (_characterInput == null)
            _characterInput = this;
        
        else if (_characterInput != this)
            throw new UnityException($"wrong Instance {_characterInput.name}.");
    }
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        camera.Set(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));    
    }
}
