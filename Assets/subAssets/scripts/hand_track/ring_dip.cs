using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Ring_dip : MonoBehaviour
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


    private void SetPosition_ring_dip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Ring_dip ring_dip;

        public RpcService(Ring_dip ring_dip)
        {
            this.ring_dip = ring_dip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_ring_dip(float x, float y, float z)
        {
            ring_dip.SetPosition_ring_dip(x, y,z);
        }
    }    
}
