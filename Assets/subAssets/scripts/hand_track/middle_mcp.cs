using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Middle_mcp : MonoBehaviour
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


    private void SetPosition_middle_mcp(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Middle_mcp middle_mcp;

        public RpcService(Middle_mcp middle_mcp)
        {
            this.middle_mcp = middle_mcp;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_middle_mcp(float x, float y, float z)
        {
            middle_mcp.SetPosition_middle_mcp(x, y,z);
        }
    }    
}
