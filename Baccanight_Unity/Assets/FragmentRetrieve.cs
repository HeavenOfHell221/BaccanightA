using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentRetrieve : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private PlayerAchievement m_playerFragmentSucces;

    [SerializeField]
    private KeySucces m_succes;
    #endregion

    #region Variables
    private InputController m_inputController = null;
    #endregion

    private void Start()
    {
        StartCoroutine(GetPlayerInput());
    }

    private IEnumerator GetPlayerInput()
    {
        while(!PlayerManager.Instance.PlayerReference)
        {
            yield return null;
        }
        m_inputController = PlayerManager.Instance.PlayerReference.GetComponent<InputController>();
    }

    public void PlayerEnter()
    {
       m_inputController.OnInteract.AddListener(GetFragment);
    }

    public void PlayerExit()
    {
        m_inputController.OnInteract.RemoveListener(GetFragment);
    }

    private void GetFragment()
    {
        m_playerFragmentSucces.ValidSucces(m_succes);
        Destroy(gameObject, .5f);
    }
}
