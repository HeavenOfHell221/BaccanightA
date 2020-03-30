using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649

    [SerializeField]
    private List<Transform> m_pathNodes;

    [SerializeField]
    private int m_StartNodeIndex = 0;

    [Range(-1, 1)]
    [SerializeField]
    private int m_NextRelativeIndex = 1;

    [SerializeField]
    private bool m_pingPongPatrol = false;

#pragma warning restore 0649
    #endregion

    #region Variables

    public int m_actualNodeIndex;

    #endregion

    void Awake()
    {
        if (m_pathNodes.Count == 0)
        {
            GameObject defaultPos = new GameObject("Node" + name);
            defaultPos.transform.SetParent(transform.parent);
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
}
