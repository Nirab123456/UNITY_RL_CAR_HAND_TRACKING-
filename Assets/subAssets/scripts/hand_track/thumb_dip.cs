using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Thumb_dip  : MonoBehaviour
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


    private void SetPosition_thumb_dip (float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Thumb_dip  thumb_dip ;

        public RpcService(Thumb_dip  thumb_dip )
        {
            this.thumb_dip  = thumb_dip ;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_thumb_dip (float x, float y, float z)
        {
            thumb_dip .SetPosition_thumb_dip (x, y,z);
        }
    }    
}
