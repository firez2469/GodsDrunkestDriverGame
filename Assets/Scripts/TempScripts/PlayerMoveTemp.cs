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

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.freezeRotation = true;
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
    }

    void FixedUpdate() {
        movePlayer();
    }

}
