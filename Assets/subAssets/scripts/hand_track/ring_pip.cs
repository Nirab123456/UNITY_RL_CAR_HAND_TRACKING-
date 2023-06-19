using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Ring_pip : MonoBehaviour
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


    private void SetPosition_ring_pip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Ring_pip ring_pip;

        public RpcService(Ring_pip ring_pip)
        {
            this.ring_pip = ring_pip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_ring_pip(float x, float y, float z)
        {
            ring_pip.SetPosition_ring_pip(x, y,z);
        }
    }    
}
