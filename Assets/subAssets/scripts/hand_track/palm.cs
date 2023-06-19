using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Palm : MonoBehaviour
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


    private void SetPosition_palm(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Palm palm;

        public RpcService(Palm palm)
        {
            this.palm = palm;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_palm(float x, float y, float z)
        {
            palm.SetPosition_palm(x, y,z);
        }
    }    
}
