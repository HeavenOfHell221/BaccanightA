using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingMovement : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 1f;
    
    private PatrolPath m_path;
    private Vector3 m_destination;
    private Rigidbody2D rb;

    private void Start()
    {
        m_path = GetComponent<PatrolPath>();
        rb = GetComponent<Rigidbody2D>();
        m_destination = m_path.GetPathNodeByIndex(0);
        StartCoroutine(UpdateDestination());
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private IEnumerator UpdateDestination()
    {
        float dist = Vector3.Distance(m_destination, transform.position);
        if(dist < 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
            m_destination = m_path.GetNextPathNode();
        }
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UpdateDestination());
    }

    private void ApplyMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_destination, m_speed * Time.fixedDeltaTime);
    }

    public void AddPlayerInChildren(GameObject player)
    {
        player.transform.SetParent(gameObject.transform, true);
    }

    public void RemovePlayerInChildren(GameObject player)
    {
        player.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerAndCam").transform, true);
        player.transform.localScale = new Vector3(1, 1, 1);
    }
}
