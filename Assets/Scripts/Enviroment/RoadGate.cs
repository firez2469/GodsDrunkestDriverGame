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
        
        //if other is player:
        //alert parent
    }
}
