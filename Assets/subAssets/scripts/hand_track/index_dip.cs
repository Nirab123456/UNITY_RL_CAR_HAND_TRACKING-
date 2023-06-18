using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Index_dip : MonoBehaviour
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


    private void SetPosition_index_dip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Index_dip index_dip;

        public RpcService(Index_dip index_dip)
        {
            this.index_dip = index_dip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_index_dip(float x, float y, float z)
        {
            index_dip.SetPosition_index_dip(x, y, z);
        }
    }    
}
