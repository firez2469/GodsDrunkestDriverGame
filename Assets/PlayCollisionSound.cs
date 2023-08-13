using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayCollisionSound : MonoBehaviour
{
    public AudioSource deer;
    public AudioSource person;
    public AudioSource rabit;

    private void OnTriggerEnter(Collider other) {
        if (other.tag == "deer")
        {
            deer.Play();
        }
        if (other.tag == "rabbit")
        {
            rabit.Play();
        }
        if (other.tag == "person")
        {
            person.Play();
            // person.gameObject.SetActive(true);
        }
    }

    // private void OnTriggerExit(Collider other) {
        
    // }
}
