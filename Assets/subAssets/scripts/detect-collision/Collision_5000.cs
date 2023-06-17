using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Collision_5000 : MonoBehaviour
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
        private Collision_5000 collision_5000;

        public RpcService(Collision_5000 collision_5000)
        {
            this.collision_5000 = collision_5000;
        }

        [JsonRpcMethod]
        public int MovingPlaneCollisionDetected_5000()
        {
            return collision_5000.MovingPlaneCollisionDetected;
        }
    }

    private int MovingPlaneCollisionDetected = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prometheus_5000"))
        {
            MovingPlaneCollisionDetected = 1;
        }
    }
}
