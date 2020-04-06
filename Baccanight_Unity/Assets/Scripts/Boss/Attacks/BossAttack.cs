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
    public bool IsStarted { get; private set; }
    public bool IsFinish { get; private set; }
    public bool IsCanceled { get; private set; }
    public bool IsUpgraded { get; private set; }
    #endregion
   
    protected abstract IEnumerator HandleAttack();

    [ContextMenu("Start Attack")]
    public virtual void StartAttack()
    {
        IsStarted = true;
    }

    [ContextMenu("End Attack")]
    protected virtual void EndAttack()
    {
        IsFinish = true;
    }

    [ContextMenu("Cancel Attack")]
    protected virtual void CancelAttack()
    {
        IsCanceled = true;
    }

    [ContextMenu("Upgrade Attack")]
    public virtual void UpgradeAttack()
    {
        IsUpgraded = true;
    }
}
