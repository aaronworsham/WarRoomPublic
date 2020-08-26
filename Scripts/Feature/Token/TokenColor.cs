//using Mirror;
//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class TokenColor : NetworkBehaviour
//{

//    bool debug = false;
//    string dHeader = "SERVER|TokenColor";

//    Token token;
//    public NetworkLogger logger;



//    #region  Client Callbacks [RUN ON CLIENT]
//    public void Awake()
//    {
//        logger = GetComponent<NetworkLogger>();
//        token = GetComponent<Token>();
//    }

//    public override void OnStartClient()
//    {
//        base.OnStartClient();
//        SelectAToken.OnTokenSelected += SelectAToken_OnTokenSelected; ;
//    }
//    private void SelectAToken_OnTokenSelected(GameObject token, GameObject soul)
//    {
//        if (!hasAuthority) return;
//        if (token.GetInstanceID() != gameObject.GetInstanceID()) return;
//        CmdChangeColor(debug);
//    }

//    public override void OnStopClient()
//    {
//        base.OnStopClient();
//        SelectAToken.OnTokenSelected -= SelectAToken_OnTokenSelected; ;
//    }
    
//    public override void OnStartLocalPlayer()
//    {
//        base.OnStartLocalPlayer();
//        dHeader = logger.DebugHeader(this.GetType().Name);


//    }

//    #endregion

//    #region Server Callbacks [RUN ON SERVER]
//    [Server]
//    public override void OnStartServer()
//    {
//        base.OnStartServer();
//        if (debug) logger.TLog(this.GetType().Name, "OnStartServer|Adding Listener for OnServerAddPlayer");
//        WRNetworkManager.RelayOnServerAddPlayer += HandleRelayOnServerAddPlayer;
//    }
//    [Server]
//    public override void OnStopServer()
//    {
//        base.OnStopServer();
//        WRNetworkManager.RelayOnServerAddPlayer -= HandleRelayOnServerAddPlayer;
//    }
//    [Server]
//    void HandleRelayOnServerAddPlayer(NetworkConnection conn)
//    {
//        if (debug) Debug.Log("SERVER|HandleRelayOnServerAddPlayer");
//        TargetChangeColor(player.color, debug);
//    }
//    #endregion


//    #region Commands

//    [Command]
//    public void CmdChangeColor(bool debug)
//    {
//        if (debug) Debug.Log("SERVER|" + gameObject.name);

//        Color color = token.getColor();
//        ChangeColor(color);
//        RpcChangeColor(color, debug);


//    }


//    #endregion

//    #region RPCs [RUN ON CLIENT]

//    [TargetRpc]
//    public void TargetChangeColor(Color color, bool debug)
//    {
//        if (debug) logger.TLog(this.GetType().Name, "TargetChangeColor|Color: " + color);
//        player.color = color;
//        ChangeSoulColor(color);

//    }

//    [ClientRpc]
//    public void RpcChangeColor(Color color, bool debug)
//    {
//        if (debug) logger.TLog(this.GetType().Name, "RpcChangeColor|" + color);
//        if (isServer) return;
//        player.color = color;
//        ChangeSoulColor(color);
//    }

//    #endregion



//    public MeshRenderer GetBaseMesh()
//    {
//        return gameObject.transform.Find("Base").GetComponent<MeshRenderer>();
//    }

//    public void ChangeColor(Color color, bool debug)
//    {

//        if (debug) logger.TLog(this.GetType().Name, "ChangeColor|Mesh" + color);
//        GetBaseMesh().material.SetColor("_Color", color);

//    }

//}

