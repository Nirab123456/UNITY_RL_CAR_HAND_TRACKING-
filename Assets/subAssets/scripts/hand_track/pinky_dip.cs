using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Pinky_dip : MonoBehaviour
{
    private RpcService rpcService; // Declare rpcService variable

    // Start is called before the first frame update
    void Start()
    {
        rpcService = new RpcService(this); // Initialize rpcService        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void SetPosition_pinky_dip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Pinky_dip pinky_dip;

        public RpcService(Pinky_dip pinky_dip)
        {
            this.pinky_dip = pinky_dip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_pinky_dip(float x, float y, float z)
        {
            pinky_dip.SetPosition_pinky_dip(x, y,z);
        }
    }    
}
