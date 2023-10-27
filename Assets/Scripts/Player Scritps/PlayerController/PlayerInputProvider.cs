using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.InputSystem.InputAction;

public class PlayerInputProvider : MonoBehaviour, IInputProvider
{
    public PlayerInputActions PlayerInputActions { get; private set; }

    public Vector2 MovementInput { get; private set; }

    public Vector2 CameraDeltaInput { get; private set; }
    public event Action OnJumpPressed;




    // Start is called before the first frame update
    void Awake()
    {
        PlayerInputActions = new PlayerInputActions();
        PlayerInputActions.PlayerMap.Enable();

        PlayerInputActions.PlayerMap.CameraMovementAction.performed += CameraMovementAction_performed;
        PlayerInputActions.PlayerMap.MovementAction.performed += MovementAction_performed;
        PlayerInputActions.PlayerMap.JumpAction.performed += JumpAction_performed;
    }

    private void JumpAction_performed(CallbackContext obj)
    {
        if(OnJumpPressed is null)
        {
            Debug.Log("Null at PlayerInputProvider.OnJumpPressed !!!");
            return;
        }
        OnJumpPressed();
    }

    private void MovementAction_performed(CallbackContext obj)
    {
        MovementInput = obj.ReadValue<Vector2>();
    }

    private void CameraMovementAction_performed(CallbackContext obj)
    {
        CameraDeltaInput = obj.ReadValue<Vector2>();
    }
}
