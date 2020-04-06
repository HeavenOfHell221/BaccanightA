using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIController : MonoBehaviour
{
    #region Inspector attributes
#pragma warning disable 0649
    [Header("General")]
    [SerializeField] private Transform m_transformToFlip;
    [SerializeField] private MovementController m_movementController;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Animator m_animator;
    [SerializeField] private float m_distanceStartBattle;
#pragma warning restore 0649
    #endregion

    #region BossAttack
#pragma warning disable 0649
    [SerializeField] private BossAttack m_startBattle;
    [SerializeField] private BossAttack[] m_basicAttacks; // Les 4 attaques de base
    [SerializeField] private BossAttack m_advancedAttack; // La 5ème attaque 
    [SerializeField] private BossAttack m_finalAttack; // La 6ème attaques
#pragma warning restore 0649
    #endregion

    #region Variables
    private Transform m_player;
    #endregion

    #region Getters / Setters
    public BossActionType CurrentState { get; private set; }
    #endregion

    private void Start()
    {
        CurrentState = BossActionType.Idle;
        m_player = PlayerManager.Instance.PlayerReference.transform;
    }

    private void Update()
    {
        UpdateIABehaviour();
    }

    private void UpdateIABehaviour()
    {
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
        }
    }

    private void HandleStartBattleState()
    {
        /* Cri de guerre, puis le combat commence */
    }

    private void HandleCounterAttackState()
    {

    }

    private void HandleIdleState()
    {
        if(DistanceFromPlayer() < m_distanceStartBattle)
        {
            CurrentState = BossActionType.StartBattle;
        }
    }

    private void HandleMovingState()
    {

    }

    private void HandleStuningState()
    {

    }

    private void HandleAttackingState()
    {

    }

    private void HandleEnragingState()
    {

    }

    private void HandleDyingState()
    {

    }

    private void Flip()
    {
        m_transformToFlip.rotation = new Quaternion(
            m_transformToFlip.rotation.x,
            m_transformToFlip.rotation.y == 0f ? 180f : 0f,
            m_transformToFlip.rotation.z,
            m_transformToFlip.rotation.w);
    }
        

    private float DistanceFromPlayer()
    {
        return Vector2.Distance(transform.position, m_player.position);
    }



}
