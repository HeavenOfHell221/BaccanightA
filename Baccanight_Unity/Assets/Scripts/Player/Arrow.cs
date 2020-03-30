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
    private PlayerMotion m_playerMotion;

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
        if(m_playerMotion.FlipSprite == -1)
        {
            m_arrowSpeed *= -1;
            gameObject.transform.localScale = new Vector3(-1, 1, 1); 
        }

        m_arrowSpeed.y = Random.Range(-0.01f, 0.01f);

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
        m_rigidbody.velocity = Vector2.zero;
        Destroy(gameObject, 0.5f);
    }
}
