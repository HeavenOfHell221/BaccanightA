﻿using UnityEngine;

public class FragmentRetrieve : MonoBehaviour
{
    #region Inspector
    [SerializeField]
    private PlayerSucces m_playerSuccesFragment;
    [SerializeField]
    private string m_fragment;
    #endregion

    #region Getters / Setters
    public string FramentID { get => m_fragment; private set => m_fragment = value; }
    #endregion


    public void PlayerEnter()
    {
        PlayerManager.Instance.PlayerInputController.OnInteract.AddListener(GetFragment);
    }

    public void PlayerExit()
    {
        PlayerManager.Instance.PlayerInputController.OnInteract.RemoveListener(GetFragment);
    }

    private void GetFragment()
    {
        Destroy(gameObject, .5f);
        m_playerSuccesFragment.SetSucces(m_fragment, true);
    }
}
