using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LoggerHelper : NetworkBehaviour
{
  
    public static string DebugHeader(GameObject obj, string className)
    {
        NetworkIdentity id = obj.GetComponent<NetworkIdentity>();
        return obj.name + "|" + id.netId + "|" + id.GetInstanceID() + "|" + id.sceneId + "|" + className +"|";
    }
}
