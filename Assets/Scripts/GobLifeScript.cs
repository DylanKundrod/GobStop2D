using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobLifeScript : MonoBehaviour
{
    int lifeTime = 8;

    IEnumerator LifeSpan()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LifeSpan());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
