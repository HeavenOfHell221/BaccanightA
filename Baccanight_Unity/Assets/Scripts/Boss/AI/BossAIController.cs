using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIController : MonoBehaviour
{
    #region Inspector attributes
#pragma warning disable 0649
    [Header("Components")]
    [Space(5)]
    [SerializeField] private BossMovementControllerAir m_movementC;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Animator m_animator;
    [SerializeField] private PatrolPath m_patrolPath;
    [SerializeField] private HealthBoss m_health;

    [Header("Attributes")]
    [Space(5)]
    [SerializeField] [Range(15, 30)] private float m_distanceStartBattle;
    [SerializeField] [Range(0.01f, 1f)] private float m_distanceToNextNode;
#pragma warning restore 0649
    #endregion

    #region BossAttack
#pragma warning disable 0649
    [Header("Attacks")]
    [Space(5)]
    [SerializeField] private BossAttack m_startBattle;
    [SerializeField] private BossAttack[] m_basicAttacks; // Les 6 attaques de base (seulement 4 dispo au début du fight)
    [SerializeField] private BossAttack m_counterAttackAttack; // La contre-attaque pendant l'attaque "Shield"
    [SerializeField] [Range(1, 6)] private float[] m_timesBetweenAttack;
#pragma warning restore 0649
    #endregion

    #region Variables
    [Header("Variables")]
    [Space(5)]
    [SerializeField] private BossAttack m_currentAttack = null;
    private Transform m_player = null; // Transform du joueur
    private float m_timeBetweenAttack; // cooldown entre deux attaques
    private int m_timeBetweenAttackIndex = 0; // Index pour le tableau des cooldowns entre deux attaques
    private int m_basicAttackPossible = 4;
    #endregion

    #region Getters / Setters
    public BossActionType CurrentState { get; private set; } = BossActionType.Idle; 
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
        switch (CurrentState)
        {
            case BossActionType.Idle:
                if (DistanceFromPlayer() < m_distanceStartBattle)
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

    private IEnumerator NextAttack(float timeWait)
    {
        yield return new WaitForSeconds(timeWait);
        UpdateIABehaviour(BossActionType.Attacking);
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

        /* 
         * 
         * Animation : Cri de guerre  
         * 
         */

        UpdateIABehaviour(BossActionType.Attacking);
    }

    private void HandleCounterAttackState()
    {
        /*
        
        if(m_currentAttack)
        {
            m_currentAttack.CancelAttack();
        }
        m_currentAttack = m_counterAttackAttack;
        m_counterAttackAttack.StartAttack();
        
        */
    }

    private void HandleIdleState()
    {
        
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

    private void HandleAttackingState()
    { 
        if(m_currentAttack == null)
        {
            m_currentAttack = m_basicAttacks[Random.Range(0, m_basicAttackPossible)];
            m_currentAttack.StartAttack();
        }
    }

    private void HandleEnragingState()
    {
        if(m_currentAttack)
        {
            m_currentAttack.CancelAttack();
        }

        /*
         *
         * Enragement
         * 
         */

        for(int i = 0; i < m_basicAttackPossible; i++)
        {
            if(m_basicAttacks[i] && !m_basicAttacks[i].IsUpgraded)
            {
                m_basicAttacks[i].UpgradeAttack();
            }
        }

        m_basicAttackPossible++;
    }

    private void HandleDyingState()
    {

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
            }
            else if(m_player.position.x < transform.position.x)
            {
                rotationY = 0f;
            }

            transform.rotation = new Quaternion(transform.rotation.x, rotationY, transform.rotation.z, transform.rotation.w);
        }
    }

    public void UpgradeSpeedAttack()
    {
        if (m_timeBetweenAttackIndex < m_basicAttacks.Length)
        {
            m_timeBetweenAttackIndex++;
            m_timeBetweenAttack = m_timesBetweenAttack[m_timeBetweenAttackIndex];
        }
    }
}
