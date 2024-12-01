using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    [SerializeField]
    private GameState gameState;

    

    // Start is called before the first frame update
    void Start()
    {
        gameState.coins = 0;
        gameState.currentCP = 1;
        gameState.isBouncy= false;
        gameState.isGlued = false;
        gameState.canFly = false;
        gameState.gameOver= false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
