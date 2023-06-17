using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class collision_7000 : MonoBehaviour
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
        private collision_7000 collision_7000;

        public RpcService(collision_7000 collision_7000)
        {
            this.collision_7000 = collision_7000;
        }

        [JsonRpcMethod]
        public int MovingPlaneCollisionDetected_7000()
        {
            return collision_7000.MovingPlaneCollisionDetected;
        }
    }

    private int MovingPlaneCollisionDetected = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prometheus_7000"))
        {
            MovingPlaneCollisionDetected = 1;
        }
    }
}
