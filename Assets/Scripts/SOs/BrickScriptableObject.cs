using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Brick", menuName = "ScriptableObjects/Brick")]
public class BrickScriptableObject : ScriptableObject {

    public GameObject brick;
    public string tag;
}
