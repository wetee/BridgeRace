using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickSpawnSystem : MonoBehaviour
{

    public static BrickSpawnSystem Instance;

    [SerializeField] Transform parent;
    [SerializeField] LayerMask brickLayer;
    [SerializeField] private string[] tags;
    [SerializeField] private Brick[] bricks;
    [SerializeField] private Player[] players;

    private Dictionary<string, int> brickPairs = new Dictionary<string, int>();
    private readonly int tempBound = 16;

    private void Awake() {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    private void Start() {
        InitalizeTable();
        InvokeRepeating(nameof(InstantiateBricks), 0f, 5f);
    }


    private void InitalizeTable() {
        foreach(string tag in tags) {
            brickPairs.Add(tag, 0);
        }
      
    }

    private void DebugAmounts() {
        foreach (KeyValuePair<string, int> entry in brickPairs) {
            if (entry.Key.Equals("YellowBrick")) {
                Debug.Log(entry.Value);
            }
        }
    }

    private void InstantiateBricks() {
        for(int x = -10; x < 11; x += 2) {
            for(int z = -6; z < 7; z += 2) {
                if (!isPositionEmpty(x, z)) continue;

                Brick tempBrick = PickRandomBrick();
                if (tempBrick != null) {
                    int tempLevel = LevelFinder(tempBrick);
                    Instantiate(tempBrick, new Vector3(x, tempLevel + 0.15f, z), Quaternion.identity, parent);
                }
            }
        }
    }

    private Brick PickRandomBrick() {
        int tempInt = Random.Range(0, 4);
        Brick tempBrick = bricks[tempInt];

        if(tempInt == 0 & brickPairs[tempBrick.tag] <= tempBound) {
            brickPairs[tempBrick.tag]++;
            return bricks[0];
        }
        else if(tempInt == 1 & brickPairs[tempBrick.tag] <= tempBound) {
            brickPairs[tempBrick.tag]++;
            return bricks[1];
        }
        else if (tempInt == 2 & brickPairs[tempBrick.tag] <= tempBound) {
            brickPairs[tempBrick.tag]++;
            return bricks[2];
        }
        else if (tempInt == 3 & brickPairs[tempBrick.tag] <= tempBound) {
            brickPairs[tempBrick.tag]++;
            return bricks[3];
        }

        return null;

    }
    
    public void DecreaseBrick(string tag) {
        foreach (string brickTag in tags) {
            if (brickTag.Equals(tag) ){
                brickPairs[brickTag]--;
            }
        }
    }

    public bool isPositionEmpty(int x, int z) {
        Vector3 tempPos = new(x, 0.12f, z);

        return !Physics.CheckSphere(tempPos, .75f, brickLayer); 
    }
    
    public int LevelFinder(Brick brick) {
        foreach(Player player in players) {
            if (player.playerSO.TagCheckForStacking.Equals(brick.gameObject.tag)) {
                //if (player.playerSO.TagCheckForStacking.Equals("YellowBrick")) Debug.Log(player.level);
                return player.level;
            }
        }
        return 0;
    }


}
