using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Sazboom.WarRoom;


namespace Sazboom
{
    namespace WarRoom
    {
        [RequireComponent(typeof(NetworkLogger))]
        [RequireComponent(typeof(PlayerModel))]
        [RequireComponent(typeof(ServerPlayerColors))]
        [RequireComponent(typeof(ServerPlayerFamily))]
        [RequireComponent(typeof(PlayerSelections))]
        [RequireComponent(typeof(PlayerFamily))]
        [RequireComponent(typeof(PlayerWaypoints))]
        [RequireComponent(typeof(PlayerPathways))]

        public class PlayerController : NetworkBehaviour
        {
            bool debug = true;
            [SerializeField] private NetworkLogger logger;
            [SerializeField] private PlayerModel playerModel;
            [SerializeField] private ServerPlayerColors serverColor;
            [SerializeField] private ServerPlayerNames serverName;
            [SerializeField] private ServerPlayerFamily serverFamily;
            [SerializeField] private PlayerSelections playerSelections;
            [SerializeField] private PlayerFamily playerFamily;
            
            [SerializeField] public GameObject waypoint;
            
            [SyncVar(hook = nameof(OnWaypointChanged))] private uint _currentWaypointId;

            [SerializeField] private GameObject _currentWaypoint;

            public GameObject CurrentWaypoint { get { return _currentWaypoint; } }
            
            [SyncVar] private bool _hasCurrentWaypoint = false;
            
            public bool HasCurrentWaypoint { get { return _hasCurrentWaypoint; } }
            
            [SerializeField] public GameObject pathway;

            [SyncVar(hook = nameof(OnPathwayChanged))] private uint _currentPathwayId;
           
            [SerializeField] private GameObject _currentPathway;
            
            public GameObject CurrentPathway { get { return _currentPathway; } }
            
            [SyncVar] private bool _hasCurrentPathway = false;
            
            public bool HasCurrentPathway { get { return _hasCurrentPathway; } }

            [SerializeField] private PlayerWaypoints playerWaypoints;
            [SerializeField] private PlayerPathways playerPathways;

            #region Callbacks

            void OnValidate()
            {
                if (logger == null)
                    logger = GetComponent<NetworkLogger>();
                if (playerModel == null)
                    playerModel = GetComponent<PlayerModel>();
                if (playerSelections == null)
                    playerSelections = GetComponent<PlayerSelections>();
                if (serverColor == null)
                    serverColor = GameObject.Find("Server").GetComponent<ServerPlayerColors>();
                if (serverName == null)
                    serverName = GameObject.Find("Server").GetComponent<ServerPlayerNames>();
                if (playerFamily == null)
                    playerFamily = GameObject.Find("Server").GetComponent<PlayerFamily>();
                if (playerWaypoints == null)
                    playerWaypoints = GetComponent<PlayerWaypoints>();
                if (playerPathways == null)
                    playerPathways = GetComponent<PlayerPathways>();
            }

            public override void OnStartClient()
            {
                base.OnStartClient();
                if (hasAuthority)
                {
                    CmdInitPlayer();
                }
            }

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
                string color = serverColor.GetAssignedColor(netId);
                string name = serverName.GetAssignedPlayerName(netId);
                TargetInitPlayer(conn, color, name);

            }

            #endregion

            #region Instace Methods

            public void CallCmdGrantAuthority(GameObject target)
            {
                CmdGrantAuthority(target);
            }

            public void CallCmdAddWaypoint(Vector3 point)
            {
                CmdAddWaypoint(point);
            }

            public void CallCmdAddPathway(Vector3 origin, Vector3 dest)
            {
                CmdAddPathway(origin, dest);
            }

            public void CallCmdRemoveCurrentWaypoint()
            {
                CmdRemoveCurrentWaypoint();
            }

            public void CallCmdRemoveCurrentPathway()
            {
                CmdRemoveCurrentPathway();
            }

            public void CallCmdChangeFocusToPlayer()
            {

            }

            public void CallCmdChangeFocusForwardInFamily()
            {

            }
            public void CallCmdChangeFocusBackwardInFamily()
            {

            }

            void OnWaypointChanged(uint oldId, uint newId)
            {
                _currentWaypoint = null;
                if (NetworkIdentity.spawned.TryGetValue(newId, out NetworkIdentity identity))
                    _currentWaypoint = identity.gameObject;
                else
                    StartCoroutine(SetWaypoint());
            }

            IEnumerator SetWaypoint()
            {
                while (_currentWaypoint == null)
                {
                    yield return null;
                    if (NetworkIdentity.spawned.TryGetValue(_currentWaypointId, out NetworkIdentity identity))
                        _currentWaypoint = identity.gameObject;
                }
            }

