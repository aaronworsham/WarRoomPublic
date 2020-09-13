using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Sazboom.WarRoom;
using System;

namespace Sazboom.WarRoom
{


           

        public class PlayerController : NetworkBehaviour, IPlayerControllable
        {

        [SerializeField] private IPlayerViewable iplayerView;
        [SerializeField] private IPlayerModelable iplayerModel;
        [SerializeField] private IPlayerActionable iplayerAction;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private GameObject tokenInstance;

        
        //iplayerModel gets called by the server's TargetInitPlayer call for AddNewPlayer BEFORE Start() on the player prefabs are called.  
        //So it has to be Awake() for the init.

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
            iplayerModel = GetComponent<IPlayerModelable>();
            iplayerView = GetComponent<IPlayerViewable>();
            iplayerAction = GetComponent<IPlayerActionable>();
            ISceneControllable sceneController = GameObject.Find("Scene").GetComponent<ISceneControllable>();
            sceneController.RegisterPlayerController(this);
        }

        public void GetToken()
        {
            if (!isLocalPlayer) return;
            CmdInitPlayer(
                iplayerModel.SelectedTokenString,
                iplayerModel.SelectedColorString,
                iplayerModel.NameEntered
                );
            characterController.enabled = true;
            iplayerAction.TokenIsReady();
        }
        public void SetGM()
        {
            iplayerAction.ClientMode = PlayerActions.ClientModes.GM;
            iplayerAction.GMIsReady();
        }


        //    bool debug = true;
        //    [SerializeField] private NetworkLogger logger;
        //    [SerializeField] private PlayerModel playerModel;
        //    [SerializeField] private ServerPlayerColors serverColor;
        //    [SerializeField] private ServerPlayerNames serverName;
        //    [SerializeField] private ServerPlayerFamily serverFamily;
        //    [SerializeField] private PlayerSelections playerSelections;
        //    [SerializeField] private PlayerFamily playerFamily;
        //    [SerializeField] private ISceneManagable sceneManager;

        //    [SerializeField] public GameObject waypoint;

        //    [SyncVar(hook = nameof(OnWaypointChanged))] private uint _currentWaypointId;

        //    [SerializeField] private GameObject _currentWaypoint;

        //    public GameObject CurrentWaypoint { get { return _currentWaypoint; } }

        //    [SyncVar] private bool _hasCurrentWaypoint = false;

        //    public bool HasCurrentWaypoint { get { return _hasCurrentWaypoint; } }

        //    [SerializeField] public GameObject pathway;

        //    [SyncVar(hook = nameof(OnPathwayChanged))] private uint _currentPathwayId;

        //    [SerializeField] private GameObject _currentPathway;

        //    public GameObject CurrentPathway { get { return _currentPathway; } }

        //    [SyncVar] private bool _hasCurrentPathway = false;

        //    public bool HasCurrentPathway { get { return _hasCurrentPathway; } }

        //    [SerializeField] private PlayerWaypoints playerWaypoints;
        //    [SerializeField] private PlayerPathways playerPathways;

        //    #region Callbacks

        //    public static Action OnLocalPlayerReady;

        //    void OnValidate()
        //    {
        //        if (logger == null)
        //            logger = GetComponent<NetworkLogger>();
        //        if (playerModel == null)
        //            playerModel = GetComponent<PlayerModel>();
        //        if (playerSelections == null)
        //            playerSelections = GetComponent<PlayerSelections>();
        //        if (serverColor == null)
        //            serverColor = GameObject.Find("Server").GetComponent<ServerPlayerColors>();
        //        if (serverName == null)
        //            serverName = GameObject.Find("Server").GetComponent<ServerPlayerNames>();
        //        if (playerFamily == null)
        //            playerFamily = GameObject.Find("Server").GetComponent<PlayerFamily>();
        //        if (playerWaypoints == null)
        //            playerWaypoints = GetComponent<PlayerWaypoints>();
        //        if (playerPathways == null)
        //            playerPathways = GetComponent<PlayerPathways>();
        //        if (sceneManager == null)
        //            sceneManager = GameObject.Find("Scene").GetComponent<ISceneManagable>();
        //    }

        //    private void Awake()
        //    {
        //        sceneManager.OnReadyForPlayerInstance += HandleReadyForPlayerInstance;
        //    }

        //    private void OnDestroy()
        //    {
        //        sceneManager.OnReadyForPlayerInstance -= HandleReadyForPlayerInstance;
        //    }

        //    public override void OnStartClient()
        //    {
        //        base.OnStartClient();
        //        Debug.Log("OnStartClient");
        //    }

