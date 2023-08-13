using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//handles behavior of each road segment (spawning AI, detecting when player has left the segment, swaping sprites)
public class Road : MonoBehaviour
{
    RoadsController parent;

    List<AIController> ai;

    public GameObject humanPrefab;
    public GameObject rabbitPrefab;
    public GameObject deerPrefab;

    public GameObject fantasyRoad;
    public GameObject darkRoad;

    public GameObject lowBoundMarker;
    public GameObject highBoundMarker;

    Vector3 lowBound;
    Vector3 highBound;

    WorldState state;

    public GameObject AIContainer;

    void Awake()
    {
        parent = GetComponentInParent<RoadsController>();
        ai = new List<AIController>();

        lowBound = lowBoundMarker.transform.position;
        highBound = highBoundMarker.transform.position;

        AIContainer.transform.position = lowBound;
    }

    //for now will be hardcoded to one of each
    public void spawnAI() {
        List<Vector3> spawns = new List<Vector3>();

        for(int i = 0; i < 3; i++) {
            bool foundValid = false;
            while(!foundValid) { // this is a bit silly D:
                Vector3 spawnLocation = new Vector3(Random.Range(lowBound.x, highBound.x), lowBound.y, Random.Range(lowBound.z, highBound.z));
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
        human.swapEnviroment(state);
        human.startWander(lowBound.x, highBound.x, lowBound.z, highBound.z);

        AIController rabbit = Instantiate(rabbitPrefab, spawns[1], Quaternion.identity, AIContainer.transform).GetComponent<AIController>();
        ai.Add(rabbit);
        rabbit.swapEnviroment(state);
        rabbit.startWander(lowBound.x, highBound.x, lowBound.z, highBound.z);

        AIController deer = Instantiate(deerPrefab, spawns[2], Quaternion.identity, AIContainer.transform).GetComponent<AIController>();
        ai.Add(deer);
        deer.swapEnviroment(state);
        deer.startWander(lowBound.x, highBound.x, lowBound.z, highBound.z);
    }

    public void swapEnviroment(WorldState time) {
        state = time;
        if(state == WorldState.DARK) {
            darkRoad.SetActive(true);
            fantasyRoad.SetActive(false);
        } else {
            darkRoad.SetActive(false);
            fantasyRoad.SetActive(true);
        }

        foreach(AIController thing in ai) {
            thing.swapEnviroment(state);
        }
    }

    public void removeRoad() {
        Destroy(gameObject);
    }

    public void roadTrigger() {
        parent.extendRoad();
    }




}
