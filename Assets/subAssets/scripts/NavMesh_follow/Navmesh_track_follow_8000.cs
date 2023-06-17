using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navmesh_track_follow_8000 : MonoBehaviour
{
    private GameObject player;
    private UnityEngine.AI.NavMeshAgent agent;

    private void Start()
    {
        player = GameObject.Find("Car8000");
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
    }

    private void Update()
    {
        if (player != null)
        {
            agent.SetDestination(player.transform.position);
        }
    }
}
