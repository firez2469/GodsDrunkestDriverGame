using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls and manages all road segment behavior
public class RoadsController : MonoBehaviour
{

    List<Road> roads;
    public GameObject roadPrefab;

    public int maxSegments;
    public int initialSegments;
    Vector3 nextRoadPos;


    //temporary
    bool readyForNextRoad;

    // Start is called before the first frame update
    void Start()
    {
        nextRoadPos = new Vector3(0,0,roadPrefab.transform.localScale.z);
        roads = new List<Road>();
        
        for(int i = 0; i < initialSegments; i++) {
            extendRoad();
        }

        readyForNextRoad = true;
    }

    //will handle enviroment swap for all AI and road segments
    public void swapEnviroment() {
        foreach(Road r in roads) {
            r.swapEnviroment();
        }
    }

    public void Update() {
        if(readyForNextRoad) {
            Invoke(nameof(extendRoad), 5.0f);
            readyForNextRoad = false;
        }
        
    }

    public void extendRoad() {
        GameObject newRoad = Instantiate(roadPrefab, nextRoadPos, Quaternion.identity, transform);
        Road newRoadScript = newRoad.GetComponent<Road>();


        roads.Add(newRoadScript);
        if(roads.Count > maxSegments) {
            Road bye = roads[0];
            roads.RemoveAt(0);
            bye.removeRoad();
        }

        nextRoadPos = new Vector3(nextRoadPos.x,nextRoadPos.y,nextRoadPos.z + roadPrefab.transform.localScale.z);
        readyForNextRoad = true;
    }
}
