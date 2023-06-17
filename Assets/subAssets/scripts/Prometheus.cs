using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyVector3
{
    public float x;
    public float y;
    public float z;

    public MyVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyVector3(Vector3 v)
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

public class Prometheus : MonoBehaviour
{
    private bool collisionDetected = false;
    private Vector3 initialPosition; // Store the initial position of the sphere
    private bool shouldResetPosition = false; // Flag to indicate if position reset is requested
    private RpcService rpcService;
    private Quaternion initialRotation; // Store the initial rotation of the sphere


    void Start()
    {
        initialPosition = transform.position; // Store the initial position
        initialPosition = transform.position; // Store the initial position
        rpcService = new RpcService(this);
    }

    void Update()
    {
        if (shouldResetPosition)
        {
            ResetPosition();
            shouldResetPosition = false;
        }
    }

    public void ResetPosition()
    {
        transform.rotation = initialRotation; // Reset the rotation to the initial rotation
        transform.position = initialPosition; // Reset the position to the initial position
    }

    public class RpcService : JsonRpcService
    {
        private Prometheus prometheus;

        public RpcService(Prometheus prometheus)
        {
            this.prometheus = prometheus;
        }

        [JsonRpcMethod]
        MyVector3 GetPosition()
        {
            return new MyVector3(prometheus.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition()
        {
            prometheus.ResetPosition(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int GetWallCollision()
        {
            return prometheus.wallCollisionDetected;
        }

        [JsonRpcMethod]
        public int GetRewardCollision()
        {
            return prometheus.rewardCollisionDetected;
        }

        [JsonRpcMethod]
        public int GetPlaneCollision()
        {
            return prometheus.PlaneCollisionDetected;
        }


        [JsonRpcMethod]
        public int CarCollisionDetected()
        {
            return prometheus.CarCollisionDetected;
        }


    }

    private int wallCollisionDetected = 0;
    private int rewardCollisionDetected = 0;
    private int PlaneCollisionDetected =0;
    private int CarCollisionDetected = 0;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallCollisionDetected = 1;
        }

        if (collision.gameObject.CompareTag("Plane"))
        {
            PlaneCollisionDetected = 1;
        } 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Car") && collision.gameObject != gameObject)
        {
            CarCollisionDetected = 1;
        }
        
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reward"))
        {
            rewardCollisionDetected = 1;
        }
        
    }

}