        //    public override void OnStartLocalPlayer()
        //    {
        //        base.OnStartLocalPlayer();
        //        OnLocalPlayerReady?.Invoke();
        //    }

        //    void HandleReadyForPlayerInstance()
        //    {
        //        string tokenString = playerModel.SelectedTokenString;
        //        string colorString = playerModel.SelectedColorString;
        //        string name = playerModel.NameEntered;
        //        
        //    }

        #region Server Callbacks

        public override void OnStartServer()
        {
            base.OnStartServer();
            WRNetworkManager.RelayOnServerAddPlayer += HandleAddPlayer;
        }
        public override void OnStopServer()
        {
            base.OnStopServer();
            WRNetworkManager.RelayOnServerAddPlayer -= HandleAddPlayer;

        }

        void HandleAddPlayer(NetworkConnection conn)
        {
            if (conn.connectionId == connectionToClient.connectionId) return;
            
            TargetInitPlayer(conn, tokenInstance, iplayerModel.SelectedTokenString, iplayerModel.SelectedColorString, iplayerModel.NameEntered);
            

        }

        #endregion

        //    #endregion

        //    #region Instace Methods

        //    public void CallCmdGrantAuthority(GameObject target)
        //    {
        //        CmdGrantAuthority(target);
        //    }

        //    public void CallCmdAddWaypoint(Vector3 point)
        //    {
        //        CmdAddWaypoint(point);
        //    }

        //    public void CallCmdAddPathway(Vector3 origin, Vector3 dest)
        //    {
        //        CmdAddPathway(origin, dest);
        //    }

        //    public void CallCmdRemoveCurrentWaypoint()
        //    {
        //        CmdRemoveCurrentWaypoint();
        //    }

        //    public void CallCmdRemoveCurrentPathway()
        //    {
        //        CmdRemoveCurrentPathway();
        //    }

        //    public void CallCmdChangeFocusToPlayer()
        //    {

        //    }

        //    public void CallCmdChangeFocusForwardInFamily()
        //    {

        //    }
        //    public void CallCmdChangeFocusBackwardInFamily()
        //    {

        //    }

        //    void OnWaypointChanged(uint oldId, uint newId)
        //    {
        //        _currentWaypoint = null;
        //        if (NetworkIdentity.spawned.TryGetValue(newId, out NetworkIdentity identity))
        //            _currentWaypoint = identity.gameObject;
        //        else
        //            StartCoroutine(SetWaypoint());
        //    }

        //    IEnumerator SetWaypoint()
        //    {
        //        while (_currentWaypoint == null)
        //        {
        //            yield return null;
        //            if (NetworkIdentity.spawned.TryGetValue(_currentWaypointId, out NetworkIdentity identity))
        //                _currentWaypoint = identity.gameObject;
        //        }
        //    }

        //    void OnPathwayChanged(uint _, uint newValue)
        //    {
        //        _currentPathway = null;
        //        if (NetworkIdentity.spawned.TryGetValue(_currentPathwayId, out NetworkIdentity identity))
        //            _currentPathway = identity.gameObject;
        //        else
        //            StartCoroutine(SetPathway());
        //    }

        //    IEnumerator SetPathway()
        //    {
        //        while (_currentPathway == null)
        //        {
        //            yield return null;
        //            if (NetworkIdentity.spawned.TryGetValue(_currentPathwayId, out NetworkIdentity identity))
        //                _currentPathway = identity.gameObject;
        //        }
        //    }

        //    #endregion

        //    #region Commands

        [Mirror.Command]
        void CmdInitPlayer(string tokenString, string colorString, string name)
        {
            uint netId = netIdentity.netId;
            iplayerModel.SelectedTokenString = tokenString;
            iplayerModel.SelectedColorString = colorString;
            iplayerModel.NameEntered = name;

            tokenInstance = Instantiate(iplayerModel.SelectedToken, transform.position, Quaternion.identity);
            NetworkServer.Spawn(tokenInstance, connectionToClient);
            tokenInstance.transform.SetParent(transform);

            tokenInstance.transform.Find("Base").GetComponent<MeshRenderer>().material = iplayerModel.SelectedDefaultMaterial;


            ////Set the token under the player hierarchy.  Also set the base color
            //playerSelections.MergePlayerAndTarget(tokenInstance);

            ////Tell all the clients to move the token under the player hierarchy and change the color
            RpcInitPlayer(tokenInstance, tokenString, colorString, name);
        }



        //    [Mirror.Command]
        //    void CmdGrantAuthority(GameObject target)
        //    {

