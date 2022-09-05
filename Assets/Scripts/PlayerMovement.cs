using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour{

    private InputManager inputManager;

    [HideInInspector] public float moveSpeed;
    [HideInInspector] public readonly float MaxMoveSpeed = 2.5f;


    private void Awake() {
        inputManager = GetComponent<InputManager>();
    }

    private void Start() {
        moveSpeed = MaxMoveSpeed;
    }

    private void Update() {
        HandleMovement();
    }

    private void HandleMovement() {
        Vector3 tempDirection = new Vector3(inputManager.MoveInput.x, 0f, inputManager.MoveInput.y);

        transform.position += moveSpeed * Time.deltaTime * tempDirection;
    }



}
