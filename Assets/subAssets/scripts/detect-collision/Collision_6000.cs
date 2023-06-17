using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Collision_6000 : MonoBehaviour
{
    private RpcService rpcService;

    void Start()
    {
        rpcService = new RpcService(this);
    }

    void Update()
    {

    }

    public class RpcService : JsonRpcService
    {
        private Collision_6000 collision_6000;

        public RpcService(Collision_6000 collision_6000)
        {
            this.collision_6000 = collision_6000;
        }

        [JsonRpcMethod]
        public int MovingPlaneCollisionDetected_6000()
        {
            return collision_6000.MovingPlaneCollisionDetected;
        }
    }

    private int MovingPlaneCollisionDetected = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prometheus_6000"))
        {
            MovingPlaneCollisionDetected = 1;
        }
    }
}
