using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;

using UnityEngine;

public class CameraController : MonoBehaviour
{
    IInputProvider input;
    

    [SerializeField] Transform VerticalCamera;
    [SerializeField] Transform HorizontalCamera;
    [SerializeField] private float Sensitivity;

    float currentHorizontalRotation = 0f;
    float currentVerticalRotation = 0f;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        input = GetComponent<IInputProvider>();
        if(input is null)
        {
            Debug.Log("CameraController Component Could not find IInputProvider. Please attach a component that inherits from IInputProvider interface :)");
        }
    }
    private void FixedUpdate()
    {
        HandleCameraInput();
    }
    void HandleCameraInput()
    {
        float horizontalIn = input.CameraDeltaInput.x;
        float verticalIn = input.CameraDeltaInput.y;

        currentHorizontalRotation += horizontalIn * Sensitivity * Time.fixedDeltaTime;
        currentVerticalRotation += verticalIn * Sensitivity * Time.fixedDeltaTime;

        currentVerticalRotation = Math.Clamp(currentVerticalRotation, -90f, 90f);

        HorizontalCamera.localRotation = Quaternion.Euler(0f,
                                                          currentHorizontalRotation,
                                                          0f);

        VerticalCamera.localRotation = Quaternion.Euler(currentVerticalRotation,
                                                        0f,
                                                        0f);
    }
}
