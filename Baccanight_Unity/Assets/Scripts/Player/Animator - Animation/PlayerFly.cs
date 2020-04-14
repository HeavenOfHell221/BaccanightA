using UnityEngine;

public class PlayerFly : StateMachineBehaviour
{
    private float m_gravityScale;
    private PlayerMovementControllerAir m_playerMovementControllerAir;
    private PlayerMovementControllerGround m_playerMovementControllerGround;
    private Rigidbody2D m_rigidbody2D;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_rigidbody2D = PlayerManager.Instance.PlayerReference.GetComponent<Rigidbody2D>();
        m_playerMovementControllerAir = PlayerManager.Instance.PlayerReference.GetComponent<PlayerMovementControllerAir>();
        m_playerMovementControllerGround = PlayerManager.Instance.PlayerReference.GetComponent<PlayerMovementControllerGround>();

        m_gravityScale = m_rigidbody2D.gravityScale;
        m_rigidbody2D.gravityScale = 0;
        m_playerMovementControllerAir.enabled = true;
        m_playerMovementControllerGround.enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_rigidbody2D.gravityScale = m_gravityScale;
        m_playerMovementControllerAir.enabled = false;
        m_playerMovementControllerGround.enabled = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
