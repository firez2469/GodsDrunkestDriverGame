using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveTemp : MonoBehaviour
{
    public float speed;
    public Transform orientation;
    public float groundFriction;

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;
    Rigidbody rigid;

    int health;
    public int maxHealth;
    int score;
    int currentScore;
    public int pointsToHeal;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
        score = 0;
        health = maxHealth;
        currentScore = 0;
    }

    void getInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    void movePlayer() {
        
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rigid.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
    }

     void speedCap() {
        Vector3 rigidV = new Vector3(rigid.velocity.x, 0, rigid.velocity.z);

        if(rigidV.magnitude > speed) {
            rigidV = rigidV.normalized*speed;
            rigid.velocity = new Vector3(rigidV.x, rigid.velocity.y, rigidV.z);
        }
        
    }

    // Update is called once per frame
    void Update() {
        getInput();
        speedCap();

        if(health < 0) { //placeholder as is everything in this class
            Debug.Log("JOEVER");
            Application.Quit();
        }
    }

    void FixedUpdate() {
        movePlayer();
    }

    public void boostSpeed(WorldState state) {
        if(state == WorldState.FANTASY) {
            speed *= 1.1f;
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
        Debug.Log("HEALTH, SCORE: " + health + ", " + score);
    }

    

}
