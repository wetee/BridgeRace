using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTracker : MonoBehaviour{

    private Player player;
    public Transform stairChecker;

    [SerializeField] public LayerMask stairLayerMask;

    private void Awake() {
        player = GetComponent<Player>();
        stairChecker = transform.Find("StairChecker");
    }

    public bool CheckOneStepForward() {
        return Physics.Raycast(stairChecker.transform.position, Vector3.forward, maxDistance:0.35f, stairLayerMask);
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.gameObject.CompareTag("StairStep")) return;

        if (!other.gameObject.GetComponent<BoxCollider>().isTrigger) return;

        if (!CheckOneStepForward()) return;

        if (player.playerSO.stackAmount <= 0) return;

        other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
        other.gameObject.GetComponent<MeshRenderer>().enabled = true;
        other.gameObject.GetComponent<Renderer>().material = player.playerSO.material;
        other.gameObject.tag = player.playerSO.TagCheckForStacking;
        BrickSpawnSystem.Instance.DecreaseBrick(player.playerSO.TagCheckForStacking);
        player.DestroyLastBrick();
        player.DecreaseAmount();

    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("EnemyAI") || collision.gameObject.CompareTag("Player")) return;

        if(collision.gameObject.CompareTag(player.playerSO.TagCheckForStacking)) return;

        if (collision.gameObject.GetComponent<BoxCollider>().isTrigger) return;

        if (player.playerSO.stackAmount <= 0) return;
            
        collision.gameObject.GetComponent<Renderer>().material = player.playerSO.material;
        collision.gameObject.tag = player.playerSO.TagCheckForStacking;
        BrickSpawnSystem.Instance.DecreaseBrick(player.playerSO.TagCheckForStacking);
        player.DestroyLastBrick();
        player.DecreaseAmount();

    }

}
