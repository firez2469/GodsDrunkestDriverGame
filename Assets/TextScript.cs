using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{

    public float lastsFor = 2f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lastsFor);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
