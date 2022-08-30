using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class PlayerTriggerActions : MonoBehaviour {

    private Player player;
    public event EventHandler OnBrickGathered;

    [SerializeField] private Transform parent;


    private void Awake() {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag(player.playerSO.TagCheckForStacking) || other.CompareTag("Collectible")) {
            player.IncrementAmount();
            Brick brickComponent = other.GetComponent<Brick>();
            brickComponent.order = player.playerSO.stackAmount;
            brickComponent.player = player;
            other.transform.Find("Model").GetComponent<Renderer>().material = player.playerSO.material;
            other.gameObject.tag = "InBack";
            if (TryGetComponent(out EnemyAI enemyAI)) {
                enemyAI.FindAllCollectibles();
            }
            OnBrickGathered?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnCollisionEnter(Collision collision) { 
        if (!collision.gameObject.CompareTag("EnemyAI")) return;

        Player collidedPlayer = collision.gameObject.GetComponent<Player>();

        if (player.Back.transform.childCount >= collidedPlayer.Back.transform.childCount) {
            collision.gameObject.GetComponent<EnemyAI>().animator.SetTrigger("isCollided");
            StartCoroutine(SpeedHandlerForEnemy(collision.gameObject.GetComponent<EnemyAI>()));
            collidedPlayer.playerSO.stackAmount = 0;
            AddForceAllBricks(collidedPlayer.Back.transform);
        }
        else {
            transform.Find("Model").GetComponent<Animator>().SetTrigger("isCollided");
            StartCoroutine(SpeedHandlerForPlayer(GetComponent<PlayerMovement>()));
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

            StartCoroutine(ColliderRBHandler(tempRigidbody, tempCollider));

            bricks[i].gameObject.tag = "Collectible";
            bricks[i].transform.SetParent(parent);
            Vector3 tempForce = new(Random.Range(-4f,4f),0f, Random.Range(0f, -4f));
            tempRigidbody.AddForce(tempForce, ForceMode.Impulse);
            bricks[i].transform.Find("Model").GetComponent<Renderer>().material = Resources.Load<Material>("CollectibleMaterial");
        }

    }

    private IEnumerator ColliderRBHandler(Rigidbody rigidbody, BoxCollider boxCollider) {
        rigidbody.isKinematic = false;
        boxCollider.isTrigger = false;
        yield return new WaitForSeconds(1f);
        rigidbody.isKinematic = true;
        boxCollider.isTrigger = true;
    }


    private IEnumerator SpeedHandlerForPlayer(PlayerMovement playerMovement) {
        playerMovement.moveSpeed = 0f;
        GetComponent<InputManager>().IsCollided = true;
        yield return new WaitForSeconds(2.85f);
        GetComponent<InputManager>().IsCollided = false;
        playerMovement.moveSpeed = playerMovement.MaxMoveSpeed;

    }

    private IEnumerator SpeedHandlerForEnemy(EnemyAI enemyAI) {
        enemyAI.moveSpeedAI = 0f;
        yield return new WaitForSeconds(3f);
        enemyAI.moveSpeedAI = enemyAI.MaxMoveSpeedAI;

    }
}
