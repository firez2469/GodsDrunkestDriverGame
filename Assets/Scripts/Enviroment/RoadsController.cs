using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls and manages all road segment behavior
public class RoadsController : MonoBehaviour
{

    WorldState state;

    List<Road> roads;
    public GameObject roadPrefab;

    public GameObject darkVolume;
    public GameObject fantasyVolume;

    public CarMovement player;
    public CarCam carcamms;

    public int maxSegments;
    public int initialSegments;
    public int AISpawnDistance;
    Vector3 nextRoadPos;

    // Start is called before the first frame update
    void Start()
    {
        state = WorldState.DARK;
        darkVolume.SetActive(true);
        fantasyVolume.SetActive(false);
        nextRoadPos = new Vector3(0,0,0);
        roads = new List<Road>();
        
        for(int i = 0; i < initialSegments; i++) {
            extendRoad();
        }

        Debug.Log(roads.Count);

        for(int i = 0; i < AISpawnDistance; i++) {
            roads[i].spawnAI();
        }
    }

    //will handle enviroment swap for all AI and road segments
    public void swapEnviroment() {
        state = (state == WorldState.FANTASY) ? WorldState.DARK : WorldState.FANTASY;

        if(state == WorldState.FANTASY) {
            darkVolume.SetActive(false);
            fantasyVolume.SetActive(true);
        } else {
            darkVolume.SetActive(true);
            fantasyVolume.SetActive(false);
        }

        carcamms.swapCam(state);


        foreach(Road r in roads) {
            r.swapEnviroment(state);
        }
    }

    public void Update() {
        // if(readyForNextRoad) {
        //     Invoke(nameof(extendRoad), 5.0f);
        //     readyForNextRoad = false;
        // }
        if(Input.GetKeyDown(KeyCode.Space)) {
            swapEnviroment();
            player.boostSpeed(state);
        }
        
    }

    public void extendRoad() {
        GameObject newRoad = Instantiate(roadPrefab, nextRoadPos, Quaternion.identity, transform);
        Road newRoadScript = newRoad.GetComponent<Road>();

        roads.Add(newRoadScript);
        newRoadScript.swapEnviroment(state);
        

        if(roads.Count > maxSegments) {
            Road bye = roads[0];
            roads.RemoveAt(0);
            bye.removeRoad();
        }

        if(roads.Count >= AISpawnDistance) {
            roads[AISpawnDistance-1].spawnAI();
        }

        nextRoadPos = new Vector3(nextRoadPos.x,nextRoadPos.y,nextRoadPos.z + 100);
    }
}

public enum WorldState { DARK,FANTASY}
