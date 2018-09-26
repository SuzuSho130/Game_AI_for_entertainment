using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckingAttack : StateMachineBehaviour {

	PlayerController pc;
	public int actionNumber;
	public float startMotionTime;
	public float motionTime;
	private bool player;

	public void SetPlayer(bool p) {
		player = p;
		pc = FindObjectOfType<PlayerController>();
	}

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (player) {
			pc.StartCoroutine(pc.AttackStart(actionNumber, startMotionTime, motionTime));
		}
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if (player) {
			pc.AttackEnd (actionNumber);
		}
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
