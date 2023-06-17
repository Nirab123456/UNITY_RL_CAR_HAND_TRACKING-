using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Collision_8000 : MonoBehaviour
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
        private Collision_8000 collision_8000;

        public RpcService(Collision_8000 collision_8000)
        {
            this.collision_8000 = collision_8000;
        }

        [JsonRpcMethod]
        public int MovingPlaneCollisionDetected_8000()
        {
            return collision_8000.MovingPlaneCollisionDetected;
        }
    }

    private int MovingPlaneCollisionDetected = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Prometheus_8000"))
        {
            MovingPlaneCollisionDetected = 1;
        }
    }
}
