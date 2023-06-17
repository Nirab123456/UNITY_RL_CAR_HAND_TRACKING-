using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navmesh_Get_Reward : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoint positions
    private int currentWaypointIndex = 0; // Index of the current waypoint
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Set the initial destination to the first waypoint
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }

    private void Update()
    {
        // Check if the agent has reached the current waypoint
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // Move to the next waypoint
            currentWaypointIndex++;

            // Check if all waypoints have been visited
            if (currentWaypointIndex >= waypoints.Length)
            {
                // If all waypoints have been visited, stop the agent
                agent.isStopped = true;
            }
            else
            {
                // Set the destination to the next waypoint
                agent.SetDestination(waypoints[currentWaypointIndex].position);
            }
        }
    }
}
