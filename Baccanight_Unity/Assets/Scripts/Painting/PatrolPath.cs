using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649

    [SerializeField] private GameObject m_nodeParent;
    [SerializeField] private int m_StartNodeIndex = 0;
    [SerializeField] [Range(-1, 1)] private int m_NextRelativeIndex = 1;
    [SerializeField] private bool m_pingPongPatrol = false;

#pragma warning restore 0649
    #endregion

    #region Variables
    public int m_actualNodeIndex = 0;
    private List<Transform> m_pathNodes = new List<Transform>();
    #endregion

    void Awake()
    {
        m_actualNodeIndex = m_StartNodeIndex;

        if(m_nodeParent)
        {
            for (int i = 0; i < m_nodeParent.transform.childCount; i++)
            {
                m_pathNodes.Add(m_nodeParent.transform.GetChild(i));
            }           
        }

        if (m_pathNodes.Count == 0)
        {
            GameObject defaultPos = new GameObject("Node " + name);
            defaultPos.transform.SetParent(null);
            defaultPos.transform.position = transform.position;
            m_pathNodes.Add(defaultPos.transform);
        }
    }

    public Vector2 GetNextPathNode()
    {
        int next = (m_actualNodeIndex + m_NextRelativeIndex);

        if ((next >= m_pathNodes.Count || next < 0) && m_pingPongPatrol)
        {
            m_NextRelativeIndex *= -1;
            next += m_NextRelativeIndex;
        }
        else if (next >= m_pathNodes.Count)
        {
            next = 0;
        }
        else if (next < 0)
        {
            next = 0;
            m_NextRelativeIndex = 1;
        }
        m_actualNodeIndex = next;
        return GetPathNodeByIndex(next);
    }

    public Vector2 GetPathNodeByIndex(int NodeIndex)
    {
        if (NodeIndex < 0 || NodeIndex >= m_pathNodes.Count || m_pathNodes[NodeIndex] == null)
        {
            return Vector2.zero;
        }

        return new Vector2(m_pathNodes[NodeIndex].position.x, m_pathNodes[NodeIndex].position.y);
    }

    public float GetDistanceToNode(Vector3 origin, int destinationNodeIndex)
    {
        if (destinationNodeIndex < 0 || destinationNodeIndex >= m_pathNodes.Count || m_pathNodes[destinationNodeIndex] == null)
        {
            return -1f;
        }

        return (m_pathNodes[destinationNodeIndex].position - origin).magnitude;
    }

    public Vector2 GetClosestNode(Vector3 origin)
    {
        int closestNodeIndex = -1;
        float closestDist = float.MaxValue;

        for(int i = 0; i < m_pathNodes.Count; i++)
        {
            float dist = GetDistanceToNode(origin, i);
            if(dist != -1f && dist < closestDist)
            {
                closestNodeIndex = i;
                closestDist = dist;
            }
        }

        if(closestNodeIndex == -1)
        {
            return Vector2.zero;
        }
        else
        {
            m_actualNodeIndex = closestNodeIndex;
            return GetPathNodeByIndex(closestNodeIndex);
        }  
    }
}
