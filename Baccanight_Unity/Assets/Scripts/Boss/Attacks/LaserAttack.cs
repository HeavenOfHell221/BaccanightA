using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserAttack : BossAttack
{
    [System.Serializable]
    private class Laser
    {
        public GameObject InitialCharge;
        public GameObject InitialCharge_Mini;
        public GameObject LaserBeam;
        public GameObject LaserBeam_Mini;
        public GameObject CollisionBeam;
        public SpriteRenderer RendererLaserBeam;
        public SpriteRenderer RendererLaserBeam_Mini;
        public Vector3 InitialPosition;

        public Laser(GameObject initialChargePrefab, GameObject laserBeamPrefab, GameObject CollisionBeamPrefab,
            GameObject laserBeam_miniPrefab, GameObject initialCharge_miniPrefab, Vector3 initialPos)
        {
            InitialPosition = initialPos;
            InitialCharge = ObjectPooler.Instance.SpawnFromPool(initialChargePrefab, initialPos, Quaternion.identity);
            LaserBeam = ObjectPooler.Instance.SpawnFromPool(laserBeamPrefab, initialPos, Quaternion.identity);
            CollisionBeam = ObjectPooler.Instance.SpawnFromPool(CollisionBeamPrefab, initialPos, Quaternion.identity);

            LaserBeam_Mini = ObjectPooler.Instance.SpawnFromPool(laserBeam_miniPrefab, initialPos, Quaternion.identity);
            InitialCharge_Mini = ObjectPooler.Instance.SpawnFromPool(initialCharge_miniPrefab, initialPos, Quaternion.identity);

            RendererLaserBeam = LaserBeam.GetComponent<SpriteRenderer>();
            RendererLaserBeam_Mini = LaserBeam_Mini.GetComponent<SpriteRenderer>();

            DesactiveGameObject();
        }

        public void DesactiveGameObject()
        {
            InitialCharge.SetActive(false);
            LaserBeam.SetActive(false);
            CollisionBeam.SetActive(false);
            LaserBeam_Mini.SetActive(false);
            InitialCharge_Mini.SetActive(false);
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
    [SerializeField] private GameObject m_initialCharge_mini;
    [SerializeField] private GameObject m_laserBeam_mini;
    [SerializeField] private float m_distance;
    [SerializeField] private float m_speedLaser;
    [SerializeField] private float m_speedLaserMini;

    [Header("Phase 1")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 2f)] private float m_chargingTime;
    [SerializeField] [Range(0.1f, 2f)] private float m_AttackDuration;  
    [SerializeField] [Range(0, -2)] private int m_damage;
    [SerializeField] [Range(0.1f, 2f)] private float m_timeBtwLaser; 
    [SerializeField] private MinMaxInt m_laserNumber;

    [Header("Phase 2")]
    [Space(5)]
    [SerializeField] [Range(0.1f, 2f)] private float m_chargingTimeUpgrade;
    [SerializeField] [Range(0.1f, 2f)] private float m_AttackDurationUpgrade;
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
        Transform randomTransform;
        m_lastPoint = m_currentPoint;

        m_numberLaserRemaining--; // Nombre de laser restant à spawn

        do
        {
            randomTransform = GetRandomPointSpawn(); // Get du point de spawn
        } while (randomTransform == m_lastPoint);

        m_currentPoint = randomTransform;
        
        Laser laser = new Laser(m_initialCharge, m_laserBeam, m_collisionBeam, m_laserBeam_mini, m_initialCharge_mini,  m_currentPoint.position);

        m_lasers.Add(laser);

        StartCoroutine(LaserWarning(laser));

        yield return new WaitForSeconds(m_chargingTime);

        StartCoroutine(HandleLaser(laser));

        yield return new WaitForSeconds(m_timeBtwLaser);

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

    private IEnumerator LaserWarning(Laser laser)
    {
        float timeElapsed = 0f; // Durée écoulée
        RaycastHit2D ray;
        bool hit = false;
        Vector2 hitPos = Vector2.zero;

        Vector2 size = laser.RendererLaserBeam_Mini.size;
        size.x = 1;
        laser.RendererLaserBeam_Mini.size = size;

        laser.InitialCharge_Mini.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, m_IA.FlipRight ? -90f : 90));
        laser.InitialCharge_Mini.SetActive(true);

        laser.LaserBeam_Mini.transform.rotation = Quaternion.Euler(new Vector3(0f, m_IA.FlipRight ? 0 : 180, 0f));
        laser.LaserBeam_Mini.SetActive(true);

        while (timeElapsed < m_chargingTime)
        {
            ray = Physics2D.Raycast(
                    new Vector2(laser.InitialPosition.x, laser.InitialPosition.y), // Origin
                    m_IA.FlipRight ? Vector2.left : Vector2.right, // Direction
                    size.x < m_distance - m_speedLaser ? size.x + m_speedLaserMini : size.x, // Distance
                    m_layerMask); // Layers

            if(ray.collider)
            {
                hitPos = ray.point;
                hit = true;
            }

            if (size.x < m_distance)
            {
                if(hit)
                {
                    size.x += (Vector2.Distance(hitPos, laser.InitialPosition) - size.x);
                }
                else
                {
                    size.x += m_speedLaserMini;     
                }
                laser.RendererLaserBeam_Mini.size = size;
            }

            timeElapsed += Time.deltaTime;

            yield return null;
        }
    }

    private IEnumerator HandleLaser(Laser laser)
    {
        /* Préparatifs */
        /* Start */
        bool hitStage = false; // Est-ce qu'on a hit le stage
        bool hitPlayer = false;
        bool collisionBeamHasSpawn = false;
        float timeElapsed = 0f; // Durée écoulée
        float distanceBtwTwoRaycast = 0.25f; // Distance (en y) avec le centre du laser
        RaycastHit2D raycastTop;
        RaycastHit2D raycastBottom;
        Vector3 hitPos = Vector3.zero;

        laser.InitialCharge.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, m_IA.FlipRight ? -90f : 90));
        laser.InitialCharge.SetActive(true);
        laser.InitialCharge_Mini.SetActive(false);

        Vector2 size = laser.RendererLaserBeam.size;
        size.x = 1;
        laser.RendererLaserBeam.size = size;

        laser.LaserBeam.transform.rotation = Quaternion.Euler(new Vector3(0f, m_IA.FlipRight ? 0 : 180, 0f));
        laser.LaserBeam.SetActive(true);

        while (timeElapsed < m_AttackDuration)
        {
            /* Lancement des raycasts */
            {
                raycastTop = Physics2D.Raycast(
                    new Vector2(laser.InitialPosition.x, laser.InitialPosition.y + distanceBtwTwoRaycast), // Origin
                    m_IA.FlipRight ? Vector2.left : Vector2.right, // Direction
                    size.x < m_distance - m_speedLaser ? size.x + m_speedLaser : size.x, // Distance
                    m_layerMask); // Layers

                raycastBottom = Physics2D.Raycast(
                    new Vector2(laser.InitialPosition.x, laser.InitialPosition.y - distanceBtwTwoRaycast), // Origin
                    m_IA.FlipRight ? Vector2.left : Vector2.right, // Direction
                    size.x < m_distance - m_speedLaser ? size.x + m_speedLaser : size.x, // Distance
                    m_layerMask); // Layers

                //Debug.DrawRay(new Vector2(laser.InitialPosition.x, laser.InitialPosition.y + distanceBtwTwoRaycast), (m_IA.FlipRight ? Vector2.left : Vector2.right) * (size.x < m_distance - deltaPerFrame ? size.x + deltaPerFrame : size.x), Color.red, Time.deltaTime, false);
                //Debug.DrawRay(new Vector2(laser.InitialPosition.x, laser.InitialPosition.y - distanceBtwTwoRaycast), (m_IA.FlipRight ? Vector2.left : Vector2.right) * (size.x < m_distance - deltaPerFrame ? size.x + deltaPerFrame : size.x), Color.blue, Time.deltaTime, false);
            }

            /* Tests de collision des raycasts */
            {
                if (raycastTop.collider)
                {
                    GameObject other = raycastTop.collider.gameObject;

                    if (other.tag == "Player")
                    {
                        other.GetComponent<Health>().ModifyHealth(m_damage, gameObject);
                        hitPlayer = true;
                        
                    }
                    else if(other.tag == "Fence" || other.tag == "Stage")
                    {
                        hitStage = true;
                    }
                    hitPos = raycastTop.point;
                }

                if (!hitPlayer && !hitStage && raycastBottom.collider)
                {
                    GameObject other = raycastBottom.collider.gameObject;

                    if (other.tag == "Player")
                    {
                        other.GetComponent<Health>().ModifyHealth(m_damage, gameObject);
                        hitPlayer = true;
                        
                    }
                    else if (other.tag == "Fence" || other.tag == "Stage")
                    {
                        hitStage = true;
                    }
                    hitPos = raycastBottom.point;
                }
            }

            if(!collisionBeamHasSpawn)
            {
                if((hitStage && !hitPlayer && size.x < m_distance) || (!hitStage && hitPlayer && size.x < m_distance))
                {  
                    collisionBeamHasSpawn = true;
                }
            }
  
            if (size.x < m_distance)
            {
                if (collisionBeamHasSpawn)
                {
                    size.x += (Vector2.Distance(hitPos, laser.InitialPosition) - size.x);
                }
                else
                {
                    size.x += m_speedLaser;
                }
                
                laser.RendererLaserBeam.size = size;
            }

            if(collisionBeamHasSpawn)
            {
                /* Le laser est arrêté par le stage */
                laser.CollisionBeam.transform.position = new Vector2(m_IA.FlipRight ? laser.InitialPosition.x - size.x : laser.InitialPosition.x + size.x, laser.InitialPosition.y);
                laser.CollisionBeam.transform.rotation = Quaternion.Euler(new Vector3(0f, m_IA.FlipRight ? 0f : 180f, 0f));
                laser.CollisionBeam.SetActive(true);
                laser.LaserBeam_Mini.SetActive(false);
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

        /*foreach (var laser in m_lasers)
        {
            laser.DesactiveGameObject();
        }*/
    }
}
