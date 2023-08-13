using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarCam : MonoBehaviour
{
    public float sensitivity = 400f;
    public Transform playerBody;

    private float xRot = 0f;
    private float yRot = 0f;

    public GameObject fantasyCam;
    public GameObject darkCam;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        fantasyCam.SetActive(false);
        darkCam.SetActive(true);
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
        // playerBody.Rotate(Vector3.up * mouseX);

    }

    public void swapCam(WorldState state) {
        if(state == WorldState.FANTASY) {
            fantasyCam.SetActive(true);
            darkCam.SetActive(false);
        } else {
            fantasyCam.SetActive(false);
            darkCam.SetActive(true);
        }
    }
}
