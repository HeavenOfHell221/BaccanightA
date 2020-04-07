using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 1f)] float m_chargingTime;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 1f)] float m_chargingTimeUpgrade;
#pragma warning restore 0649
    #endregion

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        StartCoroutine(HandleAttack());
    }

    protected override IEnumerator HandleAttack()
    {
        /* Prépare le laser */
        yield return new WaitForSeconds(m_chargingTime);
        /* Tire le laser */

        EndAttack();
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_chargingTime = m_chargingTimeUpgrade;
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack();
    }
}
