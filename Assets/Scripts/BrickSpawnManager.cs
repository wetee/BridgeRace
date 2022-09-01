using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;

public class BrickSpawnManager : MonoBehaviour {

    private Player player;
    private PlayerTriggerActions playerTriggerActions;
    private int bound = 10;

    [SerializeField] private GameObject parent;

    [SerializeField] private Brick brickPrefab;

    private void Awake() {
        player = GetComponent<Player>();
        playerTriggerActions = GetComponent<PlayerTriggerActions>();
    }

    private void Start() {
        InstantiateBricks();
        //playerTriggerActions.OnBrickGathered += PlayerTriggerActions_OnBrickGathered;
    }

    private void PlayerTriggerActions_OnBrickGathered(object sender, System.EventArgs e) {
        if(player.playerSO.stackAmount % 10 == 0) {
            InstantiateBricks();
            //Vector3 tempPos = new Vector3(Random.Range(-8, 8), player.level + 0.3f, Random.Range(-8, 8));
            //Instantiate(brickPrefab.gameObject, tempPos, Quaternion.identity, parent.transform);
        }
    }


    private void InstantiateBricks() {
        for (int i = 0; i < bound; i++) {
            Vector3 tempPos = new Vector3(Random.Range(-8, 8), player.level + 0.3f, Random.Range(-8, 8));
            Instantiate(brickPrefab.gameObject, tempPos, Quaternion.identity, parent.transform);
        }
    }

    

    

}
