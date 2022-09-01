using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Random = UnityEngine.Random;


public class PlayerTriggerActions : MonoBehaviour {

    private Player player;
    private Animator animator;
    [SerializeField] private Transform parent;



    private void Awake() {
        player = GetComponent<Player>();
        animator = transform.Find("Model").GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag(player.playerSO.TagCheckForStacking) || other.CompareTag("Collectible")) {
            player.IncrementAmount();
            Brick brickComponent = other.GetComponent<Brick>();
            brickComponent.order = player.playerSO.stackAmount;
            brickComponent.player = player;
            other.transform.Find("Model").GetComponent<Renderer>().material = player.playerSO.material;
            BrickSpawnSystem.Instance.DecreaseBrick(other.gameObject.tag);
            other.gameObject.tag = "InBack";

        }
    }

    private void OnCollisionEnter(Collision collision) { 
        if (!collision.gameObject.CompareTag("EnemyAI")) return;

        Player collidedPlayer = collision.gameObject.GetComponent<Player>();

        if (player.Back.transform.childCount >= collidedPlayer.Back.transform.childCount) {
            collision.transform.Find("Model").GetComponent<Animator>().SetTrigger("isCollided");
            EnemyAI enemyAI = collision.gameObject.GetComponent<EnemyAI>();

            if (!enemyAI.isCollided) {
                StartCoroutine(SpeedHandlerForEnemy(enemyAI));
            }
            
            collidedPlayer.playerSO.stackAmount = 0;
            AddForceAllBricks(collidedPlayer.Back.transform);
        }
        else {
            animator.SetTrigger("isCollided");

            if (TryGetComponent(out EnemyAI enemyAI)) {
                if (!enemyAI.isCollided) StartCoroutine(SpeedHandlerForEnemy(enemyAI));
            }
            else {
                InputManager tempInputManager = GetComponent<InputManager>();
                if (!tempInputManager.IsCollided) StartCoroutine(SpeedHandlerForPlayer(GetComponent<PlayerMovement>()));
                
            }
            player.playerSO.stackAmount = 0;
            AddForceAllBricks(player.Back.transform);
        }

    }

    private void AddForceAllBricks(Transform backTransform) {
        Brick[] bricks = backTransform.GetComponentsInChildren<Brick>();

        for(int i = 0; i < bricks.Length; i++) {
            bricks[i].player = null;
            bricks[i].order = 0;
            BoxCollider tempCollider = bricks[i].gameObject.GetComponent<BoxCollider>();
            Rigidbody tempRigidbody = bricks[i].gameObject.GetComponent<Rigidbody>();

            tempRigidbody.isKinematic = false;
            tempCollider.isTrigger = false;

            bricks[i].gameObject.tag = "Collectible";
            bricks[i].transform.SetParent(parent);
            Vector3 tempForce = new(Random.Range(-4f,4f),0f, Random.Range(0f, -4f));
            tempRigidbody.AddForce(tempForce, ForceMode.Impulse);
            bricks[i].transform.Find("Model").GetComponent<Renderer>().material = Resources.Load<Material>("CollectibleMaterial");
        }
    }

    private IEnumerator SpeedHandlerForPlayer(PlayerMovement playerMovement) {
        InputManager tempInputManager = GetComponent<InputManager>();
        playerMovement.moveSpeed = 0f;
        tempInputManager.IsCollided = true;
        yield return new WaitForSeconds(2.85f);
        tempInputManager.IsCollided = false;
        playerMovement.moveSpeed = playerMovement.MaxMoveSpeed;
    }

    private IEnumerator SpeedHandlerForEnemy(EnemyAI enemyAI) {
        enemyAI.moveSpeedAI = 0;
        enemyAI.isCollided = true;
        yield return new WaitForSeconds(3f);
        enemyAI.moveSpeedAI = enemyAI.MaxMoveSpeedAI;
        enemyAI.isCollided = false;
    }
}
