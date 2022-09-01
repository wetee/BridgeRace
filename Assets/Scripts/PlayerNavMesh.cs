using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;

public class PlayerNavMesh : MonoBehaviour {

    public enum State {
        Gathering,
        Building,
        DoNothing
    }

    private NavMeshAgent navMeshAgent;
    public Animator animator;
    
    private State currentState;

    public float moveSpeedAI;
    [HideInInspector] public readonly float MaxMoveSpeedAI = 3f;
    private readonly float rotationFactorPerFrame = 10f;

    [SerializeField] private Transform destination;


    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = transform.Find("Model").GetComponent<Animator>();
    }

    private void Start() {
        currentState = State.Gathering;
        navMeshAgent.speed = MaxMoveSpeedAI;
    }

    private void Update() {
        switch (currentState) {
            case State.Gathering:
                WanderAround(destination);
                animator.SetBool("isRunning", true);
                break;
            case State.Building:
                break;
            case State.DoNothing:
                animator.SetBool("isRunning", false);
                break;
        }
    }

    private void WanderAround(Transform destinationTransform) {
        float distanceBetweenPositions = Vector3.Distance(transform.position, destinationTransform.position);
        if (distanceBetweenPositions < 0.1f){
            destinationTransform.position = GetRandomDirection();
        }

        navMeshAgent.destination = destinationTransform.position;
        Vector3 tempDirection = new Vector3(destinationTransform.position.x - transform.position.x, 0f, destinationTransform.position.z - transform.position.z).normalized;
        HandleFacing(tempDirection);
    }

    private void HandleFacing(Vector3 direction) {
        Vector3 positionToLookAt = new(direction.x, 0f, direction.z);
        Quaternion currentRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
    }

    private Vector3 GetRandomDirection() {
        return new Vector3(Random.Range(-10.5f, 10.5f), transform.position.y, Random.Range(-6.5f,  6.5f));
    }
}
