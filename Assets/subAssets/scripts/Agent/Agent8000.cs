using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyPlaneVector38000
{
    public float x;
    public float y;
    public float z;

    public MyPlaneVector38000(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyPlaneVector38000(Vector3 v)
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

public class Agent8000 : MonoBehaviour
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
            ResetPosition_Plane_8000();
            shouldResetPosition_Plane = false;
        }
    }

    public void ResetPosition_Plane_8000()
    {
        transform.position = initialPosition; // Reset the position to the initial position
    }

    public class RpcService : JsonRpcService
    {
        private Agent8000 agent8000;

        public RpcService(Agent8000 agent8000)
        {
            this.agent8000 = agent8000;
        }

        [JsonRpcMethod]
        MyPlaneVector38000 GetPosition_Plane_8000()
        {
            return new MyPlaneVector38000(agent8000.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition_Plane_8000()
        {
            agent8000.ResetPosition_Plane_8000(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int PlayerCollisionDetected_8000()
        {
            return agent8000.PlayerCollisionDetected;
        }
    }

    private int PlayerCollisionDetected = 0;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            // Object from the Car layer triggered the collision
            if (other.gameObject.name == "Car8000")
            {
                // Perform your desired actions for Car5000
            }
        }
    }


}
