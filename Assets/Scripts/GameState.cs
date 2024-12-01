using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "gameState", menuName = "State/MyGameState")]
public class GameState : ScriptableObject
{
    public int coins;// { get; set; }
    public int coinsNeeded;
    public int currentCP = 1;
    public string[] gobNames = { "", "Neopolitan", "Blueberry", "Red Velvet", "Strawberry", "Orange", "Pumpkin", "Carrot", "Banana", "Chocolate", "Carolina Reaper" };
    public bool isBouncy;
    public bool isGlued;
    public bool canFly;
    public bool gameOver;


}
