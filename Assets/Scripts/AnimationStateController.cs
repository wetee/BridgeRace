using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    private InputManager inputManager;
    private Animator animator;

    [SerializeField] private float rotationFactorPerFrame;

    private void Awake() {
        inputManager = GetComponent<InputManager>();
        animator = transform.Find("Model").GetComponent<Animator>();
    }


    private void Update() {
        HandleStates();
        HandleFacing();
    }


    private void HandleStates() {
        if (inputManager.IsRunning) animator.SetBool("isRunning", true);
        else animator.SetBool("isRunning", false);
    }

    private void HandleFacing() {
        Vector3 positionToLookAt = new(inputManager.MoveInput.x, 0f, inputManager.MoveInput.y);
        Quaternion currentRotation = transform.rotation;

        if (!inputManager.IsRunning) return;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
    }

}
