using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyVector3_5000
{
    public float x;
    public float y;
    public float z;

    public MyVector3_5000 (float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyVector3_5000(Vector3 v)
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

public class Prometheus5000 : MonoBehaviour
{
    private bool collisionDetected = false;
    private Vector3 initialPosition; // Store the initial position of the sphere
    private bool shouldResetPosition = false; // Flag to indicate if position reset is requested
    private RpcService rpcService;
    private Quaternion initialRotation; // Store the initial rotation of the sphere


    void Start()
    {
        initialRotation = transform.rotation; // Store the initial rotation
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
        private Prometheus5000 prometheus5000;

        public RpcService(Prometheus5000 prometheus5000)
        {
            this.prometheus5000 = prometheus5000;
        }

        [JsonRpcMethod]
        MyVector3 GetPosition_5000()
        {
            return new MyVector3(prometheus5000.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition_5000()
        {
            prometheus5000.ResetPosition(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int GetWallCollision_5000()
        {
            return prometheus5000.wallCollisionDetected;
        }

        [JsonRpcMethod]
        public int GetRewardCollision_5000()
        {
            return prometheus5000.rewardCollisionDetected;
        }

        [JsonRpcMethod]
        public int GetPlaneCollision_5000()
        {
            return prometheus5000.PlaneCollisionDetected;
        }


        [JsonRpcMethod]
        public int CarCollisionDetected_5000()
        {
            return prometheus5000.CarCollisionDetected;
        }
        [JsonRpcMethod]
        public int GetMovingPlaneCollision_5000()
        {
            return prometheus5000.movingPlaneCollisionDetected;
        }


    }

    private int wallCollisionDetected = 0;
    private int rewardCollisionDetected = 0;
    private int PlaneCollisionDetected = 0;
    private int CarCollisionDetected = 0;
    private int movingPlaneCollisionDetected = 0;

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

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            wallCollisionDetected = 0;
        }

        if (collision.gameObject.CompareTag("Plane"))
        {
            PlaneCollisionDetected = 0;
        } 
        if (collision.gameObject.layer == LayerMask.NameToLayer("Car") && collision.gameObject != gameObject)
        {
            CarCollisionDetected = 0;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Reward"))
        {
            rewardCollisionDetected = 1;
        }
        if (other.CompareTag("Prometheus_5000"))
        {
            movingPlaneCollisionDetected = 1;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Reward"))
        {
            rewardCollisionDetected = 0;
        }
        if (other.CompareTag("Prometheus_5000"))
        {
            movingPlaneCollisionDetected = 0;
        }
    }


}
