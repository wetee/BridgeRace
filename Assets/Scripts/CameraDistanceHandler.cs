using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraDistanceHandler : MonoBehaviour{

    [SerializeField] CinemachineVirtualCamera virtualCamera;
    CinemachineFramingTransposer CFT;

    [SerializeField] private float sensivity;

    private readonly float baseDistance = 10f;

    private Player player;

    private void Awake() {
        CFT = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        player = GetComponent<Player>();
    }

    private void Start() {
        CFT.m_CameraDistance = baseDistance;    
    }

    private void Update() {
        HandleCameraDistance();
    }

    private void HandleCameraDistance() {
        if(player.playerSO.stackAmount > 10) {
            CFT.m_CameraDistance = baseDistance + (player.playerSO.stackAmount - 10) * sensivity;
        }
        else if (player.playerSO.stackAmount > 30) {
            CFT.m_CameraDistance = 17f;
        }
    }

}
