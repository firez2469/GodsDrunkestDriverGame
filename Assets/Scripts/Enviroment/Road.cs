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
    public void spawnAI(int count) {
        List<Vector3> spawns = new List<Vector3>();

        for(int i = 0; i < count; i++) {
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

        for(int i = 0; i < count; i++) {
            GameObject willUse;
            int selected = Random.Range(0,3);
            if(selected == 0) {
                willUse = humanPrefab;
            } else if(selected == 1) {
                willUse = rabbitPrefab;
            } else {
                willUse = deerPrefab;
            }


            AIController newAI = Instantiate(willUse, spawns[i], Quaternion.identity, AIContainer.transform).GetComponent<AIController>();
            ai.Add(newAI);
            newAI.swapEnviroment(state);
            newAI.startWander(lowBound.x, highBound.x, lowBound.z, highBound.z);
        }
        
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
