using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseInputProvider : MonoBehaviour
{
	/// <summary>
	/// Base class for all InputProviders (I intend on having 2 InputProvider classes, one for the player and one for AI)
	/// </summary>
	public abstract Vector2 MovementInput{ get; protected set; }
	public abstract Vector2 CameraDeltaInput { get; protected set; }
	
	public event Action JumpProvided;
	protected virtual void InvokeJumpProvided()
	{
		this.JumpProvided?.Invoke();
	}
	//public abstract void SetJumpActionActive(bool isActive);
	
	public event Action ShootProvided;
	protected virtual void InvokeShootProvided()
	{
		this.ShootProvided?.Invoke();
	}
	
	public event Action ShootFinished;
	protected virtual void InvokeShootFinished()
	{
		this.ShootFinished?.Invoke();
	}

	public event Action ReloadProvided;
	protected virtual void InvokeReloadProvided()
	{
		this.ReloadProvided?.Invoke();
	}
}
