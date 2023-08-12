using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    float xLow, xHigh, zLow, zHigh;
    NavMeshAgent agent;
    bool traveling;

    public float idleTime;
    // Start is called before the first frame update
    void Awake()
    {
        traveling = false;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(agent.remainingDistance < .5f && traveling) {
            traveling = false;
            Invoke(nameof(wander), idleTime);
        }
    }

    public void startWander(float xLow, float xHigh, float zLow, float zHigh) {

        this.xLow = xLow;
        this.xHigh = xHigh;
        this.zLow = zLow;
        this.zHigh = zHigh;
        wander();
    }

    void wander() {
        agent.destination = new Vector3(Random.Range(xLow, xHigh), transform.position.y, Random.Range(zLow, zHigh));
        traveling = true;
    }

}
