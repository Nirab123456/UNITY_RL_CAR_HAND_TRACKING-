using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Index_tip : MonoBehaviour
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


    private void SetPosition_index_tip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Index_tip index_tip;

        public RpcService(Index_tip index_tip)
        {
            this.index_tip = index_tip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_index_tip(float x, float y, float z)
        {
            index_tip.SetPosition_index_tip(x, y,z);
        }
    }    
}
