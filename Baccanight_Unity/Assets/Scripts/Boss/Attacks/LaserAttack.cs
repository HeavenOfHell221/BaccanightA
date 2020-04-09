using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : BossAttack
{
    #region Inspector
#pragma warning disable 0649
    [Header("General")]
    [Space(5)]
    [SerializeField] private Transform[] m_fireLaserBossPoints;
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private BossAIController m_IA;
    [SerializeField] private GameObject m_Model_1;
    [SerializeField] private GameObject m_Model_2;
    [SerializeField] private GameObject m_Model_3;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 2f)] private float m_chargingTime;
    [SerializeField] [Range(0.1f, 2f)] private float m_AttackDuration;
    [SerializeField] [Range(10f, 40f)] private float m_distance;
    [SerializeField] [Range(0, -2)] private int m_damage;
    [SerializeField] private float m_timeBtwLaser; 
    [SerializeField] private MinMaxInt m_laserNumber;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 2f)] private float m_chargingTimeUpgrade;
    [SerializeField] [Range(0.1f, 2f)] private float m_AttackDurationUpgrade;
    [SerializeField] [Range(10f, 30f)] private float m_distanceUpgrade;
    [SerializeField] [Range(0, -2)] private int m_damageUpgrade;
    [SerializeField] private float m_timeBtwLaserUpgrade;
    [SerializeField] private MinMaxInt m_laserNumberUpgrade;
#pragma warning restore 0649
    #endregion

    private int m_numberLaserRemaining;
    private Transform m_currentPoint;

    private void Awake()
    {
        m_Model_1.SetActive(false);
        m_Model_2.SetActive(false);
        m_Model_3.SetActive(false);
    }


    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        m_numberLaserRemaining = m_laserNumber.GetRandomValue();
        StartCoroutine(HandleAttack());
    }

    protected override IEnumerator HandleAttack()
    {
        m_numberLaserRemaining--;
        m_currentPoint = GetRandomPointSpawn();

        LaserWarning();

        yield return new WaitForSeconds(m_chargingTime);

        SpawnLaser();

        yield return new WaitForSeconds(m_AttackDuration + m_timeBtwLaser);

        if(m_numberLaserRemaining > 0)
        {
            StartCoroutine(HandleAttack());
        }
        else
        {
            EndAttack();
        }    
    }

    [ContextMenu("Upgrade Attack")]
    public override void UpgradeAttack()
    {
        base.UpgradeAttack();
        m_chargingTime = m_chargingTimeUpgrade;
        m_AttackDuration = m_AttackDurationUpgrade;
        m_distance = m_distanceUpgrade;
        m_damage = m_damageUpgrade;
        m_timeBtwLaser = m_timeBtwLaserUpgrade;
        m_laserNumber = m_laserNumberUpgrade;
}

    [ContextMenu("Cancel Attack")]
    public override void CancelAttack()
    {
        StopAllCoroutines();
        base.CancelAttack();
        m_Model_1.SetActive(false);
        m_Model_2.SetActive(false);
        m_Model_3.SetActive(false);
    }

    protected override void EndAttack()
    {
        base.EndAttack();      
    }

    private void LaserWarning()
    {
        m_Model_1.transform.position = m_currentPoint.position;
        m_Model_1.SetActive(true);
    }

    private void SpawnLaser()
    {
        StartCoroutine(HandleLaser());
    }

    private IEnumerator HandleLaser()
    {
        bool hit = false;
        m_Model_2.transform.position = m_currentPoint.position;
        m_Model_2.SetActive(true);

        SpriteRenderer sprite = m_Model_2.GetComponent<SpriteRenderer>();
        Vector2 size = sprite.size;
        size.y = 1;
        sprite.size = size;
        float timeElapsed = 0f;

        while (timeElapsed < m_AttackDuration)
        {
            if(!hit)
            {
                RaycastHit2D raycast = Physics2D.Raycast(m_currentPoint.position, (!m_IA.FlipRight ? Vector2.right : Vector2.left), size.y + 1, m_layerMask);

                if (raycast.collider)
                {
                    GameObject other = raycast.collider.gameObject;
                    if (other.tag == "Player")
                    {
                        other.GetComponent<Health>().ModifyHealth(m_damage, gameObject);    
                    }
                    hit = true;
                    m_Model_3.transform.position = raycast.point;
                    m_Model_3.SetActive(true);
                }
            }
            
            if (size.y < m_distance && !hit)
            {
                size.y += 1f;
                sprite.size = size;
            }
  
            timeElapsed += Time.deltaTime;

            yield return null;
        }
        m_Model_1.SetActive(false);
        m_Model_2.SetActive(false);
        m_Model_3.SetActive(false);
    }

    private Transform GetRandomPointSpawn()
    {
        return m_fireLaserBossPoints[Random.Range(0, m_fireLaserBossPoints.Length)];
    }
}
