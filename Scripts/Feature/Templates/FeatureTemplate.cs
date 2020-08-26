using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Template : NetworkBehaviour
{
    //Debugger
    bool debug = false;
    public NetworkLogger logger;

    //SyncVar
    [SyncVar(hook = nameof(CallFromSvar))]
    public string str = "string";

    //Event Called when SyncVar changes
    public delegate void TemplateDelegate(string str);
    public event TemplateDelegate EventName;

    #region CommandBar Call [RUN ON CLIENT]

    void commandbarcall(string str) {
        CmdCall(str);

    }

    #endregion

    #region  Client Callbacks [RUN ON CLIENT]
    public void Awake()
    {
        logger = GetComponent<NetworkLogger>();
        if (debug) logger.TLog(this.GetType().Name, "template");
    }

    [Mirror.Client]
    public void CallFromSvar(string oldStr, string newStr)
    {
        //Do Logic
        EventName?.Invoke(newStr);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }

    public override void OnStopAuthority()
    {
        base.OnStopAuthority();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }

    #endregion

    #region Server Callbacks [RUN ON SERVER]
    //Server callbacks when a client joins, used for late join catchup state

    [Mirror.Server]
    public override void OnStartServer()
    {
        base.OnStartServer();
        WRNetworkManager.RelayOnServerAddPlayer += WRNetworkManager_RelayOnServerAddPlayer;
    }

    [Mirror.Server]    
    public override void OnStopServer()
    {
        base.OnStopServer();
        WRNetworkManager.RelayOnServerAddPlayer -= WRNetworkManager_RelayOnServerAddPlayer;
    }

    [Mirror.Server]
    private void WRNetworkManager_RelayOnServerAddPlayer(NetworkConnection conn)
    {
        TargetCall(conn, "arg1");
    }
    #endregion

    #region Commands [RUN ON SERVER]

    [Mirror.Command]
    //The change of the SyncVar causes current clients to update as well.
    public void CmdCall(string str)
    {
        //Do Logic
        EventName?.Invoke(str);
    }
    #endregion

    #region Target [RUN ON CLIENT]
    //This is run to catch up late join clients
    [Mirror.TargetRpc]
    public void TargetCall(NetworkConnection conn, string str)
    {
    }
    #endregion



    #region Instance Methods [RUN ON BOTH]


    #endregion

}

