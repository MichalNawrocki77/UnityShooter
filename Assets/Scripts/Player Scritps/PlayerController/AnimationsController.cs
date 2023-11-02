using System.Collections;
using System.Collections.Generic;

using UnityEditorInternal;

using UnityEngine;

public class AnimationsController : MonoBehaviour
{
	Animator animator;
	[SerializeField] Transform leftHandAnchor;
	[SerializeField] Transform leftElbowAnchor;
	private void Start()
	{
		animator = GetComponent<Animator>();
	}
	private void OnAnimatorIK(int layerIndex)
	{
		animator.SetIKHintPosition(AvatarIKHint.LeftElbow, leftElbowAnchor.position);
		animator.SetIKHintPositionWeight(AvatarIKHint.LeftElbow, 1);

		animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandAnchor.position);
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
	}
}
