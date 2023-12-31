using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    float xLow, xHigh, zLow, zHigh;
    NavMeshAgent agent;
    Rigidbody rigid;
    bool traveling;
    float launchCooldown;
    float lastLaunch;

    bool launched;
    
    WorldState state;

    [Header ("general")]
    public float idleTime;
    LayerMask car;

    [Header ("DARK")]
    public float launchForce;
    public float yLaunchForce;
    public float darkmass;

    public int carDamage;

    [Header ("FANTASY")]
    public float dayLaunchForce;
    public float dayYLaunchForce;
    public float daymass;

    public int pointValue;

    // Start is called before the first frame update
    void Awake()
    {
        lastLaunch = 0;
        traveling = false;
        launchCooldown = .5f;
        agent = GetComponent<NavMeshAgent>();
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!launched && agent.remainingDistance < .5f && traveling) {
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
        if(!launched) {
            agent.destination = new Vector3(Random.Range(xLow, xHigh), transform.position.y, Random.Range(zLow, zHigh));
            traveling = true;
        }
    }

    void OnCollisionEnter(Collision collision) {
        if(collision.gameObject.name == "player" && Time.time - lastLaunch > launchCooldown) {

            CarMovement player = collision.gameObject.GetComponent<CarMovement>();
            int value = (state == WorldState.FANTASY) ? pointValue : carDamage;
            player.hitThing(value, state);
            //send damage/score to player

            lastLaunch = Time.time;
            launched = true;
            rigid.freezeRotation = false;

            float launchStrength = (state == WorldState.FANTASY) ? dayLaunchForce : launchForce;
            float yLaunchStrength = (state == WorldState.FANTASY) ? dayYLaunchForce : yLaunchForce;

            Vector3 launchVec = transform.position - collision.gameObject.transform.position;
            launchVec.y = 0;
            launchVec = launchVec.normalized * launchStrength * player.getSpeedScalar();;
            launchVec.y = yLaunchStrength;

            agent.enabled = false;
            rigid.AddForce(launchVec, ForceMode.Impulse);
        }

    }

    public void swapEnviroment(WorldState time) {
        state = time;
        if(state == WorldState.DARK) {
            rigid.mass = darkmass;
            //swap to dark skin
        } else {
            rigid.mass = daymass;
            //swap to fantasy skin
        }
    }

    public int getPointDamage() {
        return (state == WorldState.FANTASY) ? pointValue : carDamage;
    }

}
