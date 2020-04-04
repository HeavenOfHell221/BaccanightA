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
    public bool IsStarted { get; protected set; }
    public bool IsFinish { get; protected set; }
    public bool IsCanceled { get; protected set; }
    public bool InProgress { get; protected set; }
    #endregion

    public abstract void StartAttack();
    public abstract IEnumerator HandleAttack();
    public abstract void EndAttack();
}
