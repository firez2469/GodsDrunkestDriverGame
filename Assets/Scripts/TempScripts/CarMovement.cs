using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float speedBoost;

    public float tripTime;
    public float sleepTime;

    public RoadsController roads;

    public float maxForceBoost;

    public float turnSpeed;

    public int maxHealth;
    int health;
    public int pointsToHeal;

    int score;
    int currentScore;

    WorldState state;

    Rigidbody rigid;

    float swapTime;
    public float swapCooldown;

    bool canSwap;

    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        currentScore = 0;
        health = maxHealth;

        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;

        state = WorldState.DARK;
        swapTime = Time.time;
        canSwap = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && state == WorldState.DARK && canSwap) {
            canSwap = false;
            state = WorldState.FANTASY;
            Invoke(nameof(endTrip), tripTime);
            this.boostSpeed(state);
            roads.swapEnviroment(state);
        }

        if(Time.time - swapTime > sleepTime || health < 0) {
            //GG
        }
    }

    void endTrip() {
        state = WorldState.DARK;
        roads.swapEnviroment(state);

        Invoke(nameof(shroomCooldown), swapCooldown);
    }

    void shroomCooldown() {
        canSwap = true;
    }

    void FixedUpdate() {
        rigid.MoveRotation(rigid.rotation * Quaternion.Euler(0f, Input.GetAxis("Horizontal") * turnSpeed, 0f));
        
        rigid.velocity = speed * transform.right.normalized;
    }

    public void boostSpeed(WorldState state) {
        if(state == WorldState.FANTASY) {
            speed *= speedBoost;
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
}


