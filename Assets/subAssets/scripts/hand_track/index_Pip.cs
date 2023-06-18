using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Index_pip : MonoBehaviour
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


    private void SetPosition_index_pip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Index_pip index_pip;

        public RpcService(Index_pip index_pip)
        {
            this.index_pip = index_pip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_index_pip(float x, float y, float z)
        {
            index_pip.SetPosition_index_pip(x, y, z);
        }
    }    
}
