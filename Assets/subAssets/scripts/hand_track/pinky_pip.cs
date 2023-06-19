using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Pinky_pip : MonoBehaviour
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


    private void SetPosition_pinky_pip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Pinky_pip pinky_pip;

        public RpcService(Pinky_pip pinky_pip)
        {
            this.pinky_pip = pinky_pip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_pinky_pip(float x, float y, float z)
        {
            pinky_pip.SetPosition_pinky_pip(x, y,z);
        }
    }    
}
