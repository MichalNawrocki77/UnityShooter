using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputProvider : MonoBehaviour
{
	public abstract Vector2 MovementInput{ get; protected set; }
	public abstract Vector2 CameraDeltaInput { get; protected set; }
	public event Action OnJumpProvided;
	protected virtual void InvokeJump()
	{
		this.OnJumpProvided?.Invoke();
	}
	//public abstract void SetJumpActionActive(bool isActive);
	public event Action OnShootProvided;
	protected virtual void InvokeShoot()
	{
		this.OnShootProvided?.Invoke();
	}
}
