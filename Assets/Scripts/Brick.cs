using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Brick : MonoBehaviour {

    public Player player;

    public int order;

    private Rigidbody _rigidbody;
    private BoxCollider boxCollider;

    private void Awake() {
        _rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Update() {
        if (player != null) {
            HandlePosition();
            CheckLevel();
        }
    }

    private void HandlePosition() {
        transform.SetParent(player.Back.transform, false);
        transform.position = transform.parent.position + player.Offset * order;
        transform.rotation = Quaternion.Euler(transform.rotation.x, player.transform.rotation.eulerAngles.y - 180f, transform.rotation.z);
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            _rigidbody.isKinematic = true;
            boxCollider.isTrigger = true;
        }
    }

    private void CheckLevel() {
        if (transform.position.y < player.level) Destroy(gameObject);  
    }

}
