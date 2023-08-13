using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
    public float oppositeForce; //gonna just use 3* sqrt(speed)
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
    public int pointsToHeal;
    public int obstacleDamage;

    public float scoreStreakTime;

    float lastScore;
    int streakCount;

    int health;
    
    int score;
    int currentScore;

    Rigidbody rigid;

    [Header ("UI")]
    public GameObject scoreTextPrefab;
    public Transform textPosition;
    public Transform textParent;

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
            this.swapState(state);
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
        this.swapState(state);
        Invoke(nameof(endCD), tripCooldown);
    }

    void endCD() {
        canTrip = true;
    }

    void FixedUpdate() {

        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0f, Input.GetAxis("Horizontal") * turnSpeed, 0f));
        rigid.AddForce(transform.right * speed, ForceMode.Force);
        // Debug.Log("FORWARD: " + transform.forward);

        // Vector3 xzSpeed = speed * transform.right.normalized;
        // rigid.velocity = new Vector3(xzSpeed.x, rigid.velocity.y, xzSpeed.z);

        rideGround();
        carTorque();
    }

  
    public void swapState(WorldState state) {
        if(state == WorldState.FANTASY) {
            speed *= speedIncrease;
            if(speed > maxSpeed) {
                speed = maxSpeed;
            }

            // ScoreText.SetActive(true);
            // multiplierText.SetActive(true);
            // newScoreText.SetActive(true);

        } else {
            if(currentScore > pointsToHeal){
                health += (int)(.5f * (currentScore - pointsToHeal));
                if(health > maxHealth) {
                    health = maxHealth;
                }

                currentScore = 0;
            }
            // ScoreText.SetActive(false);
            // multiplierText.SetActive(false);
            // newScoreText.SetActive(false);
        } 
    }

    public void hitThing(int value, WorldState state) {
        if(state == WorldState.FANTASY) {
            streakCount++;
            Invoke(nameof(checkstreak), scoreStreakTime+.0001f);
            lastScore = Time.time;
            currentScore += value * streakCount;
            score += value * streakCount;

            showText(value * streakCount);
        } else {
            health -= value;
        }

        int willHeal = (int)(.5f * (currentScore - pointsToHeal));
        Debug.Log("SCORE: " + score + ", CURRENTSCORE " + currentScore + ", CURRENT STREAK" + streakCount + ", WILLHEAL " + willHeal + ", HEALTH" + health);
        // ScoreText.text = ""+score;
        // multiplierText.text = "x" + streakCount;

    }

    void showText(int amount) {
        GameObject textThing = Instantiate(scoreTextPrefab, textPosition.position, Quaternion.identity, textParent);
        textThing.GetComponent<TextMesh>().text = "+" + amount;
    }

    void checkstreak() {
        if(Time.time - lastScore > scoreStreakTime) {
            streakCount = 0;
            // multiplierText.text = "";
        }
    }

    //how hard I hit the thing
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
        float rotationDegrees = Quaternion.Angle(rigid.rotation, uprightRotationTarget);

        // Debug.Log("ROT: " + rigid.rotation + ", targ " + uprightRotationTarget + ", amountOfDeegs" + rotationDegrees);
        Vector3 direction =  Vector3.Cross(Vector3.up, transform.up);

        // Debug.Log("DIRECTION: " + direction);

        rigid.AddTorque((-direction * (rotationDegrees * Mathf.Deg2Rad * rotSpringStrength)) - (rigid.angularVelocity * rotSpringDamper));
    }

    void OnCollisionEnter(Collision collision) {
        if(((1 <<collision.gameObject.layer) & obstacleLayer) != 0) {
            health -= obstacleDamage;

            Vector3 direction = transform.position - collision.gameObject.transform.position;
            direction.y = 0;
            rigid.AddForce(direction.normalized * 3.0f * (float)Mathf.Sqrt(speed) + Vector3.up * oppositeCollisionYForce, ForceMode.Impulse);
        }
    }
}
