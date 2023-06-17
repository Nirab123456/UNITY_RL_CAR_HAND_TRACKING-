using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class RayCast7000 : MonoBehaviour
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
        private RayCast7000 controller;

        public RpcService(RayCast7000 controller)
        {
            this.controller = controller;
        }

        [JsonRpcMethod]
        public RayResults GetRayCastsResults_7000()
        {
            return controller.GetRayCastsResults();
        }

        [JsonRpcMethod]
        public void SomeMethod_7000()
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
