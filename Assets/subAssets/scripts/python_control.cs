using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class Python_control : MonoBehaviour
{
    PrometeoCarController PrometeoCarController;
    RpcService rpc;

    // Start is called before the first frame update
    void Start()
    {
        PrometeoCarController = GetComponent<PrometeoCarController>();
        rpc = new RpcService(this);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Custom RPC service class for controls
    public class RpcService : JsonRpcService
    {
        private Python_control controller;

        public RpcService(Python_control controller)
        {
            this.controller = controller;
        }

        [JsonRpcMethod]
        public void GoForward()
        {
            controller.PrometeoCarController.GoForward();
        }

        [JsonRpcMethod]
        public void GoReverse()
        {
            controller.PrometeoCarController.GoReverse();
        }

        [JsonRpcMethod]
        public void TurnLeft()
        {
            controller.PrometeoCarController.TurnLeft();
        }

        [JsonRpcMethod]
        public void TurnRight()
        {
            controller.PrometeoCarController.TurnRight();
        }

        [JsonRpcMethod]

        public void ResetSteeringAngle()
        {
            controller.PrometeoCarController.ResetSteeringAngle();
        }

        [JsonRpcMethod]
        public void ThrottleOff()
        {
            controller.PrometeoCarController.ThrottleOff();
        }



        [JsonRpcMethod]

        public void Handbrake()
        {
            controller.PrometeoCarController.Handbrake();
        }


        [JsonRpcMethod]

        public void RecoverTraction()
        {
            controller.PrometeoCarController.RecoverTraction();
        }


        [JsonRpcMethod]
        public float CarSpeedUI()
        {
            controller.PrometeoCarController.CarSpeedUI();
            float speed = controller.PrometeoCarController.carSpeed; // Assuming 'carSpeed' is a public variable in the 'PrometeoCarController' class
            // print(speed);
            return speed;
        }

        

    }
}
