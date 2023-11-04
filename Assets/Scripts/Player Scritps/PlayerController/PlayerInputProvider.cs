using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

public class PlayerInputProvider : BaseInputProvider
{
    private PlayerInputActions playerInputActions;

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

		
    }

	private void ShootAction_performed(CallbackContext obj)
	{
		InvokeShoot();
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
		InvokeJump();
	}
	public void SetJumpActionActive(bool isActive)
	{
		if (isActive)
		{
			playerInputActions.PlayerMap.JumpAction.Enable();
		}
		else if (!isActive)
		{
			playerInputActions.PlayerMap.JumpAction.Disable();
		}
	}
}