            void OnPathwayChanged(uint _, uint newValue)
            {
                _currentPathway = null;
                if (NetworkIdentity.spawned.TryGetValue(_currentPathwayId, out NetworkIdentity identity))
                    _currentPathway = identity.gameObject;
                else
                    StartCoroutine(SetPathway());
            }

            IEnumerator SetPathway()
            {
                while (_currentPathway == null)
                {
                    yield return null;
                    if (NetworkIdentity.spawned.TryGetValue(_currentPathwayId, out NetworkIdentity identity))
                        _currentPathway = identity.gameObject;
                }
            }

            #endregion

            #region Commands

            [Mirror.Command]
            void CmdInitPlayer()
            {
                if (debug) logger.TLog(this.GetType().Name, "CmdInitializePlayer");
                uint netId = netIdentity.netId;
                string color = serverColor.GetAssignedColor(netId);
                string name = serverName.GetAssignedPlayerName(netId); 
                playerModel.ChangeColor(color);
                playerModel.ChangeName(name);
                RpcInitPlayer(color, name);
            }

            [Mirror.Command]
            void CmdGrantAuthority(GameObject target)
            {

                target.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
                uint id = netIdentity.netId;
                string color = serverColor.GetAssignedColor(id);
                serverFamily.AddToFamily(id, target);
                playerModel.HideSoul();
                playerSelections.MergePlayerAndTarget(target, color);
                RpcMergePlayerWithTarget(target, color);
                TargetAddToFamily(target);
            }

            [Mirror.Command]
            void CmdAddWaypoint(Vector3 point)
            {
                
                uint netId = netIdentity.netId;
                string color = serverColor.GetAssignedColor(netId);
                GameObject wp = Instantiate(waypoint, point, Quaternion.identity);
                playerWaypoints.ChangeColor(wp, color);
                _currentWaypoint = wp;
                _currentWaypointId = wp.GetComponent<NetworkIdentity>().netId;
                _hasCurrentWaypoint = true;
                NetworkServer.Spawn(wp, connectionToClient);
                RpcAddWaypoint(wp, color);
            }

            [Mirror.Command]
            void CmdAddPathway(Vector3 origin, Vector3 dest)
            {
                uint netId = netIdentity.netId;
                string color = serverColor.GetAssignedColor(netId);
                GameObject pw = Instantiate(pathway, origin, Quaternion.identity);
                playerPathways.SetEndPoints(pw, origin, dest);
                playerPathways.ChangeColor(pw, color);
                _currentPathway = pw;
                _currentPathwayId = pw.GetComponent<NetworkIdentity>().netId;
                _hasCurrentPathway = true;
                NetworkServer.Spawn(pw, connectionToClient);
                RpcAddPathway(pw, color, origin, dest);
            }

            [Mirror.Command]
            void CmdRemoveCurrentWaypoint()
            {
                NetworkServer.Destroy(_currentWaypoint);
                _currentWaypoint = null;
                _hasCurrentWaypoint = false;
            }

            [Mirror.Command]
            void CmdRemoveCurrentPathway()
            {
                NetworkServer.Destroy(_currentPathway);
                _currentPathway = null;
                _hasCurrentPathway = false;
            }

            [Mirror.Command]
            void CmdChangeFocusToPlayer()
            {

            }

            [Mirror.Command]
            void CmdChangeFocusForwardInFamily()
            {

            }

            [Mirror.Command]
            void CmdChangeFocusBackwardInFamily()
            {

            }

            #endregion

            #region RPCs

            //Client RPC
            [ClientRpc]
            void RpcInitPlayer(string color, string name)
            {
                if (debug) logger.TLog(this.GetType().Name, "RpcChangeColor|" + color);
                playerModel.ChangeColor(color);
                playerModel.ChangeName(name);
            }
            
            [ClientRpc]
            void RpcMergePlayerWithTarget(GameObject target, string color)
            {
                if (debug) logger.TLog(this.GetType().Name, "TargetMergePlayerWithTarget|" + color);
                playerModel.HideSoul();
                playerSelections.MergePlayerAndTarget(target, color);
            }

            [ClientRpc]
            void RpcAddWaypoint(GameObject wp, string color)
            {
                playerWaypoints.ChangeColor(wp, color);
            }

            [ClientRpc]
            void RpcAddPathway(GameObject pw, string color, Vector3 origin, Vector3 dest)
            {
                playerPathways.SetEndPoints(pw, origin, dest);
                playerPathways.ChangeColor(pw, color);
            }

            //Target RPCs
            [TargetRpc]
            void TargetInitPlayer(NetworkConnection conn, string color, string name)
            {
                if (debug) logger.TLog(this.GetType().Name, "TargetChangeColor|" + color);
                playerModel.ChangeColor(color);
                playerModel.ChangeName(name);
            }

            [TargetRpc]
            void TargetAddToFamily(GameObject target)
            {
                playerFamily.AddToFamiliy(target);
            }



            #endregion


        }
    }

}

