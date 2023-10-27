using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInputProvider
{
    public Vector2 MovementInput{ get; }
    public Vector2 CameraDeltaInput { get; }
    public event Action OnJumpPressed;
}
