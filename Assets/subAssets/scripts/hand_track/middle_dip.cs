using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Middle_dip : MonoBehaviour
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


    private void SetPosition_middle_dip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Middle_dip middle_dip;

        public RpcService(Middle_dip middle_dip)
        {
            this.middle_dip = middle_dip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_middle_dip(float x, float y, float z)
        {
            middle_dip.SetPosition_middle_dip(x, y,z);
        }
    }    
}
