using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossAttack : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
#pragma warning restore 0649
    #endregion

    #region Variables
    #endregion

    #region Inspector
    public bool IsStarted { get; private set; } = false;
    public bool IsFinish { get; private set; } = false;
    public bool IsCanceled { get; private set; } = false;
    public bool IsUpgraded { get; private set; }= false;
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
