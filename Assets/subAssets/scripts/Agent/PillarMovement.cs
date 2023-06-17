using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PillarMovement : MonoBehaviour
{
    public Transform startPoint; // Waypoint at one end
    public Transform endPoint;   // Waypoint at the other end

    private NavMeshAgent agent;
    private Transform currentTarget; // Current target waypoint

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        SetNewDestination(startPoint);
    }

    private void SetNewDestination(Transform target)
    {
        agent.SetDestination(target.position);
        currentTarget = target;
    }

    private void Update()
    {
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Reached the current target waypoint
            if (currentTarget == startPoint)
            {
                // Reached the startPoint, set new destination to endPoint
                SetNewDestination(endPoint);
            }
            else if (currentTarget == endPoint)
            {
                // Reached the endPoint, set new destination to startPoint
                SetNewDestination(startPoint);
            }
        }
    }
}
