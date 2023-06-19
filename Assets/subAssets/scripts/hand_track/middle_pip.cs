using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Middle_pip : MonoBehaviour
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


    private void SetPosition_middle_pip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Middle_pip middle_pip;

        public RpcService(Middle_pip middle_pip)
        {
            this.middle_pip = middle_pip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_middle_pip(float x, float y, float z)
        {
            middle_pip.SetPosition_middle_pip(x, y,z);
        }
    }    
}
