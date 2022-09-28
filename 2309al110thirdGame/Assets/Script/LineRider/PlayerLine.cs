using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLine : MonoBehaviour
{

    public Rigidbody2D rb2D;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb2D.bodyType = RigidbodyType2D.Dynamic;       
        }
    }
}
