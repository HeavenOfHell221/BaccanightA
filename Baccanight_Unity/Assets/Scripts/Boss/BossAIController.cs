using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAIController : MonoBehaviour
{
    #region Inspector
#pragma warning disable 0649
    [SerializeField] private Transform m_transformToFlip;
    [SerializeField] private MovementController m_movementController;
    [SerializeField] private Rigidbody2D m_rigidbody;
    [SerializeField] private Animator m_animator;
#pragma warning restore 0649
    #endregion

    #region Variables
    #endregion

    #region Getters / Setters
    public BossActionType CurrentState { get; private set; } = BossActionType.Idle;
    #endregion

    private void Start()
    {

    }

    private void Update()
    {
        UpdateStates();
        UpdateIABehaviour();
    }

    
    private void UpdateStates()
    {

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
        }
    }

    private void HandleIdleState()
    {

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
        
}
