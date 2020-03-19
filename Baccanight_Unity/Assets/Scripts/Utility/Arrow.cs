using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField]
    private Vector2 m_arrowSpeed;

    [SerializeField]
    private Rigidbody2D m_rigidbody;

    [SerializeField]
    private SpriteRenderer m_sprite;

	//[SerializeField]
	//private LayerMask whatIsGround;
	//[SerializeField]
	//private LayerMask whatIsBoss;

	//private LayerMask HitMask;

	//[SerializeField]
	//private Transform m_HitCheck;

	void Start()
    {
        //HitMask = whatIsBoss | whatIsGround;
        if(!PlayerManager.Instance.IsPlayerLookHeadIsLeft)
        {
            m_arrowSpeed *= -1;
            m_sprite.flipX = true;

        }

        m_rigidbody.AddForce(m_arrowSpeed, ForceMode2D.Force);
    }

    /*void FixedUpdate()
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
	}*/

    public void OnEnter(GameObject target)
    {
        /*
         * Si le layer de target c'est le boss alors récupérer faire les dégâts
         * Si le layer c'est le stage alors faire disparaitre la flèche
         * 
         */
        Debug.Log("Je suis entrée en contact avec le layer : " + target.layer);
        Destroy(gameObject, .05f);
    }
}
