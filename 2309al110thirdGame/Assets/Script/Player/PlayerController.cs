using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{



    //!Oggetti attaccati al player
    private BoxCollider2D _bc2D;
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private SpriteRenderer _spriterenderer;

    //!Logica script
    private Vector2 _movementInput;
    [SerializeField] private float _speed = 1f;
    private Vector3 _shootVector3Position;
    private Vector3 _wallhoottVector3Position;

    
    //!Abilità magie etc...
    public bool _canMove = true;
    private Camera _camera;
    private Vector3 _worldPosition;
    [SerializeField] GameObject _shootPosition;
    [SerializeField] GameObject _wallhootPosition;
    [SerializeField] GameObject _aquaBullet;
    [SerializeField] GameObject _waterBlast;


    
    void Start()
    {
        _bc2D = GetComponent<BoxCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _spriterenderer = GetComponent<SpriteRenderer>();
        _shootVector3Position =_shootPosition.transform.position;
        _wallhoottVector3Position = _wallhootPosition.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if(_canMove)
        {
            _movementInput.y = 0f;
            if(!(_movementInput.x ==0))
            {
                if(_movementInput.x > 0)
                {
                    _spriterenderer.flipX = false;
                    _shootPosition.transform.localPosition = _shootVector3Position;
                    _wallhootPosition.transform.localPosition = _wallhoottVector3Position;
                }
                else if(_movementInput.x < 0)
                {
                    _spriterenderer.flipX = true;
                    _shootPosition.transform.localPosition = new Vector3(-1 * _shootVector3Position.x, _shootVector3Position.y,0);
                    _wallhootPosition.transform.localPosition = new Vector3(-1 * _wallhoottVector3Position.x, _wallhoottVector3Position.y,0);

                }
                MovePlayer(_movementInput);   
                _animator.SetInteger("state",(int)AniamtionState.Running);
            }
            else
            {
                _animator.SetInteger("state",(int)AniamtionState.Idleing) ;
            }
        }
    }

    private void TeleportTo(Vector3 mousePosition)
    {
        transform.position = mousePosition;
    }

    private void MovePlayer(Vector2 _direction)
    {
        _rb2D.MovePosition((Vector2)transform.position + _direction * _speed *Time.fixedDeltaTime);
    }

    void OnMove(InputValue movementValue)
    {
        _movementInput = movementValue.Get<Vector2>();
    }

    public void LockMovement()
    {
        _canMove = false;
    }
    public void UnLockMovement()
    {
        _canMove = true;
    }

    void OnTeleport()
    {

    }

    void OnShoot()
    {
       Instantiate (_aquaBullet,_shootPosition.transform.position,_shootPosition.transform.rotation);
    }
    void OnWallHoot()
    {
       Instantiate (_waterBlast,_wallhootPosition.transform.position,_wallhootPosition.transform.rotation);

    }

}

    

public enum AniamtionState
{
    Idleing,
    Running
}
