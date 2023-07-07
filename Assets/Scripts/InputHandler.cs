using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler
{
    Player player;

    float rotationX;
    float rotationY;

    Vector3 movementVector;

    Vector2 cameraInput;
    Vector2 playerMovementVector2;
    public InputHandler(Player player)
    {
        this.player = player;
    }

    #region Camera
    public void CameraMovementAction_performed(InputAction.CallbackContext obj)
    {
        cameraInput = obj.ReadValue<Vector2>();
    }
    public void CameraRotation()
    {

        float mouseX = cameraInput.x * Time.fixedDeltaTime * player.mouseSensitivity;
        float mouseY = cameraInput.y * Time.fixedDeltaTime * player.mouseSensitivity;

        rotationX += mouseY;
        rotationY += mouseX;

        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        player.transform.rotation = Quaternion.Euler(0f, rotationY, 0f);
        player.cameraHolder.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
    #endregion

    #region Movement
    public void MovementAction_performed(InputAction.CallbackContext obj)
    {
        playerMovementVector2 = obj.ReadValue<Vector2>();
    }
    public void PlayerMovement()
    {
        if (player.isGrounded) 
        {
            movementVector = player.transform.forward * playerMovementVector2.y + player.transform.right * playerMovementVector2.x;
            movementVector *= player.speed * player.speedMultiplier;
        }
        else
        {
            movementVector = player.transform.forward * playerMovementVector2.y + player.transform.right * playerMovementVector2.x;
            movementVector *= player.speed * player.speedMultiplier * player.inAirSpeedMultiplier;
        }


        player.rb.AddForce(movementVector, ForceMode.Force);
    }
    #endregion

    #region Jump
    public void JumpAction_performed(InputAction.CallbackContext obj)
    {
        if (player.isGrounded)
        {
            player.rb.AddForce(Vector3.up * player.jumpForce, ForceMode.Impulse);
        }
    }
    #endregion
}
