using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuild : MonoBehaviour
{
    private enum BuildStates {
        GoEntrance,
        GoBuilding
    }

    private Player player;
    private EnemyAI enemyAI;

    private Transform stairTransform;

    private Transform entrance;
    private Transform firstStepTransform;
    private Transform target;

    public bool IsBuildingPhase = false;
    private BuildStates currentBuildState;

    private float rotationFactorPerFrame = 10f;
    private void Awake() {
        player = GetComponent<Player>();
        enemyAI = GetComponent<EnemyAI>();

    }

    private void Start() {
        currentBuildState = BuildStates.GoEntrance;
    }

    private void Update() {
        IsBuildingPhase = enemyAI.currentState == EnemyAI.State.Building;

        if (!IsBuildingPhase) return;
        Debug.Log(currentBuildState.ToString());

        ClosestStairCheck();
        entrance = stairTransform.Find("TheEntrance").transform;
        target = stairTransform.Find("TheTarget").transform;
        firstStepTransform = transform.Find("TheFirstStep");

        switch (currentBuildState) {
            case BuildStates.GoEntrance:
                GoEntrance();
                break;
            case BuildStates.GoBuilding:
                GoBuilding();
                break;
        }
        
    }


    private void ClosestStairCheck() {
        GameObject[] stairs = GameObject.FindGameObjectsWithTag("Stair");

        float minDistance = Mathf.Infinity;
        Transform closestStair = null;
        foreach (GameObject stairGameobject in stairs) {
            float tempDistance = Vector3.Distance(transform.position, stairGameobject.transform.position);
            if (tempDistance < minDistance) {
                minDistance = tempDistance;
                closestStair = stairGameobject.transform;
            }
        }

        if (closestStair != null) stairTransform = closestStair.transform;
    }

    private void GoBuilding() {
        if(player.playerSO.stackAmount <= 0) {
            currentBuildState = BuildStates.GoEntrance;
            return;
        }

        if (Vector3.Distance(transform.position, target.position) < 0.05f) {
            target.position += new Vector3(0f, 0.2f, 0.5f);
        }

        transform.position = Vector3.MoveTowards(transform.position, target.position, enemyAI.MaxMoveSpeedAI * Time.deltaTime);
        Vector3 tempDirection = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, target.position.z - transform.position.z).normalized;

        HandleFacing(tempDirection);
    }

    public void GoEntrance() {
        transform.position = Vector3.MoveTowards(transform.position, entrance.position, enemyAI.MaxMoveSpeedAI * Time.deltaTime);
        Vector3 tempDirection = new Vector3(entrance.position.x - transform.position.x, target.position.y - transform.position.y, entrance.position.z - transform.position.z).normalized;
        HandleFacing(tempDirection);
        if (Vector3.Distance(transform.position, entrance.position) < 0.2f){
            if (player.playerSO.stackAmount >= 5) {
                currentBuildState = BuildStates.GoBuilding;
            }
            else if(player.playerSO.stackAmount == 0) {
                transform.position += Vector3.back * 5;
                target.position = firstStepTransform.position + new Vector3(0f, 0.2f, 0f);
                enemyAI.currentState = EnemyAI.State.Gathering;
            }

        }

    }

    public void HandleFacing(Vector3 direction) {
        Vector3 positionToLookAt = new(direction.x, 0f, direction.z);
        Quaternion currentRotation = transform.rotation;

        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
    }



}
