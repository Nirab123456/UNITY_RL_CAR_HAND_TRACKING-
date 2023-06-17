using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class collision_9000 : MonoBehaviour
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
        private collision_9000 collision_9000;

        public RpcService(collision_9000 collision_9000)
        {
            this.collision_9000 = collision_9000;
        }

        [JsonRpcMethod]
        public int MovingPlaneCollisionDetected_9000()
        {
            return collision_9000.MovingPlaneCollisionDetected;
        }
    }

    private int MovingPlaneCollisionDetected = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prometheus_9000"))
        {
            MovingPlaneCollisionDetected = 1;
        }
    }
}
