using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHandler : MonoBehaviour{

    private Player player;
    public event EventHandler OnLevelChanged;
    

    private void Awake() {
        player = GetComponent<Player>();
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            player.level = (int)collision.transform.position.y;
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
