using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [Header("States")]
    [Space(5)]
    [SerializeField] private bool m_isStarted = false;
    [SerializeField] private bool m_isUpgraded = false;
    [SerializeField] private bool m_isCanceled = false;
    [SerializeField] private bool m_isFinish = false;
#pragma warning restore 0649
    #endregion

    #region Variables
    #endregion

    #region Inspector
    public bool IsStarted { get => m_isStarted; private set => m_isStarted = value; }
    public bool IsFinish { get => m_isFinish; private set => m_isFinish = value; }
    public bool IsCanceled { get => m_isCanceled; private set => m_isCanceled = value; }
    public bool IsUpgraded { get => m_isUpgraded; private set => m_isUpgraded = value; }
    #endregion
   
    protected abstract IEnumerator HandleAttack();

    public virtual void StartAttack()
    {
        IsStarted = true;
        IsFinish = false;
        IsCanceled = false;
    }

    protected virtual void EndAttack()
    {
        IsStarted = false;
        IsFinish = true;
        IsCanceled = false;
    }

    public virtual void CancelAttack()
    {
        IsStarted = false;
        IsFinish = false;
        IsCanceled = true;
    }

    public virtual void UpgradeAttack()
    {
        IsUpgraded = true;
    }
}
