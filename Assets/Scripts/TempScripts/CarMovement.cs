using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CarMovement : MonoBehaviour
{
    public float speed;
    public float maxSpeed;
    public float speedIncrease;

    public float maxForceBoost;

    public RoadsController road;
    public float tripTime;
    public float tripCooldown;
    bool canTrip;
    WorldState state;

    public float turnSpeed;

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
        rigid.freezeRotation = true;

        canTrip = true;
        state = WorldState.DARK;
    }

    // Update is called once per frame
    void Update()
    {
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
        
        rigid.velocity = speed * transform.right.normalized;
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
}
