using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementControllerAir : MovementController
{
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

    override protected void ApplyMovement()
    {
        if (Mathf.Abs(Move.x) > GameConstants.LimitDicretePosition || Mathf.Abs(Move.y) > GameConstants.LimitDicretePosition)
        {
            float velocityX = Mathf.Lerp(Rigidbody.velocity.x, Move.x * m_speed, m_smoothSpeed);
            float velocityY = Mathf.Lerp(Rigidbody.velocity.y, Move.y * m_speed, m_smoothSpeed);
            Rigidbody.velocity = new Vector2(velocityX, velocityY);
        }
        else
        {
            Rigidbody.velocity = new Vector2(0f, 0f);
        }
    }
}
