using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTriggerActions : MonoBehaviour{
    private Player player;

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other) {

        if (other.CompareTag(player.playerSO.TagCheckForStacking)) {
            player.IncrementAmount();
            other.GetComponent<Brick>().order = player.playerSO.stackAmount;
            other.GetComponent<Brick>().player = player;
            other.gameObject.tag = "Collectible";
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) return;

        if (!collision.gameObject.CompareTag("EnemyAI")) return;

        Player collidedPlayer = collision.gameObject.GetComponent<Player>();

        if (player.Back.transform.childCount >= collidedPlayer.Back.transform.childCount) {
            collision.gameObject.GetComponent<EnemyAI>().animator.SetTrigger("isCollided");
            StartCoroutine(SpeedHandlerForEnemy(collision.gameObject.GetComponent<EnemyAI>()));
            collidedPlayer.playerSO.stackAmount = 0;
            //UtilsClass.AddForceAllBricks(collidedPlayer.Back.transform);
        }
        else {
            transform.Find("Model").GetComponent<Animator>().SetTrigger("isCollided");
            StartCoroutine(SpeedHandlerForPlayer(GetComponent<PlayerMovement>()));
            player.playerSO.stackAmount = 0;
            //UtilsClass.AddForceAllBricks(player.Back.transform);
        }

    }

    IEnumerator SpeedHandlerForPlayer(PlayerMovement playerMovement) {
        playerMovement.moveSpeed = 0f;
        GetComponent<InputManager>().IsCollided = true;
        yield return new WaitForSeconds(2.85f);
        GetComponent<InputManager>().IsCollided = false;
        playerMovement.moveSpeed = playerMovement.MaxMoveSpeed;

    }

    IEnumerator SpeedHandlerForEnemy(EnemyAI enemyAI) {
        enemyAI.moveSpeedAI = 0f;
        yield return new WaitForSeconds(2.85f);
        enemyAI.moveSpeedAI = enemyAI.MaxMoveSpeedAI;

    }
}
