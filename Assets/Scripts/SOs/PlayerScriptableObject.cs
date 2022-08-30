using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player", menuName = "ScriptableObjects/PlayerType")]
public class PlayerScriptableObject : ScriptableObject{

    public string TagCheckForStacking;
    public int stackAmount;
    public Material material;



}
