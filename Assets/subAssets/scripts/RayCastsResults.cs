using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AustinHarris.JsonRpc;

public class RayCastsResults : MonoBehaviour
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
        private RayCastsResults controller;

        public RpcService(RayCastsResults controller)
        {
            this.controller = controller;
        }

        [JsonRpcMethod]
        public RayResults GetRayCastsResults()
        {
            return controller.GetRayCastsResults();
        }

        [JsonRpcMethod]
        public void SomeMethod()
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
