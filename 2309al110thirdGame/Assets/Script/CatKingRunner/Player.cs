using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //!Game
    public float distance = 0f;

    //!Runn
    public float acceleration = 10;
    public float maxAcceleration = 10;
    public float maxVelocity = 100f;

    //Thre perfetct jump essere tuo
    //!Gravity and jump creation
    public float gravity;
    public Vector2 velocity;
    public float jumpVelocity = 20;
    public float groundHeight = 10;
    public bool isGrounded = false;

    //!Jump logic 
    public bool isHoldingJump = false;
    public float maxHoldJump = 0.4f;
    public float holdJumpTimer = 0.0f;
    public float jumpGroundThreshold = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 pos = transform.position;
        float groundDistance = Mathf.Abs(pos.y - groundHeight);
        if(isGrounded || groundDistance <= jumpGroundThreshold)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                isGrounded = false;
                velocity.y = jumpVelocity;
                isHoldingJump = true;
                holdJumpTimer = 0;

            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            isHoldingJump = false;
        }
    }
    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        //Sto saltando in questo momento
        if(!isGrounded)
        {   
            if(isHoldingJump)
            {
                holdJumpTimer += Time.fixedDeltaTime;
                if(holdJumpTimer >= maxHoldJump)
                    isHoldingJump = false;
            }
            pos.y += velocity.y * Time.fixedDeltaTime;
            if(!isHoldingJump)
                velocity.y += gravity * Time.fixedDeltaTime;

            Vector2 rayOrigin  = new Vector2(pos.x + 0.7f,pos.y);
            Vector2 rayDirection = Vector2.up;
            float rayDistance = velocity.y * Time.fixedDeltaTime;
            RaycastHit2D hit2D = Physics2D.Raycast(rayOrigin,rayDirection,rayDistance);
            ///Quando torno a terra risetto tutto 
            // if(pos.y <= groundHeight)
            // {
            //     pos.y = groundHeight;
            //     isGrounded = true;

            // }
        }
        //Sto correndo in questo momento
        if(isGrounded)
        {            
            float velocityRatio = velocity.x / maxVelocity;
            acceleration = maxAcceleration *(1 - velocityRatio);
            velocity.x += acceleration * Time.fixedDeltaTime;
            if(velocity.x >= maxVelocity)
            {
                velocity.x = maxVelocity;
            }
        }
        distance += velocity.x * Time.fixedDeltaTime;
        transform.position = pos; 
    }


    
}
