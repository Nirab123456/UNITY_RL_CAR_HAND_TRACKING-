using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Ring_mcp : MonoBehaviour
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


    private void SetPosition_ring_mcp(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Ring_mcp ring_mcp;

        public RpcService(Ring_mcp ring_mcp)
        {
            this.ring_mcp = ring_mcp;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_ring_mcp(float x, float y, float z)
        {
            ring_mcp.SetPosition_ring_mcp(x, y,z);
        }
    }    
}
