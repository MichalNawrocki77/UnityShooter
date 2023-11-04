using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputProvider : MonoBehaviour
{
	public abstract Vector2 MovementInput{ get; protected set; }
	public abstract Vector2 CameraDeltaInput { get; protected set; }
	public event Action JumpProvided;
	protected virtual void InvokeJump()
	{
		this.JumpProvided?.Invoke();
	}
	//public abstract void SetJumpActionActive(bool isActive);
	public event Action ShootProvided;
	protected virtual void InvokeShoot()
	{
		this.ShootProvided?.Invoke();
	}
	public event Action ShootFinished;
	protected virtual void InvokeShootFinished()
	{
		this.ShootFinished?.Invoke();
	}
}
