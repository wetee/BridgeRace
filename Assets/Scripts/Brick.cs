using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Brick : MonoBehaviour {

    public BrickScriptableObject brickSO;

    public Player player;


    public int order;
    
    private void Update() {
        if (player != null) {
            HandlePosition();
        }
    }

    private void HandlePosition() {
        transform.SetParent(player.Back.transform, false);
        transform.position = transform.parent.position + player.Offset * order;
        transform.rotation = Quaternion.Euler(transform.rotation.x, player.transform.rotation.eulerAngles.y - 180f, transform.rotation.z);
    }

}
