using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    [Header ("speed")]
    public float speed;
    public float maxSpeed;
    public float speedIncrease;
    public float turnSpeed;

    [Header ("gravity bounce and collisions")]
    public float distOffGround;
    public LayerMask ignoreMe;
    public float springStrength;
    public float stringDamper;

    public float rotSpringStrength;
    public float rotSpringDamper;

    Quaternion uprightRotationTarget;

    public float maxForceBoost;

    [Header ("tripping")]
    public RoadsController road;
    public float tripTime;
    public float tripCooldown;

    bool canTrip;
    WorldState state;

    [Header ("hp and points")]
    public int maxHealth;
    int health;
    public int pointsToHeal;
    int score;
    int currentScore;

    Rigidbody rigid;

    // Start is called before the first frame update
    void Start()
    {
        uprightRotationTarget = transform.rotation;
        score = 0;
        currentScore = 0;
        health = maxHealth;

        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;

        canTrip = true;
        state = WorldState.DARK;
    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q)) {
            transform.position = new Vector3(transform.position.x,transform.position.y + 2,transform.position.z);
        }
        if(Input.GetKeyDown(KeyCode.Space) && canTrip) {
            canTrip = false;
            state = WorldState.FANTASY;
            road.swapEnviroment(state);
            this.boostSpeed(state);
            Invoke(nameof(exitTrip), tripTime);
        }
    }

    void exitTrip() {
        state = WorldState.DARK;
        road.swapEnviroment(state);
        this.boostSpeed(state);
        Invoke(nameof(endCD), tripCooldown);
    }

    void endCD() {
        canTrip = true;
    }

    void FixedUpdate() {

        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0f, Input.GetAxis("Horizontal") * turnSpeed, 0f));
        Vector3 xzSpeed = speed * transform.right.normalized;
        rigid.velocity = new Vector3(xzSpeed.x, rigid.velocity.y, xzSpeed.z);

        rideGround();
    }

  
    public void boostSpeed(WorldState state) {
        if(state == WorldState.FANTASY) {
            speed *= speedIncrease;
            if(speed > maxSpeed) {
                speed = maxSpeed;
            }
        } else if(currentScore > pointsToHeal){
            health += (int)(.5f * (currentScore - pointsToHeal));
            if(health > maxHealth) {
                health = maxHealth;
            }
        }
        
    }

    public void hitThing(int value, WorldState state) {
        if(state == WorldState.FANTASY) {
            currentScore += value;
            score += value;
        } else {
            health -= value;
        }
    }

    //how hard I hit the
    public float getSpeedScalar() {
        return 1+(speed*maxForceBoost/maxSpeed);
    }

    void rideGround() {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, -transform.up, out hit, 200f, ~ignoreMe)) {

            Debug.Log("HIT: " + hit.distance + ", " + hit.collider.gameObject.name);
            

            float rayDirVel = Vector3.Dot(-transform.up, rigid.velocity);

            float x = hit.distance - distOffGround;

            float springForce = (x * springStrength) - (rayDirVel * stringDamper);
            Debug.Log("SP STR: " + (x * springStrength) + ", DAMPER: " + (rayDirVel * stringDamper) + ", TOTAL: " + springForce + ", YVEL: " + rigid.velocity.y);

            rigid.AddForce(-transform.up * springForce, ForceMode.Force);

            // Debug.Log("RIGIDV: " + rigid.velocity);
        } else {
            Debug.Log("HUGE PROBLEM NO GROUND");
        }
    }

    public void carTorque() {
        // Quaternion goal = Utils.Math.ShortestRotation(uprightRotationTarget, transform.rotation);

        // Vector3 axis;
        // float rotationDegrees;

        // goal.ToAngleAxis(out rotationDegrees, out axis);

        // rigid.AddTorque((axis * (rotationDegrees * Mathf.Deg2Rad * rotSpringStrength)) - (rigid.angularVelocity * rotSpringDamper));
    }
}
