using UnityEngine;

public class TargetTracker : MonoBehaviour
{
    public Transform target; // Reference to the target object to track
    public float trackingSpeed = 5f; // Speed at which the tracking object follows the target

    private void Update()
    {
        if (target != null)
        {
            // Calculate the direction from the tracking object to the target
            Vector3 direction = target.position - transform.position;

            // Calculate the rotation needed to face the target
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly rotate the tracking object towards the target
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, trackingSpeed * Time.deltaTime);
        }
    }
}
