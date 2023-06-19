using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Thumb_tip : MonoBehaviour
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


    private void SetPosition_thumb_tip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Thumb_tip thumb_tip;

        public RpcService(Thumb_tip thumb_tip)
        {
            this.thumb_tip = thumb_tip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_thumb_tip(float x, float y, float z)
        {
            thumb_tip.SetPosition_thumb_tip(x, y,z);
        }
    }    
}
