using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerTimeController : TimeControlled
{

    float _moveSpeed = 5;
    float _jumpVelocity = 40;
    public override void TimeUpdate()
    {
        base.TimeUpdate();
        Vector2 pos = transform.position;
        pos.y += _velocity.y *Time.deltaTime;
        _velocity.y += TimeController._gravity * Time.deltaTime;


        if(pos.y < 1)
        {
            pos.y = 1;
            _velocity.y = 0;
        }
        if(Input.GetKeyDown(KeyCode.W))
        {
            _velocity.y = _jumpVelocity;
        }
        if(Input.GetKey(KeyCode.A))
        {
            pos.x -= _moveSpeed * Time.deltaTime;

        }
        if(Input.GetKey(KeyCode.D))
        {
            pos.x += _moveSpeed * Time.deltaTime;   
        }



        transform.position = pos;
    }
}