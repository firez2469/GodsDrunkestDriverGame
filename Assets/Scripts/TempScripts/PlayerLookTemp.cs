using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLookTemp : MonoBehaviour
{

    public float sensitivity = 400f;
    public Transform playerBody;

    private float xRot = 0f;
    private float yRot = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;


        xRot -= mouseY;
        yRot += mouseX;
        xRot = Mathf.Clamp(xRot, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRot, yRot, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }

    public Vector3 getCameraForward() {
        return transform.forward;
    }
}
