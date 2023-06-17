using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class RayCast6000 : MonoBehaviour
{
    private RayCasts rayCastsScript;
    RpcService rpc;

    private void Start()
    {
        rayCastsScript = GetComponent<RayCasts>();
        rpc = new RpcService(this);
    }

    public class RpcService : JsonRpcService
    {
        private RayCast6000 controller;

        public RpcService(RayCast6000 controller)
        {
            this.controller = controller;
        }

        [JsonRpcMethod]
        public RayResults GetRayCastsResults_6000()
        {
            return controller.GetRayCastsResults();
        }

        [JsonRpcMethod]
        public void SomeMethod_6000()
        {
            controller.SomeMethod();
        }
    }

    private RayResults GetRayCastsResults()
    {
        return rayCastsScript.GetObservation();
    }

    private void SomeMethod()
    {
        RayResults rayResults = rayCastsScript.GetObservation();

        Debug.Log("Number of Object Types: " + rayResults.NumObjectTypes);

        foreach (var distances in rayResults.rayDistances)
        {
            foreach (var distance in distances)
            {
                Debug.Log("Distance: " + distance);
            }
        }

        foreach (var objectTypes in rayResults.rayHitObjectTypes)
        {
            foreach (var objectType in objectTypes)
            {
                Debug.Log("Object Type: " + objectType);
            }
        }
    }
}
