using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;

public class GMController : NetworkBehaviour
{

    public void CallCmdMoveScene(string sceneName)
    {
        CmdMoveNextScene(sceneName);
    }

    [Mirror.Command]
    void CmdMoveNextScene(string sceneName)
    {
        NetworkManager network = GameObject.Find("Network").GetComponent<NetworkManager>();
        network.ServerChangeScene(sceneName);
        //RpcResetLocation();
    }

    [Mirror.ClientRpc]
    void RpcResetLocation()
    {

    }

}
