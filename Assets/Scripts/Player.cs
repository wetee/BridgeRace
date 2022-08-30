using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
    public PlayerScriptableObject playerSO;
    public GameObject Back;

    [HideInInspector] public int level = 0;

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



}
