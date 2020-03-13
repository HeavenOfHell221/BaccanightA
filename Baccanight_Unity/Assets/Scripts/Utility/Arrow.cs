using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float ArrowSpeed = 20f;

	[SerializeField]
	private LayerMask whatIsGround;
	[SerializeField]
	private LayerMask whatIsBoss;

	private LayerMask HitMask;

	[SerializeField]
	private Transform m_HitCheck;

	void Start()
    {
		HitMask = whatIsBoss | whatIsGround;
    }

    void FixedUpdate()
    {
		transform.position = transform.forward * ArrowSpeed;
		CheckHit();
    }

	protected void CheckHit()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_HitCheck.position, .05f, HitMask);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				if (colliders[i].gameObject.layer == whatIsBoss)
				{
					//Deal Damage
				}
				Destroy(gameObject);
			}
		}
	}
}
