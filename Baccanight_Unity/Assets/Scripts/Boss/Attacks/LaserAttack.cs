using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("General")]
    [Space(5)]
    [SerializeField] private Transform m_fireLaserBoss;
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private BossAIController m_IA;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 1f)] private float m_chargingTime;
    [SerializeField] [Range(0.1f, 1f)] private float m_AttackDuration;
    [SerializeField] [Range(10f, 30f)] private float m_distance;
    [SerializeField] [Range(0, -2)] private int m_damage;
    [SerializeField] private MinMaxInt m_laserNumber;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 1f)] private float m_chargingTimeUpgrade;
    [SerializeField] [Range(0.1f, 1f)] private float m_AttackDurationUpgrade;
    [SerializeField] [Range(10f, 30f)] private float m_distanceUpgrade;
    [SerializeField] [Range(0, -2)] private int m_damageUpgrade;
    [SerializeField] private MinMaxInt m_laserNumberUpgrade;
#pragma warning restore 0649
    #endregion



    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();

        float number = m_laserNumber.GetRandomValue();

        for(int i = 0; i < number; i++)
        {
            StartCoroutine(HandleAttack());
        }
    }

    protected override IEnumerator HandleAttack()
    {
        LaserWarning();

        yield return new WaitForSeconds(m_chargingTime);

        SpawnLaser();

        yield return new WaitForSeconds(m_AttackDuration);

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

    private void LaserWarning()
    {

    }

    private void SpawnLaser()
    {
        StartCoroutine(LaserDetection());
    }

    private IEnumerator LaserDetection()
    {
        while(!IsFinish)
        {
            RaycastHit2D raycast = Physics2D.Raycast(m_fireLaserBoss.position, (m_IA.FlipRight ? Vector2.right : Vector2.left), m_distance, m_layerMask);

            if (raycast.collider)
            {
                GameObject other = raycast.collider.gameObject;
                if (other.tag == "Player")
                {
                    other.GetComponent<Health>().ModifyHealth(m_damage, gameObject);
                    Debug.DrawRay(m_fireLaserBoss.position, (m_IA.FlipRight ? Vector2.right : Vector2.left) * m_distance, Color.red);
                } 
            }
            else
            {
                Debug.DrawRay(m_fireLaserBoss.position, (m_IA.FlipRight ? Vector2.right : Vector2.left) * m_distance, Color.green);
            }

            yield return null;
        }
    }
}
