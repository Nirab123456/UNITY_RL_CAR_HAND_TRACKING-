using UnityEngine;
using AustinHarris.JsonRpc;
using System.Collections.Generic;
using System.Collections;

public class MyRewardVector3
{
    public float x;
    public float y;
    public float z;

    public MyRewardVector3(float x, float y, float z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public MyRewardVector3(Vector3 v)
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

public class Reward : MonoBehaviour
{
    private Vector3 initialPosition; // Store the initial position of the sphere
    private bool shouldResetPosition_Reward = false; // Flag to indicate if position reset is requested
    private RpcService rpcService;

    void Start()
    {
        initialPosition = transform.position; // Store the initial position
        rpcService = new RpcService(this);
    }

    void Update()
    {
        if (shouldResetPosition_Reward)
        {
            ResetPosition_Reward();
            shouldResetPosition_Reward = false;
        }
    }

    public void ResetPosition_Reward()
    {
        transform.position = initialPosition; // Reset the position to the initial position
    }

    public class RpcService : JsonRpcService
    {
        private Reward reward;

        public RpcService(Reward reward)
        {
            this.reward = reward;
        }

        [JsonRpcMethod]
        public MyRewardVector3 GetPosition_Reward()
        {
            return new MyRewardVector3(reward.transform.position);
        }

        [JsonRpcMethod]
        public void ResetPosition_Reward()
        {
            reward.ResetPosition_Reward(); // Reset the position to the initial position
        }

        [JsonRpcMethod]
        public int RewardPlayerCollisionDetected()
        {
            return reward.RewardPlayerCollisionDetected;
        }
    }

    private int RewardPlayerCollisionDetected = 0;
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            RewardPlayerCollisionDetected = 1;
        }
    }
}
