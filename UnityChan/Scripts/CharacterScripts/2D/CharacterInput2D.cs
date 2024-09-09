using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInput2D : MonoBehaviour
{
    public static CharacterInput2D Instance
    {
        get { return _characterInput;}
    }
    
    private static CharacterInput2D _characterInput;
    
    private Vector2 move;
    
    public Vector2 MoveInput
    {
        get {return move;} 
    }
    private void Awake()
    {
        if (_characterInput == null)
            _characterInput = this;
        
        else if (_characterInput != this)
            throw new UnityException($"wrong instance {_characterInput.name}.");
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move.Set(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        Debug.Log(move);
    }
}
