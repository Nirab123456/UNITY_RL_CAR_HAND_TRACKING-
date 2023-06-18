using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Index_mcp : MonoBehaviour
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


    private void SetPosition_index_mcp(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Index_mcp index_mcp;

        public RpcService(Index_mcp index_mcp)
        {
            this.index_mcp = index_mcp;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_index_mcp(float x, float y, float z)
        {
            index_mcp.SetPosition_index_mcp(x, y, z);
        }
    }    
}
