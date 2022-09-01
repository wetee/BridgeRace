using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public PlayerScriptableObject playerSO;
    public GameObject Back;

    public int level;

    public readonly Vector3 Offset = new Vector3(0f, 0.25f, 0f);

    private void Awake() {
        playerSO.stackAmount = 0;
    }


    public void IncrementAmount() {
        playerSO.stackAmount++;
    }

    public void ChangeAmountBy(int amount) {
        playerSO.stackAmount += amount;
    }

    public void DestroyLastBrick() {
        Brick[] bricksTemp = GetComponentsInChildren<Brick>();

        float maxDistance = 0f;
        Brick brickTemp = null;
        foreach (Brick brick in bricksTemp) {
            if (brick.transform.position.y > maxDistance) {
                maxDistance = transform.position.y;
                brickTemp = brick;
            }
        }

        if (brickTemp != null) Destroy(brickTemp.gameObject); 
            
    }

    public void DecreaseAmount() {
        if (playerSO.stackAmount <= 0) return;

        playerSO.stackAmount--;
    }



}
