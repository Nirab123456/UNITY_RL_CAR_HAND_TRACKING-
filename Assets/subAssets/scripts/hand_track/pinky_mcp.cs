using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Pinky_mcp : MonoBehaviour
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


    private void SetPosition_pinky_mcp(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Pinky_mcp pinky_mcp;

        public RpcService(Pinky_mcp pinky_mcp)
        {
            this.pinky_mcp = pinky_mcp;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_pinky_mcp(float x, float y, float z)
        {
            pinky_mcp.SetPosition_pinky_mcp(x, y,z);
        }
    }    
}
