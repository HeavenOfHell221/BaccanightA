using UnityEngine;

public class ActiveModel : MonoBehaviour
{
    [SerializeField] private GameObject m_gameObject;

    public void ActiveObject()
    {
        m_gameObject.SetActive(true);
    }
}
