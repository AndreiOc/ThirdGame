using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageController : MonoBehaviour
{

    // Start is called before the first frame update
    public float _speed = 20f;
    public Rigidbody2D _rb2D;
    public Animator _animator;
    private Transform shootPosition;
    private Transform wallhootPosition;
    public SpriteRenderer _spriteRenderer;
    public TypeOfMage _type;
    private void Awake()
    {
            shootPosition = GameObject.Find("shootPosition").GetComponent<Transform>();
            if(shootPosition.localPosition.x < 0)
                _spriteRenderer.flipX = true;
            else if(shootPosition.localPosition.x > 0)
                _spriteRenderer.flipX = false;        
        
    }
    void Start()
    {
        if(_type == TypeOfMage.Dynamic)
            _rb2D.velocity = shootPosition.localPosition * _speed;
        else if(_type == TypeOfMage.Static)
        {

 
        }
    }

    // Update is called once per frame

    public void SetBorn()
    {
        _animator.SetTrigger("Born");

    }
    public void SetGo()
    {
        _animator.SetTrigger("Go");
    }
}
public enum TypeOfMage
{
    Dynamic,
    Static
}
