using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour{

    [SerializeField] private Joystick stick;

    public Vector3 MoveInput { get; private set; }
    public bool IsRunning { get; private set; }

     public bool IsCollided;

    private void Awake() {
        HandleStickSettings();
    }

    private void Update() {

        if (IsCollided) return;

        MoveInput = stick.Direction;

        IsRunningCheck();
    }

    private void HandleStickSettings() {
        stick.DeadZone = 0.9f;
        stick.HandleRange = 0.1f;
    }

    private void IsRunningCheck() {
        IsRunning = MoveInput.x != 0 || MoveInput.y != 0;
    }


}
