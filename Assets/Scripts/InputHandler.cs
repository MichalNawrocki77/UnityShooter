using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler
{
    Player player;
    Rigidbody rb;

    float rotationX;
    float rotationY;

    Vector2 cameraInput;
    Vector2 playerMovementVector2;
    public InputHandler(Player player, Rigidbody rb)
    {
        this.player = player;
        this.rb = rb;
    }

    


    #region Camera
    public void CameraMovementAction_performed(InputAction.CallbackContext obj)
    {
        cameraInput = obj.ReadValue<Vector2>();
    }
    public void CameraRotation()
    {

        float mouseX = cameraInput.x * Time.deltaTime * player.mouseSensitivity;
        float mouseY = cameraInput.y * Time.deltaTime * player.mouseSensitivity;

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
        rb.AddForce(player.transform.forward * playerMovementVector2.y * player.speed, ForceMode.Acceleration);
        rb.AddForce(player.transform.right * playerMovementVector2.x * player.speed, ForceMode.Acceleration);
    }
    #endregion

    #region Jump
    public void JumpAction_performed(InputAction.CallbackContext obj)
    {
        Debug.Log("JumpAction_performed() called");
        rb.AddForce(Vector3.up * 4, ForceMode.Impulse);
    }
    #endregion
}
