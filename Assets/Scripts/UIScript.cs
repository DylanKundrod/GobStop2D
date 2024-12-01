using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField]
    GameState gameState;
    [SerializeField]
    public TextMeshProUGUI goldCointsText;
    [SerializeField]
    public TextMeshProUGUI endLevelText;
    // Start is called before the first frame update
    void Start()
    {
        endLevelText.gameObject.SetActive(false);
    }

    IEnumerator DisplayEndGameMessage()
    {
        endLevelText.gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        endLevelText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        goldCointsText.text = (gameState.gobNames[gameState.currentCP] + ": " + gameState.coins.ToString() + "/"+ gameState.currentCP.ToString());
        if (gameState.gameOver)
        {
            StartCoroutine(DisplayEndGameMessage());
        }
    }
}
