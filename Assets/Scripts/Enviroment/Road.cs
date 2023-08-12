using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles behavior of each road segment (spawning AI, detecting when player has left the segment, swaping sprites)
public class Road : MonoBehaviour
{
    RoadsController parent;
    Vector3 corner;
    float spawnXRange;
    float spawnZRange;

    List<AIController> ai;

    public GameObject humanPrefab;
    public GameObject rabbitPrefab;
    public GameObject deerPrefab;

    bool day;

    public GameObject AIContainer;

    void Awake()
    {
        parent = GetComponentInParent<RoadsController>();
        ai = new List<AIController>();
        day = false;

        corner = new Vector3(transform.position.x - transform.localScale.x/2,
                 transform.position.y + transform.localScale.y/2,
                 transform.position.z - transform.localScale.z/2);
        spawnXRange = transform.localScale.x;
        spawnZRange = transform.localScale.z;
        AIContainer.transform.position = corner;
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

        //will abstract later am lazy
        AIController human = Instantiate(humanPrefab, spawns[0], Quaternion.identity, AIContainer.transform).GetComponent<AIController>();
        ai.Add(human);
        human.swapEnviroment(day);
        human.startWander(corner.x, corner.x + spawnXRange, corner.z,corner.z + spawnZRange);

        AIController rabbit = Instantiate(rabbitPrefab, spawns[1], Quaternion.identity, AIContainer.transform).GetComponent<AIController>();
        ai.Add(rabbit);
        rabbit.swapEnviroment(day);
        rabbit.startWander(corner.x, corner.x + spawnXRange, corner.z,corner.z + spawnZRange);

        AIController deer = Instantiate(deerPrefab, spawns[2], Quaternion.identity, AIContainer.transform).GetComponent<AIController>();
        ai.Add(deer);
        deer.swapEnviroment(day);
        deer.startWander(corner.x, corner.x + spawnXRange, corner.z,corner.z + spawnZRange);
    }

    public void swapEnviroment(bool time) {
        day = time;
        //swap enviroment here

        //swap enviroment on every ai

        foreach(AIController thing in ai) {
            thing.swapEnviroment(day);
        }
    }

    public void removeRoad() {
        Destroy(gameObject);
    }

    public void roadTrigger() {
        parent.extendRoad();
    }




}
