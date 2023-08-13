using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls and manages all road segment behavior
public class RoadsController : MonoBehaviour
{

    WorldState state;

    List<Road> roads;

    [Header ("road assembling")]
    public GameObject roadPrefab;
    public GameObject backCliff;
    public int maxSegments;
    public int initialSegments;
    public int AISpawnDistance;

    [Header ("world states")]
    public GameObject darkVolume;
    public GameObject fantasyVolume;
    public CarCam carcamms;

    [Header ("AI spawning behavior")]
    public int stateSwapsTillSpawn;
    public int initialCount;
    public float countMultiplier;
    public int maxCount;

    float currentSpawnAmount;
    int swaps;

    Vector3 nextRoadPos;
    Vector3 backCliffPos;
    

    // Start is called before the first frame update
    void Start()
    {
        state = WorldState.DARK;
        darkVolume.SetActive(true);
        fantasyVolume.SetActive(false);
        nextRoadPos = new Vector3(0,0,0);
        backCliffPos = new Vector3(0,5.25f,-79);
        roads = new List<Road>();
        currentSpawnAmount = 0f;
        
        for(int i = 0; i < initialSegments; i++) {
            extendRoad();
        }

        Debug.Log(roads.Count);

        for(int i = 0; i < AISpawnDistance; i++) {
            roads[i].spawnAI((int)currentSpawnAmount);
        }
    }

    //will handle enviroment swap for all AI and road segments
    public void swapEnviroment(WorldState time) {
        state = time;

        if(state == WorldState.FANTASY) {
            darkVolume.SetActive(false);
            fantasyVolume.SetActive(true);
        } else {
            darkVolume.SetActive(true);
            fantasyVolume.SetActive(false);

            swaps++;
        }

        if(state == WorldState.DARK && swaps >= stateSwapsTillSpawn && currentSpawnAmount == 0) {
            currentSpawnAmount = initialCount;
        } else if(state == WorldState.DARK && swaps >= stateSwapsTillSpawn && currentSpawnAmount < maxCount) {
            currentSpawnAmount = (currentSpawnAmount*countMultiplier >= maxCount) ? maxCount : currentSpawnAmount*countMultiplier;
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
            backCliffPos = backCliffPos + new Vector3(0,0,100);
        }

        if(roads.Count >= AISpawnDistance) {
            roads[AISpawnDistance-1].spawnAI((int)currentSpawnAmount);
        }

        nextRoadPos = new Vector3(nextRoadPos.x,nextRoadPos.y,nextRoadPos.z + 100);
        backCliff.transform.position = backCliffPos;
    }
}

public enum WorldState { DARK,FANTASY}
