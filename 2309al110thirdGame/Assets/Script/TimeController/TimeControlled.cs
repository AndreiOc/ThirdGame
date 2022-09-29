using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControlled : MonoBehaviour
{
    public Vector2 _velocity;
    public AnimationClip currentAnimation;
    public float animationTime;

    public virtual void  TimeUpdate()
    {
        if(currentAnimation != null)
        {
            animationTime += Time.deltaTime;
            if(animationTime > currentAnimation.length)
            {
                animationTime = animationTime - currentAnimation.length;
            }
        }
    }
    public void UpdateAnimation()
    {
        if(currentAnimation != null)
        {
            currentAnimation.SampleAnimation (gameObject,animationTime);
        }
    }
}
