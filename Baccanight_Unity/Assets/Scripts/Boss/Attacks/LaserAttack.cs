using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : BossAttack
{
    [System.Serializable]
    private class Laser
    {
        public GameObject InitialCharge;
        public GameObject LaserBeam;
        public GameObject CollisionBeam;
        public SpriteRenderer RendererLaserBeam;

        public Laser(GameObject initialChargePrefab, GameObject laserBeamPrefab, GameObject CollisionBeamPrefab, Vector3 initialPos)
        {
            InitialCharge = ObjectPooler.Instance.SpawnFromPool(initialChargePrefab, initialPos, Quaternion.identity);
            LaserBeam = ObjectPooler.Instance.SpawnFromPool(laserBeamPrefab, initialPos, Quaternion.identity);
            CollisionBeam = ObjectPooler.Instance.SpawnFromPool(CollisionBeamPrefab, initialPos, Quaternion.identity);
            RendererLaserBeam = LaserBeam.GetComponent<SpriteRenderer>();

            DesactiveGameObject();
        }

        public void DesactiveGameObject()
        {
            InitialCharge.SetActive(false);
            LaserBeam.SetActive(false);
            CollisionBeam.SetActive(false);
        }
    }


    #region Inspector
#pragma warning disable 0649
    [Header("General")]
    [Space(5)]
    [SerializeField] private Transform[] m_fireLaserBossPoints;
    [SerializeField] private LayerMask m_layerMask;
    [SerializeField] private BossAIController m_IA;
    [SerializeField] private GameObject m_initialCharge;
    [SerializeField] private GameObject m_laserBeam;
    [SerializeField] private GameObject m_collisionBeam;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 2f)] private float m_chargingTime;
    [SerializeField] [Range(0.1f, 2f)] private float m_AttackDuration;
    [SerializeField] [Range(10f, 40f)] private float m_distance;
    [SerializeField] [Range(0, -2)] private int m_damage;
    [SerializeField] [Range(0.1f, 2f)] private float m_timeBtwLaser; 
    [SerializeField] private MinMaxInt m_laserNumber;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 2f)] private float m_chargingTimeUpgrade;
    [SerializeField] [Range(0.1f, 2f)] private float m_AttackDurationUpgrade;
    [SerializeField] [Range(10f, 30f)] private float m_distanceUpgrade;
    [SerializeField] [Range(0, -2)] private int m_damageUpgrade;
    [SerializeField] [Range(0.1f, 2f)] private float m_timeBtwLaserUpgrade;
    [SerializeField] private MinMaxInt m_laserNumberUpgrade;
#pragma warning restore 0649
    #endregion

    private int m_numberLaserRemaining;
    private Transform m_currentPoint;
    private List<Laser> m_lasers;
    private Transform m_lastPoint;

    [ContextMenu("Start Attack")]
    public override void StartAttack()
    {
        base.StartAttack();
        m_lasers = new List<Laser>();
        m_currentPoint = null;
        m_numberLaserRemaining = m_laserNumber.GetRandomValue(); // Nombre de laser à lancer
        StartCoroutine(HandleAttack());     
    }

    protected override IEnumerator HandleAttack()
    {
        m_numberLaserRemaining--; // Nombre de laser restant à spawn

        do
        {
            m_currentPoint = GetRandomPointSpawn(); // Get du point de spawn
        } while (m_currentPoint == m_lastPoint);

        m_lastPoint = m_currentPoint;

        Laser laser = new Laser(m_initialCharge, m_laserBeam, m_collisionBeam, transform.position);
        m_lasers.Add(laser);

        LaserWarning(laser, m_currentPoint.position);

        yield return new WaitForSeconds(m_chargingTime);

        StartCoroutine(HandleLaser(laser, m_currentPoint.position));

        yield return new WaitForSeconds(/*m_AttackDuration +*/ m_timeBtwLaser);

        // s'il reste des laser à spawn
        if(m_numberLaserRemaining > 0)
        {
            StartCoroutine(HandleAttack());
        }
        else
        {
            EndAttack();
        }    
    }

    private void LaserWarning(Laser laser, Vector2 spawnPointPos)
    {
        laser.InitialCharge.transform.position = spawnPointPos;
        laser.InitialCharge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, m_IA.FlipRight ? -90f : 90));
        laser.InitialCharge.SetActive(true);
    }

    private IEnumerator HandleLaser(Laser laser, Vector2 spawnPointPos)
    {
        /* Start */
        bool hit = false;
        float timeElapsed = 0f;
        float deltaPerFrame = 1;
        
        /* Resize Model_2 */
        Vector2 size = laser.RendererLaserBeam.size;
        size.y = 1;
        laser.RendererLaserBeam.size = size;

        /* Spawn Model_2 */
        laser.LaserBeam.transform.position = spawnPointPos;
        laser.LaserBeam.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, m_IA.FlipRight ? 90f : -90));
        laser.LaserBeam.SetActive(true);

        while (timeElapsed < m_AttackDuration)
        {
            if(!hit)
            {
                RaycastHit2D raycast = Physics2D.Raycast(
                    spawnPointPos, 
                    m_IA.FlipRight ? Vector2.left : Vector2.right, 
                    size.y < m_distance ? size.y + 1 : size.y,
                    m_layerMask);

                //Debug.DrawRay(spawnPointPos, (m_IA.FlipRight ? Vector2.left : Vector2.right) * (size.y + 1), Color.red, Time.deltaTime, false);

                if (raycast.collider)
                {
                    GameObject other = raycast.collider.gameObject;

                    if (other.tag == "Player")
                    {
                        other.GetComponent<Health>().ModifyHealth(m_damage, gameObject);    
                    }

                    hit = true;
                    laser.CollisionBeam.transform.position = raycast.point;
                    laser.CollisionBeam.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, m_IA.FlipRight ? 90f : -90));
                    laser.CollisionBeam.SetActive(true);
                }
            }
            
            if (size.y < m_distance && !hit)
            {
                size.y += deltaPerFrame;
                laser.RendererLaserBeam.size = size;
            }
  
            timeElapsed += Time.deltaTime;

            yield return null;
        }

        laser.DesactiveGameObject();
    }

    private Transform GetRandomPointSpawn()
    {
        return m_fireLaserBossPoints[Random.Range(0, m_fireLaserBossPoints.Length)];
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

        foreach(var laser in m_lasers)
        {
            laser.DesactiveGameObject();
        }
    }

    protected override void EndAttack()
    {
        base.EndAttack();

        foreach (var laser in m_lasers)
        {
            laser.DesactiveGameObject();
        }
    }
}
