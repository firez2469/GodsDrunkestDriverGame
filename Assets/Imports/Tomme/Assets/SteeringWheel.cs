using UnityEngine;
using UnityEngine.Events;
//using UnityEngine.XR.Interaction.Toolkit;

public class SteeringWheel : MonoBehaviour
{
    [SerializeField] private Transform wheelTransform;

    public float rotationZ;
    public float lookZLimit = 50;

    void Start()
    {
        transform.localRotation = Quaternion.Euler(-21, 180, rotationZ);
    }

    void Update()
    {
        rotationZ = Mathf.Clamp(transform.localRotation.z, -lookZLimit, lookZLimit);
        rotationZ = Mathf.Clamp(transform.localRotation.z, -50, 50);
        
        //transform.localRotation = Quaternion.Euler(x, y, rotationZ);
    }

}