using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles behavior of each road segment (spawning AI, detecting when player has left the segment, swaping sprites)
public class Road : MonoBehaviour
{
    Vector3 corner;
    float spawnXRange;
    float spawnZRange;

    public GameObject humanPrefab;
    public GameObject rabbitPrefab;
    public GameObject deerPrefab;

    public GameObject AIContainer;

    void Start()
    {
        corner = new Vector3(transform.position.x - transform.localScale.x/2,
                 transform.position.y + transform.localScale.y/2,
                 transform.position.z - transform.localScale.z/2);
        spawnXRange = transform.localScale.x;
        spawnZRange = transform.localScale.z;
        AIContainer.transform.position = corner;
        Debug.Log(corner);
        Debug.Log(AIContainer.transform.position);
        Debug.Log(spawnXRange);
        spawnAI();
    }

    //for now will be hardcoded to one of each
    public void spawnAI() {
        List<Vector3> spawns = new List<Vector3>();

        for(int i = 0; i < 3; i++) {
            bool foundValid = false;
            while(!foundValid) { // this is a bit silly D:
                Vector3 spawnLocation = new Vector3(Random.Range(corner.x, corner.x+spawnXRange), corner.y, Random.Range(corner.z, corner.z+spawnZRange));
                foundValid = true;
                foreach(Vector3 spawn in spawns) {
                    if(Vector3.Distance(spawnLocation, spawn) < 5) {
                        foundValid = false;
                        break;
                    }
                }
                if(foundValid) {
                    spawns.Add(spawnLocation);
                }
            }
        }

        Debug.Log(spawns.Count);
        Debug.Log(spawns[0]);
        Debug.Log(spawns[1]);
        Debug.Log(spawns[2]); 
        Instantiate(humanPrefab, spawns[0], Quaternion.identity, AIContainer.transform)
                .GetComponent<AIController>().startWander(corner.x, corner.x + spawnXRange, corner.z,corner.z + spawnZRange);
        Instantiate(rabbitPrefab, spawns[1], Quaternion.identity, AIContainer.transform)
                .GetComponent<AIController>().startWander(corner.x, corner.x + spawnXRange, corner.z,corner.z + spawnZRange);
        Instantiate(deerPrefab, spawns[2], Quaternion.identity, AIContainer.transform)
                .GetComponent<AIController>().startWander(corner.x, corner.x + spawnXRange, corner.z,corner.z + spawnZRange);

    }

    public void swapEnviroment() {

    }

    public void removeRoad() {
        Destroy(gameObject);
    }




}
