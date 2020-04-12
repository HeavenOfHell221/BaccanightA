using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintingMovement : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 1f;

    [SerializeField]
    private float m_timeWait = 0.1f;

    [SerializeField]
    private bool m_canMove = true;
    
    private PatrolPath m_path;
    private Vector3 m_destination;

    private void Start()
    {
        if (m_canMove)
        {
            m_path = GetComponent<PatrolPath>();
            m_destination = m_path.GetPathNodeByIndex(0);
            StartCoroutine(UpdateDestination());
        }
    }

    private void FixedUpdate()
    {
        if (m_canMove)
        {
            ApplyMovement();
        }
    }

    private IEnumerator UpdateDestination()
    {
        float dist = Vector3.Distance(m_destination, transform.position);
        if(dist < 0.1f)
        {
            yield return new WaitForSeconds(m_timeWait);
            m_destination = m_path.GetNextPathNode();
        }
        yield return new WaitForSeconds(0.05f);
        StartCoroutine(UpdateDestination());
    }
    
    private void ApplyMovement()
    {
        Vector3 newPos = Vector3.MoveTowards(transform.position, m_destination, m_speed * Time.fixedDeltaTime);
        transform.localPosition = newPos;
    }

    public void AddPlayerInChildren(GameObject player)
    {
        player.transform.SetParent(gameObject.transform, true);
        player.transform.rotation = new Quaternion(0f, player.transform.localRotation.y,
            0f, player.transform.localRotation.w);
    }

    public void StayPlayer(GameObject player)
    {
        player.transform.rotation = new Quaternion(0f, player.transform.localRotation.y,
            0f, player.transform.localRotation.w);
    }

    public void RemovePlayerInChildren(GameObject player)
    {
        player.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerAndCam").transform, true);
        player.transform.localScale = new Vector3(1, 1, 1);
        player.transform.rotation = new Quaternion(0f, player.transform.localRotation.y,
            0f, player.transform.localRotation.w);
    }
}
