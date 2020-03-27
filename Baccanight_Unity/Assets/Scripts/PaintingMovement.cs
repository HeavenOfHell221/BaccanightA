using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingMovement : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 1f;
    
    private PatrolPath m_path;
    private Vector3 m_destination;

    private void Start()
    {
        m_path = GetComponent<PatrolPath>();
        m_destination = m_path.GetPathNodeByIndex(0);
    }

    private void Update()
    {
        UpdateDestination();
        ApplyMovement();
    }

    private void UpdateDestination()
    {
        float dist = Mathf.Abs(m_destination.x - transform.position.x);
        if(dist < 0.1f)
        {
            m_destination = m_path.GetNextPathNode();
        }
    }

    private void ApplyMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_destination, m_speed * Time.deltaTime);
    }
}
