using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GobSpawnerScript : MonoBehaviour
{
    [SerializeField]
    public GameObject GobPrefab;
    public Vector3 position;
    [SerializeField]
    public float speed1;
    [SerializeField] 
    public float speed2;


    IEnumerator SpawnPointersUniformDist(float minTime, float maxTime)
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(minTime, maxTime);
            yield return new WaitForSeconds(waitTime);
            

            Instantiate(GobPrefab, position, Quaternion.identity);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnPointersUniformDist(speed1, speed2));
    }

    // Update is called once per frame
    void Update()
    {
        position = GameObject.FindGameObjectWithTag("GobSpawner").transform.position;
    }
}
