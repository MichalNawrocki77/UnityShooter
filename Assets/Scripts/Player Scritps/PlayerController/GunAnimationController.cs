using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

public class AnimationsController : MonoBehaviour
{
	Animator animator;
	[SerializeField] MovementController movementProvider;
	private void Start()
	{
		animator = GetComponent<Animator>();
		if(animator is null)
		{
			Debug.Log("Gun Animator is null");
		}
	}
	private void Update()
	{
		UpdateSpeedParam();
	}
	void UpdateSpeedParam()
	{
		animator.SetFloat("Speed", Mathf.InverseLerp(0,movementProvider.maxSpeed, movementProvider.CurrentXZVelocity.magnitude));	
	}
}
