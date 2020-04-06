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
    [SerializeField] private float m_distanceStartBattle;
    [SerializeField] private float m_distanceToNextNode;
#pragma warning restore 0649
    #endregion

    #region BossAttack
#pragma warning disable 0649
    [Header("Attacks")]
    [Space(5)]
    [SerializeField] private BossAttack m_startBattle;
    [SerializeField] private BossAttack[] m_basicAttacks; // Les 4 attaques de base
    [SerializeField] private BossAttack m_advancedAttack; // La 5ème attaque 
    [SerializeField] private BossAttack m_finalAttack; // La 6ème attaques
    [SerializeField] private float[] m_timesBetweenAttack;
#pragma warning restore 0649
    #endregion

    #region Variables
    private Transform m_player = null;
    private bool m_isEnraged = false;
    private BossAttack m_currentAttack = null;
    private float m_timeBetweenAttack;
    private int m_timeBetweenAttackIndex = 0;
    #endregion

    #region Getters / Setters
    public BossActionType CurrentState; /*{ get; private set; }*/
    #endregion

    private void Start()
    {
        CurrentState = BossActionType.Idle;
        m_player = PlayerManager.Instance.PlayerReference.transform;
        m_timeBetweenAttack = m_timesBetweenAttack[m_timeBetweenAttackIndex];
    }

    private void Update()
    {
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
            UpdateIABehaviour(BossActionType.Attacking);
        }
    }

    private void UpdateIABehaviour(BossActionType newState)
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
                StartCoroutine(HandleAttackingState(m_timeBetweenAttack));
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
         * Cri de guerre  
        */

        UpdateIABehaviour(BossActionType.Attacking);
    }

    private void HandleCounterAttackState()
    {

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

    private IEnumerator HandleAttackingState(float m_timeWait)
    {
        yield return new WaitForSeconds(m_timeWait);
        if(m_currentAttack == null)
        {
            m_currentAttack = m_basicAttacks[Random.Range(0, m_basicAttacks.Length)];
            m_currentAttack.StartAttack();
        }
    }

    [ContextMenu("Enraging State")]
    public void HandleEnragingState()
    {
        if(m_currentAttack)
        {
            m_currentAttack.CancelAttack();
        }

        CurrentState = BossActionType.Enraging;

        foreach(var attack in m_basicAttacks)
        {
            if(attack)
            {
                attack.UpgradeAttack();
            }
        }
    }

    private void HandleDyingState()
    {

    }

    private void Flip()
    {
        transform.rotation = new Quaternion(
            transform.rotation.x,
            transform.rotation.y == 0f ? 180f : 0f,
            transform.rotation.z,
            transform.rotation.w);
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
        if(m_player)
        {

        }
    }

    public void UpgradeSpeedAttack()
    {
        m_timeBetweenAttackIndex++;
        m_timeBetweenAttack = m_timesBetweenAttack[m_timeBetweenAttackIndex];
    }
}
