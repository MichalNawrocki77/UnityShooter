using System;
using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;

using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

public class PlayerInputProvider : BaseInputProvider
{
    public PlayerInputActions playerInputActions { get; private set; }

    public override Vector2 MovementInput { get; protected set; }

	public override Vector2 CameraDeltaInput { get; protected set; }

	void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.PlayerMap.Enable();

        playerInputActions.PlayerMap.CameraMovementAction.performed += CameraMovementAction_performed;
        playerInputActions.PlayerMap.MovementAction.performed += MovementAction_performed;
        playerInputActions.PlayerMap.JumpAction.performed += JumpAction_performed;
		playerInputActions.PlayerMap.ShootAction.performed += ShootAction_performed;
		//playerInputActions.PlayerMap.ShootAction.started += DebugShootAction_started;
		playerInputActions.PlayerMap.ShootAction.canceled += ShootAction_canceled;
		playerInputActions.PlayerMap.ReloadAction.performed += ReloadAction_performed;

	}

	//private void DebugShootAction_started(CallbackContext obj)
	//{
	//	Debug.Log("ShootActionStarted()");
	//}

	private void ReloadAction_performed(CallbackContext obj)
	{
		InvokeReloadProvided();
	}

	private void ShootAction_performed(CallbackContext obj)
	{
		InvokeShootProvided();
		Debug.Log("ShootActionPerformed()");
	}
	private void ShootAction_canceled(CallbackContext obj)
	{
		InvokeShootFinished();
		Debug.Log("ShootActionCanceled()");
	}

	private void MovementAction_performed(CallbackContext obj)
    {
        MovementInput = obj.ReadValue<Vector2>();
    }

    private void CameraMovementAction_performed(CallbackContext obj)
    {
        CameraDeltaInput = obj.ReadValue<Vector2>();
    }
	private void JumpAction_performed(CallbackContext obj)
	{
		InvokeJumpProvided();
	}

	//public override void SetJumpActionActive(bool isActive)
	//{
	//	if (isActive)
	//	{
	//		playerInputActions.PlayerMap.JumpAction.Enable();
	//	}
	//	else if (!isActive)
	//	{
	//		playerInputActions.PlayerMap.JumpAction.Disable();
	//	}
	//}
}
