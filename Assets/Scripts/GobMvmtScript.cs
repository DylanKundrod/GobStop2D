using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobMvmtScript : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(7, 8);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((-8.5f * Time.deltaTime), 0, 0);
        
    }

    
}
