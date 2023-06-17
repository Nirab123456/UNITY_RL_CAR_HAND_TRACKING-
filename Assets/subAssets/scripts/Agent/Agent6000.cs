using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyPlaneVector36000
{
    public float x;
    public float y;
    public float z;

    public MyPlaneVector36000(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyPlaneVector36000(Vector3 v)
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

public class Agent6000 : MonoBehaviour
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
            ResetPosition_Plane_6000();
            shouldResetPosition_Plane = false;
        }
    }

    public void ResetPosition_Plane_6000()
    {
        transform.position = initialPosition; // Reset the position to the initial position
    }

    public class RpcService : JsonRpcService
    {
        private Agent6000 agent6000;

        public RpcService(Agent6000 agent6000)
        {
            this.agent6000 = agent6000;
        }

        [JsonRpcMethod]
        MyPlaneVector36000 GetPosition_Plane_6000()
        {
            return new MyPlaneVector36000(agent6000.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition_Plane_6000()
        {
            agent6000.ResetPosition_Plane_6000(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int PlayerCollisionDetected_6000()
        {
            return agent6000.PlayerCollisionDetected;
        }
    }

    private int PlayerCollisionDetected = 0;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            // Object from the Car layer triggered the collision
            if (other.gameObject.name == "Car6000")
            {
                // Perform your desired actions for Car5000
            }
        }
    }


}
