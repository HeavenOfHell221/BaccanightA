using UnityEngine;


/// <summary>
/// Handle trigger detection for invoke events in response.
/// </summary>
public class EventScriptTrigger : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649

    [SerializeField] private GameObjectEvent OnEnterDetect;
    [SerializeField] private GameObjectEvent OnStayDetect;
    [SerializeField] private GameObjectEvent OnExitDetect;
    [SerializeField] private LayerMask Layers;

    private Collider2D m_myCollider;
#pragma warning restore 0649
    #endregion

    private void Awake()
    {
        m_myCollider = GetComponent<Collider2D>();
        m_myCollider.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & Layers) != 0)
        {
            OnEnterDetect.Invoke(other.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & Layers) != 0)
        {
            OnExitDetect.Invoke(other.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & Layers) != 0)
        {
            OnStayDetect.Invoke(other.gameObject);
        }
    }

    private void OnDisable()
    {
        m_myCollider.isTrigger = false;
        m_myCollider.enabled = false;
    }

    private void OnEnable()
    {
        m_myCollider.isTrigger = true;
        m_myCollider.enabled = true;
    }
}

