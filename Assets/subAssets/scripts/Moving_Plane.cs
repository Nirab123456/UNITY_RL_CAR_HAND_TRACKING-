using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyPlaneVector3
{
    public float x;
    public float y;
    public float z;

    public MyPlaneVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyPlaneVector3(Vector3 v)
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

public class Moving_Plane : MonoBehaviour
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
            ResetPosition_Plane();
            shouldResetPosition_Plane = false;
        }
    }

    public void ResetPosition_Plane()
    {
        transform.position = initialPosition; // Reset the position to the initial position
    }

    public class RpcService : JsonRpcService
    {
        private Moving_Plane movingPlane;

        public RpcService(Moving_Plane movingPlane)
        {
            this.movingPlane = movingPlane;
        }

        [JsonRpcMethod]
        MyPlaneVector3 GetPosition_Plane()
        {
            return new MyPlaneVector3(movingPlane.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition_Plane()
        {
            movingPlane.ResetPosition_Plane(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int PlayerCollisionDetected()
        {
            return movingPlane.PlayerCollisionDetected;
        }
    }

    private int PlayerCollisionDetected = 0;


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Car") && other.gameObject != gameObject)
        {
            PlayerCollisionDetected = 1;
        }
    }
}
