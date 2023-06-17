using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyPlaneVector35000
{
    public float x;
    public float y;
    public float z;

    public MyPlaneVector35000(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyPlaneVector35000(Vector3 v)
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

public class Agent5000 : MonoBehaviour
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
            ResetPosition_Plane_5000();
            shouldResetPosition_Plane = false;
        }
    }

    public void ResetPosition_Plane_5000()
    {
        transform.position = initialPosition; // Reset the position to the initial position
    }

    public class RpcService : JsonRpcService
    {
        private Agent5000 agent5000;

        public RpcService(Agent5000 agent5000)
        {
            this.agent5000 = agent5000;
        }

        [JsonRpcMethod]
        MyPlaneVector35000 GetPosition_Plane_5000()
        {
            return new MyPlaneVector35000(agent5000.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition_Plane_5000()
        {
            agent5000.ResetPosition_Plane_5000(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int PlayerCollisionDetected_5000()
        {
            return agent5000.PlayerCollisionDetected;
        }
    }

    private int PlayerCollisionDetected = 0;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            // Object from the Car layer triggered the collision
            if (other.gameObject.name == "Car5000")
            {
                // Perform your desired actions for Car5000
            }
        }
    }


}
