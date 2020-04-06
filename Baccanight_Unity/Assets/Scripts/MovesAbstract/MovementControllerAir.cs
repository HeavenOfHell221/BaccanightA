using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerAir : MovementController
{
    //[Header("Movement Controller Air", order = 1)]

    public override void OnMove(Vector2 motion)
    {
        Move = motion;
    }

    virtual protected void FixedUpdate()
    {
        if (m_canMove)
        {
            ApplyMovement();
        }
    }

    public override void ApplyMovement()
    {
        if (Mathf.Abs(Move.x) > GameConstants.LimitDicretePosition || Mathf.Abs(Move.y) > GameConstants.LimitDicretePosition)
        {
            float velocityX = Mathf.Lerp(Rigidbody.velocity.x, Move.x * Speed, m_smoothSpeed);
            float velocityY = Mathf.Lerp(Rigidbody.velocity.y, Move.y * Speed, m_smoothSpeed);
            Rigidbody.velocity = new Vector2(velocityX, velocityY);
        }
        else
        {
            Rigidbody.velocity = new Vector2(0f, 0f);
        }
    }
}
