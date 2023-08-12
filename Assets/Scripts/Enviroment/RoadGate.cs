using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this may be a bit goofy but oh well
public class RoadGate : MonoBehaviour
{

    Road parent;

    void Awake()
    {
        parent = GetComponentInParent<Road>();
    }


    void OnTriggerEnter(Collider other) {
        Debug.Log("COLLLIDDDEEE WITH " + other.gameObject.name);
        if(other.gameObject.name == "player") {
            parent.roadTrigger();
            Destroy(gameObject);
        }
    }
}
