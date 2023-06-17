using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    [Header("Settings")]
    public Transform car;
    public float distance = 6.4f;
    public float height = 1.4f;

    [Header("Damping")]
    public float rotationDamping = 3.0f;
    public float heightDamping = 2.0f;

    [Header("Zoom Settings")]
    public float zoomRatio = 0.5f;
    public float baseFOV = 60f;
    public float maxFOV = 75f;

    private Vector3 rotationVector;

    private void LateUpdate()
    {
        float wantedAngle = rotationVector.y;
        float wantedHeight = car.position.y + height;

        float currentAngle = transform.eulerAngles.y;
        float currentHeight = transform.position.y;

        currentAngle = Mathf.LerpAngle(currentAngle, wantedAngle, rotationDamping * Time.deltaTime);
        currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

        Quaternion currentRotation = Quaternion.Euler(0, currentAngle, 0);
        transform.position = car.position - currentRotation * Vector3.forward * distance;
        transform.position = new Vector3(transform.position.x, currentHeight, transform.position.z);

        transform.LookAt(car);
    }

    private void FixedUpdate()
    {
        Vector3 localVelocity = car.InverseTransformDirection(car.GetComponent<Rigidbody>().velocity);

        if (localVelocity.z < -0.1f)
        {
            rotationVector.y = car.eulerAngles.y + 180;
        }
        else
        {
            rotationVector.y = car.eulerAngles.y;
        }

        // Setting the field of view of the camera based on car's velocity
        float acceleration = car.GetComponent<Rigidbody>().velocity.magnitude;
        float targetFOV = baseFOV + acceleration * zoomRatio;

        // Clamp the FOV within the specified range
        targetFOV = Mathf.Clamp(targetFOV, baseFOV, maxFOV);

        // Smoothly adjust the camera FOV
        GetComponent<Camera>().fieldOfView = Mathf.Lerp(GetComponent<Camera>().fieldOfView, targetFOV, Time.deltaTime);
    }
}
