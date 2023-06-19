using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Thumb_mcp : MonoBehaviour
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


    private void SetPosition_thumb_mcp(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Thumb_mcp thumb_mcp;

        public RpcService(Thumb_mcp thumb_mcp)
        {
            this.thumb_mcp = thumb_mcp;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_thumb_mcp(float x, float y, float z)
        {
            thumb_mcp.SetPosition_thumb_mcp(x, y,z);
        }
    }    
}
