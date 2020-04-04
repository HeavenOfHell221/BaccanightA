﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFly : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = PlayerManager.Instance.PlayerReference;
        player.GetComponent<Rigidbody2D>().gravityScale = 0;
        player.GetComponent<PlayerMovementControllerAir>().enabled = true;
        player.GetComponent<PlayerMovementControllerGround>().enabled = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject player = PlayerManager.Instance.PlayerReference;
        player.GetComponent<Rigidbody2D>().gravityScale = 0.8f;
        player.GetComponent<PlayerMovementControllerAir>().enabled = false;
        player.GetComponent<PlayerMovementControllerGround>().enabled = true;
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
