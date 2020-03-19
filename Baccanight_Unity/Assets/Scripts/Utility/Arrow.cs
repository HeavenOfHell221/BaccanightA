using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private float ArrowSpeed = 0.3f;

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
		transform.position = new Vector3(transform.position.x+ArrowSpeed, transform.position.y, transform.position.z);
		CheckHit();
    }

	private void CheckHit()
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_HitCheck.position, 0.15f, HitMask);
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
