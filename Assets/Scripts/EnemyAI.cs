using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour{

    public enum State {
        Gathering,
        Building,
        DoNothing
    }

    private Player player;
    private EnemyBuild enemyBuild;
    private string tagForBrick;
    
    List<GameObject> bricks;
    [HideInInspector] public Animator animator;

    public State currentState;
    public bool isCollided;
    

    public float moveSpeedAI;
    [HideInInspector] public readonly float MaxMoveSpeedAI = 3.25f;

    private readonly float rotationFactorPerFrame = 10f;


    private void Awake() {
        player = GetComponent<Player>();
        tagForBrick = player.playerSO.TagCheckForStacking;
        animator = transform.Find("Model").GetComponent<Animator>();
        enemyBuild = GetComponent<EnemyBuild>();
    }

    private void Start() {
        currentState = State.Gathering;
        moveSpeedAI = MaxMoveSpeedAI;
    }

    private void Update() {
        switch (currentState) {
            case State.Gathering:
                FindAllCollectibles();
                GatherClosestBricks();
                animator.SetBool("isRunning", true);
                if (player.playerSO.stackAmount >= 5) {
                    currentState = State.Building;
                }
                break;
            case State.Building:
                animator.SetBool("isRunning", true);
          
                break;
            case State.DoNothing:
                animator.SetBool("isRunning", false);
                break;
        }
    }

    public void FindAllCollectibles() {
        bricks = new List<GameObject>(GameObject.FindGameObjectsWithTag(tagForBrick));
        bricks.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Collectible")));
    }


    private void GatherClosestBricks() { 
        float minDistance = Mathf.Infinity;
        Transform closestBrick = null;
        foreach (GameObject brickGameObject in bricks) {
            float tempDistance = Vector3.Distance(transform.position, brickGameObject.transform.position);
            if( tempDistance < minDistance) {
                minDistance = tempDistance;
                closestBrick = brickGameObject.transform;
            }
        }

        if(closestBrick == null) {
            currentState = State.DoNothing;
            return;
        }

        Vector3 tempVector = new(closestBrick.position.x, 0f, closestBrick.position.z);
        transform.position = Vector3.MoveTowards(transform.position, tempVector, moveSpeedAI * Time.deltaTime);
        Vector3 tempDirection = new Vector3(closestBrick.position.x - transform.position.x, 0f, closestBrick.position.z - transform.position.z).normalized; 
        HandleFacing(tempDirection);

    }

    public void HandleFacing(Vector3 direction) {
        Vector3 positionToLookAt = new(direction.x, 0f, direction.z);
        Quaternion currentRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
    }

}
