using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorRequirement : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField]
    private PlayerSucces m_playerSucces;
    [SerializeField]
    private string m_succesForOpenDoor;
    [SerializeField]
    private bool m_allwaysOpen;
    [SerializeField]
    private FadeTextMeshProUGUI m_fadeText;
    [SerializeField] private SpriteRenderer m_renderer;
    [SerializeField] private Sprite m_open;
    [SerializeField] private Sprite m_close;
    [SerializeField] private Animator m_anim;
#pragma warning restore 0649
    #endregion

    #region Variables
    private BoxCollider2D m_DoorCollider;
    #endregion

    
    void Start() 
    {
        m_fadeText.enabled = false;
        m_DoorCollider = gameObject.GetComponent<BoxCollider2D>();
        StartCoroutine(TestDoor());     
    }

    public void OpenDoor()
    {
        m_DoorCollider.enabled = true;
        if(m_renderer)
            m_renderer.sprite = m_open;
        m_fadeText.enabled = true;
        if (m_anim)
            m_anim.enabled = true;
    }

    public void CloseDoor()
    {
        m_DoorCollider.enabled = false;
        if (m_renderer)
            m_renderer.sprite = m_close;
        m_fadeText.enabled = false;
    }
    private IEnumerator TestDoor()
    {
        yield return new WaitForSeconds(0.5f);

        if (m_allwaysOpen || m_playerSucces.HaveSucces(m_succesForOpenDoor))
        {
            OpenDoor();
        }
        else
        {
            StartCoroutine(TestDoor());
        }
        
    }
}
