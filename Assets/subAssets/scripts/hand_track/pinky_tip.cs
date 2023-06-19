using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Pinky_tip : MonoBehaviour
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


    private void SetPosition_pinky_tip(float x, float y, float z)
    {
        Vector3 newPosition = new Vector3(x, y, z);
        transform.position = newPosition;
    }

    public class RpcService : JsonRpcService
    {
        private Pinky_tip pinky_tip;

        public RpcService(Pinky_tip pinky_tip)
        {
            this.pinky_tip = pinky_tip;
        }
        
        
        [JsonRpcMethod]
        public void SetPosition_pinky_tip(float x, float y, float z)
        {
            pinky_tip.SetPosition_pinky_tip(x, y,z);
        }
    }    
}
