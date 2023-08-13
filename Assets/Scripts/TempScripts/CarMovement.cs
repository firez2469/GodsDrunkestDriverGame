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
    public float friction;

    [Header ("gravity bounce")]
    public float distOffGround;
    public LayerMask ignoreMe;
    public float springStrength;
    public float stringDamper;
    public float rotSpringStrength;
    public float rotSpringDamper;

    [Header ("collisions")]
    public LayerMask obstacleLayer;
    public float maxForceBoost;
    Quaternion uprightRotationTarget;
    public float oppositeForce;
    public float oppositeCollisionYForce;
    float lastCollision;
    public float collisionCooldown;

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
        
        score = 0;
        currentScore = 0;
        health = maxHealth;

        rigid = GetComponent<Rigidbody>();
        rigid.drag = friction;
        uprightRotationTarget = rigid.rotation;
        // rigid.freezeRotation = true;

        canTrip = true;
        state = WorldState.DARK;
        lastCollision = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        capSpeed();
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

    void capSpeed() {
        Vector3 xzSpeed = rigid.velocity;
        xzSpeed.y = 0;
        if(xzSpeed.magnitude > speed) {
            xzSpeed = xzSpeed.normalized * speed;
        }
        rigid.velocity = new Vector3(xzSpeed.x, rigid.velocity.y, xzSpeed.z);
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
        rigid.AddForce(transform.right * speed, ForceMode.Force);
        Debug.Log("FORWARD: " + transform.forward);

        // Vector3 xzSpeed = speed * transform.right.normalized;
        // rigid.velocity = new Vector3(xzSpeed.x, rigid.velocity.y, xzSpeed.z);

        rideGround();
        carTorque();
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

            // Debug.Log("HIT: " + hit.distance + ", " + hit.collider.gameObject.name);
            

            float rayDirVel = Vector3.Dot(-transform.up, rigid.velocity);

            float x = hit.distance - distOffGround;

            float springForce = (x * springStrength) - (rayDirVel * stringDamper);
            // Debug.Log("SP STR: " + (x * springStrength) + ", DAMPER: " + (rayDirVel * stringDamper) + ", TOTAL: " + springForce + ", YVEL: " + rigid.velocity.y);

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
        float rotationDegrees = Quaternion.Angle(rigid.rotation, uprightRotationTarget);

        Debug.Log("ROT: " + rigid.rotation + ", targ " + uprightRotationTarget + ", amountOfDeegs" + rotationDegrees);
        Vector3 direction =  Vector3.Cross(Vector3.up, transform.up);

        Debug.Log("DIRECTION: " + direction);

        rigid.AddTorque((-direction * (rotationDegrees * Mathf.Deg2Rad * rotSpringStrength)) - (rigid.angularVelocity * rotSpringDamper));
    }

    void OnCollisionEnter(Collision collision) {
        if(((1 <<collision.gameObject.layer) & obstacleLayer) != 0) {
            Vector3 direction = transform.position - collision.gameObject.transform.position;
            direction.y = 0;
            rigid.AddForce(direction.normalized * oppositeForce + Vector3.up * oppositeCollisionYForce, ForceMode.Impulse);
        }
    }
}
