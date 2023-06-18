using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Middle_tip : MonoBehaviour
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


    private void SetPosition_middle_tip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Middle_tip middle_tip;

        public RpcService(Middle_tip middle_tip)
        {
            this.middle_tip = middle_tip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_middle_tip(float x, float y, float z)
        {
            middle_tip.SetPosition_middle_tip(x, y, z);
        }
    }    
}
