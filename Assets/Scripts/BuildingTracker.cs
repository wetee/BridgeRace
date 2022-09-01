using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingTracker : MonoBehaviour{
    private Player player;
    private Transform stairChecker;

    [SerializeField] LayerMask stairLayerMask;

    private void Awake() {
        player = GetComponent<Player>();
        stairChecker = transform.Find("StairChecker");
    }

    private bool CheckOneStepForward() {
        return Physics.Raycast(stairChecker.transform.position, Vector3.forward, maxDistance:0.35f, stairLayerMask);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("StairStep")) {
            if (CheckOneStepForward()) {
                if (player.playerSO.stackAmount <= 0) return;

                other.gameObject.GetComponent<BoxCollider>().isTrigger = false;
                other.gameObject.GetComponent<MeshRenderer>().enabled = true;
                other.gameObject.GetComponent<Renderer>().material = player.playerSO.material;
                BrickSpawnSystem.Instance.DecreaseBrick(player.playerSO.TagCheckForStacking);
                player.DestroyLastBrick();
                player.DecreaseAmount();

            }
        }
    }



}
