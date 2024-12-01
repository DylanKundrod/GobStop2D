using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Finish : MonoBehaviour
{
    private AudioSource finishSound;
    [SerializeField]
    private GameState gameState;

    // Start is called before the first frame update
    void Start()
    {
        finishSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            finishSound.Play();
            gameState.coins = 0;
            gameState.gameOver = true;
            StartCoroutine(Wait());
            CompleteLevel();
        }
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(5);
        gameState.gameOver = false;
    }
    private void CompleteLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

}
