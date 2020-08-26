using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;

public class NetworkLogger : MonoBehaviour
{

    int index = 0;

    public void NLog(string str)
    {
        Debug.Log(str);
    }

    public void TLog(string className, string log)
    {
        Debug.Log(DebugHeader(className));
        Debug.Log("==> " + log + "\n---\n");
        index++;
    }

    public void WLog(string className, string log)
    {
        Debug.LogWarning(DebugHeader(className));
        Debug.LogWarning("==> " + log + "\n---\n");
        index++;
    }

    public string EarlyDebugHeader(string className)
    {
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        str.Append("[E]|");
        str.Append(index.ToString());
        str.Append("|Instance: " + gameObject.GetInstanceID());
        str.Append("|Class: " + className);
        return str.ToString();
    }

    public string DebugHeader(string className)
    {
        NetworkIdentity id = gameObject.GetComponent<NetworkIdentity>();
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        str.Append(index.ToString());
        str.Append("|Instance: " + gameObject.GetInstanceID());
        str.Append("|NetID: " + id.netId);
        str.Append("|Class: " + className);

        return str.ToString();
    }

}
