using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIController : MonoBehaviour
{
    #region Inspector attributes
#pragma warning disable 0649
    [Header("Components")]
    [Space(10)]
    [SerializeField] private BossMovementControllerAir m_movementC;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Animator m_animator;
    [SerializeField] private PatrolPath m_patrolPath;
    [SerializeField] private HealthBoss m_health;
    //[SerializeField] private GameObject m_spawnBoss;

    [Header("Attributes")]
    [Space(10)]
    [SerializeField] [Range(15, 30)] private float m_distanceStartBattle;
    [SerializeField] [Range(0.01f, 1f)] private float m_distanceToNextNode;
#pragma warning restore 0649
    #endregion

    #region BossAttack
#pragma warning disable 0649
    [Header("Attacks")]
    [Space(10)]
    [SerializeField] private BossAttack m_startBattle;
    [Space(5)]
    [SerializeField] private BossAttack[] m_basicAttacks; // Les 6 attaques de base (seulement 4 dispo au début du fight)
    [Space(5)]
    [SerializeField] private BossAttack m_counterAttackAttack; // La contre-attaque pendant l'attaque "Shield"
    [Space(5)]
    [SerializeField] [Range(1, 6)] private float[] m_timesBetweenAttack;

    [Header("Variables")]
    [Space(10)]
    [SerializeField] private BossAttack m_currentAttack = null;
    [SerializeField] private BossActionType m_currentState = BossActionType.Idle;
#pragma warning restore 0649
    #endregion

    #region Variables
    private Transform m_player = null; // Transform du joueur
    private float m_timeBetweenAttack; // cooldown entre deux attaques
    private int m_timeBetweenAttackIndex = 0; // Index pour le tableau des cooldowns entre deux attaques
    private int m_basicAttackPossible = 4;
    private bool m_battleHasStart = false;
    public LinkedList<BossAttack> m_lastAttacks = new LinkedList<BossAttack>();
    #endregion

    #region Getters / Setters
    public BossActionType CurrentState { get => m_currentState; private set => m_currentState = value; }
    public bool FlipRight { get; private set; } = true;
    #endregion

    private void Start()
    {
        m_player = PlayerManager.Instance.PlayerReference.transform;
        m_timeBetweenAttack = m_timesBetweenAttack[m_timeBetweenAttackIndex];
    }

    private void Update()
    {
        HandleDirectionToPlayer();
        UpdateStates();      
    }

    private void UpdateStates()
    {
        float distanceFromPlayer = DistanceFromPlayer();

        switch (CurrentState)
        {
            case BossActionType.Idle:
                if (distanceFromPlayer < m_distanceStartBattle)
                {
                    UpdateIABehaviour(BossActionType.StartBattle);
                }
                break;
            default:
                break;
        }

        if (m_currentAttack && m_currentAttack.IsFinish)
        {
            m_currentAttack = null;
            StartCoroutine(NextAttack(m_timeBetweenAttack));
        }
    }

    private IEnumerator NextAttack(float timeWait, int attackID = -1)
    {
        yield return new WaitForSecondsRealtime(timeWait);

        if(attackID != -1)
        {
            CurrentState = BossActionType.Attacking;
            HandleAttackingState(attackID);
        }
        else
        {
            UpdateIABehaviour(BossActionType.Attacking);
        }

        
    }

    public void UpdateIABehaviour(BossActionType newState)
    {
        CurrentState = newState;

        switch(CurrentState)
        {
            case BossActionType.Idle:
                HandleIdleState();
                break;

            case BossActionType.Moving:
                HandleMovingState();
                break;

            case BossActionType.Attacking:
                HandleAttackingState();
                break;

            case BossActionType.Dying:
                HandleDyingState();
                break;

            case BossActionType.Stuning:
                HandleStuningState();
                break;

            case BossActionType.Enraging:
                HandleEnragingState();
                break;

            case BossActionType.StartBattle:
                HandleStartBattleState();
                break;

            case BossActionType.CounterAttack:
                HandleCounterAttackState();
                break;

            default:
                break;
        }
    }

    private void HandleStartBattleState()
    {
        if(!m_battleHasStart)
        {
            m_battleHasStart = true;

            /* 
             * Animation : Cri de guerre  
             */

            Fence fence = GameObject.FindGameObjectWithTag("Fence").GetComponent<Fence>();
            fence.CloseFence();
        }

        m_health.IsInvincible = false;

        StartCoroutine(NextAttack(m_timeBetweenAttack));
    }

    private void HandleCounterAttackState()
    {
        if(m_currentAttack)
        {
            m_currentAttack.CancelAttack();
        }

        m_currentAttack = m_counterAttackAttack;
        m_counterAttackAttack.StartAttack();
    }

    private void HandleIdleState()
    {
        if(m_currentAttack)
        {
            m_currentAttack.CancelAttack();
            m_currentAttack = null;
        }
    }

    private void HandleMovingState()
    {
        if (m_movementC.DistanceFromDestination(m_movementC.Destination) < m_distanceToNextNode)
        {
            m_movementC.NewDestination(m_patrolPath.GetNextPathNode());
            m_movementC.ApplyMovement();
        }
    }

    private void HandleStuningState()
    {

    }

    private void HandleAttackingState(int attackID = -1)
    {
        StartCoroutine(_HandleAttackingState(attackID));
    }

    private IEnumerator _HandleAttackingState(int attackID = -1)
    { 
        if (m_currentAttack == null)
        {
            BossAttack newAttack = m_basicAttacks[attackID == -1 ? Random.Range(0, m_basicAttackPossible) : attackID];

            // Si y'a pas encore 2 attaques de déjà faites
            if (m_lastAttacks.Count < 2)
            {
                m_lastAttacks.AddFirst(newAttack);
            }
            else
            {
                // Si le boss veut faire 3 fois la même attaque d'affilé
                if (newAttack == m_lastAttacks.First.Value && newAttack == m_lastAttacks.First.Next.Value)
                {
                    yield return null;
                    HandleAttackingState(); // On reprend une attaque au hasard
                }
                else
                {
                    // On décale l'attaque en position 0 en 1
                    m_lastAttacks.First.Next.Value = m_lastAttacks.First.Value;
                    // On ajoute la nouvelle attaque en position 0
                    m_lastAttacks.AddFirst(newAttack);
                }

                //Debug.Log(m_lastAttacks.First.Value.name + " | " + m_lastAttacks.First.Next.Value.name);
            }

            m_currentAttack = newAttack;
            m_currentAttack.StartAttack();
        }
    }

    private void HandleEnragingState()
    {
        // Amélioration des attaques
        for (int i = 0; i < m_basicAttackPossible; i++)
        {
            if (m_basicAttacks[i] && !m_basicAttacks[i].IsUpgraded)
            {
                m_basicAttacks[i].UpgradeAttack();
            }
        }

        // Nouvelle attaque disponible
        m_basicAttackPossible++;

        UpgradeSpeedBetweenTwoAttacks();

        StartCoroutine(_HandleEnragingState());
    }

    private IEnumerator _HandleEnragingState()
    {
        if(m_currentAttack)
        {
            m_currentAttack.CancelAttack();
            m_currentAttack = null;
        }

        m_animator.SetTrigger("Enraged");
        PlayerManager.Instance.ShakeCamera.Shake(3f, 1f, 1f);

        while(m_health.IsInvincible)
        {
            yield return null;
        }

        StartCoroutine(NextAttack(0.8f, 4));
    }

    private void HandleDyingState()
    {
        Destroy(gameObject);
    }

    private float DistanceFromPlayer()
    {
        if (m_player)
        {
            return Vector2.Distance(transform.position, m_player.position);
        }

        return float.MaxValue;
    }

    private void HandleDirectionToPlayer()
    {
        float rotationY = 0f;

        if(m_player)
        {
            if(m_player.position.x > transform.position.x)
            {
                rotationY = 180f;
                FlipRight = false;
            }
            else if(m_player.position.x < transform.position.x)
            {
                rotationY = 0f;
                FlipRight = true;
            }

            transform.rotation = new Quaternion(transform.rotation.x, rotationY, transform.rotation.z, transform.rotation.w);
        }
    }

    public void UpgradeSpeedBetweenTwoAttacks()
    {
        if (m_timeBetweenAttackIndex < m_basicAttacks.Length - 1)
        {
            m_timeBetweenAttackIndex++;
            m_timeBetweenAttack = m_timesBetweenAttack[m_timeBetweenAttackIndex];
        }
    }
}