        //        target.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
        //        uint id = netIdentity.netId;
        //        string color = serverColor.GetAssignedColor(id);
        //        serverFamily.AddToFamily(id, target);
        //        playerSelections.MergePlayerAndTarget(target);
        //        RpcMergePlayerWithTarget(target);
        //        TargetAddToFamily(target);
        //    }

        //    [Mirror.Command]
        //    void CmdAddWaypoint(Vector3 point)
        //    {

        //        uint netId = netIdentity.netId;
        //        string color = serverColor.GetAssignedColor(netId);
        //        GameObject wp = Instantiate(waypoint, point, Quaternion.identity);
        //        playerWaypoints.ChangeColor(wp, color);
        //        _currentWaypoint = wp;
        //        _currentWaypointId = wp.GetComponent<NetworkIdentity>().netId;
        //        _hasCurrentWaypoint = true;
        //        NetworkServer.Spawn(wp, connectionToClient);
        //        RpcAddWaypoint(wp, color);
        //    }

        //    [Mirror.Command]
        //    void CmdAddPathway(Vector3 origin, Vector3 dest)
        //    {
        //        uint netId = netIdentity.netId;
        //        string color = serverColor.GetAssignedColor(netId);
        //        GameObject pw = Instantiate(pathway, origin, Quaternion.identity);
        //        playerPathways.SetEndPoints(pw, origin, dest);
        //        playerPathways.ChangeColor(pw, color);
        //        _currentPathway = pw;
        //        _currentPathwayId = pw.GetComponent<NetworkIdentity>().netId;
        //        _hasCurrentPathway = true;
        //        NetworkServer.Spawn(pw, connectionToClient);
        //        RpcAddPathway(pw, color, origin, dest);
        //    }

        //    [Mirror.Command]
        //    void CmdRemoveCurrentWaypoint()
        //    {
        //        NetworkServer.Destroy(_currentWaypoint);
        //        _currentWaypoint = null;
        //        _hasCurrentWaypoint = false;
        //    }

        //    [Mirror.Command]
        //    void CmdRemoveCurrentPathway()
        //    {
        //        NetworkServer.Destroy(_currentPathway);
        //        _currentPathway = null;
        //        _hasCurrentPathway = false;
        //    }

        //    [Mirror.Command]
        //    void CmdChangeFocusToPlayer()
        //    {

        //    }

        //    [Mirror.Command]
        //    void CmdChangeFocusForwardInFamily()
        //    {

        //    }

        //    [Mirror.Command]
        //    void CmdChangeFocusBackwardInFamily()
        //    {

        //    }

        //    #endregion

        //    #region RPCs

        //Client RPC
        [ClientRpc]
        void RpcInitPlayer(GameObject tokenInstance, string tokenString, string colorString, string name)
        {

            iplayerModel.SelectedTokenString = tokenString;
            iplayerModel.SelectedColorString = colorString;
            iplayerModel.NameEntered = name;

            tokenInstance.transform.SetParent(transform);
            tokenInstance.transform.Find("Base").GetComponent<MeshRenderer>().material = iplayerModel.SelectedDefaultMaterial;
        }

        //    [ClientRpc]
        //    void RpcMergePlayerWithTarget(GameObject target)
        //    {
        //        playerSelections.MergePlayerAndTarget(target);
        //    }

        //    [ClientRpc]
        //    void RpcAddWaypoint(GameObject wp, string color)
        //    {
        //        playerWaypoints.ChangeColor(wp, color);
        //    }

        //    [ClientRpc]
        //    void RpcAddPathway(GameObject pw, string color, Vector3 origin, Vector3 dest)
        //    {
        //        playerPathways.SetEndPoints(pw, origin, dest);
        //        playerPathways.ChangeColor(pw, color);
        //    }


        //Target RPCs
        [TargetRpc]
        void TargetInitPlayer(NetworkConnection conn, GameObject tokenInstance, string tokenString, string colorString, string name)
        {
            Debug.Log("PlayerController|TargetInitPlayer");
            if (isLocalPlayer) return;
            Debug.Log("PlayerController|TargetInitPlayer|Is Not Local Player");

            iplayerModel.SelectedTokenString = tokenString;
            iplayerModel.SelectedColorString = colorString;
            iplayerModel.NameEntered = name;

            tokenInstance.transform.SetParent(transform);
            tokenInstance.transform.position = transform.position;
            tokenInstance.transform.Find("Base").GetComponent<MeshRenderer>().material = iplayerModel.SelectedDefaultMaterial;
        }

        //    [TargetRpc]
        //    void TargetAddToFamily(GameObject target)
        //    {
        //        playerFamily.AddToFamiliy(target);
        //    }



        //    #endregion


    }
    

}

