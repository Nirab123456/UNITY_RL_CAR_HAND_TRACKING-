using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyPlaneVector39000
{
    public float x;
    public float y;
    public float z;

    public MyPlaneVector39000(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyPlaneVector39000(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }

    public Vector3 AsVector3()
    {
        return new Vector3(x, y, z);
    }
}

public class Agent9000 : MonoBehaviour
{
    private Vector3 initialPosition; // Store the initial position of the sphere
    private bool shouldResetPosition_Plane = false; // Flag to indicate if position reset is requested
    private RpcService rpcService;

    void Start()
    {
        initialPosition = transform.position; // Store the initial position
        rpcService = new RpcService(this);
    }

    void Update()
    {
        if (shouldResetPosition_Plane)
        {
            ResetPosition_Plane_9000();
            shouldResetPosition_Plane = false;
        }
    }

    public void ResetPosition_Plane_9000()
    {
        transform.position = initialPosition; // Reset the position to the initial position
    }

    public class RpcService : JsonRpcService
    {
        private Agent9000 agent9000;

        public RpcService(Agent9000 agent9000)
        {
            this.agent9000 = agent9000;
        }

        [JsonRpcMethod]
        MyPlaneVector39000 GetPosition_Plane_9000()
        {
            return new MyPlaneVector39000(agent9000.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition_Plane_9000()
        {
            agent9000.ResetPosition_Plane_9000(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int PlayerCollisionDetected_9000()
        {
            return agent9000.PlayerCollisionDetected;
        }
    }

    private int PlayerCollisionDetected = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            // Object from the Car layer triggered the collision
            if (other.gameObject.name == "Car9000")
            {
                // Perform your desired actions for Car5000
            }
        }
    }


}
