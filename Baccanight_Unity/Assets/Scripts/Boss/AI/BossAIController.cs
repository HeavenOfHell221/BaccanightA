﻿using System.Collections;
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
    #endregion

    #region Getters / Setters
    public BossActionType CurrentState { get => m_currentState; private set => m_currentState = value; }  
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
                if (distanceFromPlayer > m_distanceStartBattle)
                {
                    m_health.IsInvincible = true;
                    UpdateIABehaviour(BossActionType.Idle);
                }
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
        if(!m_battleHasStart)
        {
            m_battleHasStart = true;

           /* 
            * Animation : Cri de guerre  
            */
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
        // Cancel de l'attaque en cours
        if(m_currentAttack)
        {
            m_currentAttack.CancelAttack();
            m_currentAttack = null;
        }

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
        while(m_health.IsInvincible)
        {
            yield return null;
        }

        StartCoroutine(NextAttack(m_timeBetweenAttack));
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

    public void UpgradeSpeedBetweenTwoAttacks()
    {
        if (m_timeBetweenAttackIndex < m_basicAttacks.Length)
        {
            m_timeBetweenAttackIndex++;
            m_timeBetweenAttack = m_timesBetweenAttack[m_timeBetweenAttackIndex];
        }
    }
}
