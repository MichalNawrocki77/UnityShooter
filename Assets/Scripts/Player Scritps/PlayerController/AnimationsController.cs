using System.Collections;
using System.Collections.Generic;

using UnityEditorInternal;

using UnityEngine;

public class AnimationsController : MonoBehaviour
{
	Animator animator;
	[SerializeField] Transform leftHandAnchor;
	private void Start()
	{
		animator = GetComponent<Animator>();
	}
	private void OnAnimatorIK(int layerIndex)
	{
		animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandAnchor.position);
		animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);

		animator.SetIKRotation(AvatarIKGoal.RightHand, leftHandAnchor.rotation);
		animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

	}
}
