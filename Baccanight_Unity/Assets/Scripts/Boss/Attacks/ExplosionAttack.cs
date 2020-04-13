using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("General")]
    [Space(5)]
    [SerializeField] private GameObject m_explosionFireball;

    [Header("Attributes")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 25f)] private float m_ballSpeedFlat;
    [SerializeField] [Range(20, 200)] private int m_numberPerBurst = 50;
    [SerializeField] [Range(0.05f, 0.5f)] private float m_cooldownBetweenFireball = 0.1f;

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
        for (int i = 0; i < m_numberPerBurst; i++)
        {
            GameObject obj = ObjectPooler.Instance.SpawnFromPool(m_explosionFireball, transform.position, Quaternion.identity, transform);
            obj.GetComponent<ExplosionFireball>().Speed = m_ballSpeedFlat;
            yield return new WaitForSecondsRealtime(m_cooldownBetweenFireball);
        }

        yield return new WaitForSecondsRealtime(1f);

        EndAttack();
    }

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack();
    }
}
