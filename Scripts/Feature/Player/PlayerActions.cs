﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Text;

namespace Sazboom
{
    namespace WarRoom
    {
        [RequireComponent(typeof(CharacterController))]
        [RequireComponent(typeof(NetworkTransform))]
        [RequireComponent(typeof(CapsuleCollider))]
        [RequireComponent(typeof(PlayerSelections))]
        [RequireComponent(typeof(NetworkLogger))]
        [RequireComponent(typeof(PlayerController))]
        [RequireComponent(typeof(PlayerFamily))]
        [RequireComponent(typeof(PlayerDistances))]
        [RequireComponent(typeof(PlayerWaypoints))]
        [RequireComponent(typeof(PlayerPathways))]
        [RequireComponent(typeof(PlayerMovement))]
        public class PlayerActions : NetworkBehaviour
        {
            readonly bool debug = false;
            [SerializeField] private NetworkLogger logger;
            [SerializeField] private CharacterController characterController;
            [SerializeField] private CapsuleCollider capsuleCollider;
            [SerializeField] private PlayerSelections playerSelections;
            [SerializeField] private PlayerController playerController;
            [SerializeField] private PlayerFamily playerFamily;
            [SerializeField] private CameraController cameraController;
            [SerializeField] private PlayerDistances playerDistances;
            [SerializeField] private PlayerWaypoints playerWaypoints;
            [SerializeField] private PlayerPathways playerPathways;
            [SerializeField] private PlayerMovement playerMovement;

            #region Properties

            [Header("Modes")]
            [SerializeField] private bool _targetMode = false;
            [SerializeField] private bool _distanceMode = false;
            [SerializeField] private bool _waypointMode = false;
            [SerializeField] private bool _clickToMoveMode = false;
            [SerializeField] private bool _pathwayMode = false;

            [Header("Cursors")]
            [SerializeField] private Texture2D targetCursor;
            [SerializeField] private Texture2D waypointCursor;
            [SerializeField] private Texture2D stepsCursor;
            [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
            [SerializeField] private Vector2 targetHotspot;
            [SerializeField] private Vector2 waypointHotspot;
            [SerializeField] private Vector2 stepsHotspot;

            private Coroutine clickToMove;
            #endregion



            #region Callbacks
            void OnValidate()
            {
                if (logger == null)
                    logger = GetComponent<NetworkLogger>();
                if (characterController == null)
                    characterController = GetComponent<CharacterController>();
                if (capsuleCollider == null)
                    capsuleCollider = GetComponent<CapsuleCollider>();
                if (playerSelections == null)
                    playerSelections = GetComponent<PlayerSelections>();
                if (playerController == null)
                    playerController = GetComponent<PlayerController>();
                if (playerFamily == null)
                    playerFamily = GetComponent<PlayerFamily>();
                if (cameraController == null)
                    cameraController = GetComponent<CameraController>();
                if (playerDistances == null)
                    playerDistances = GetComponent<PlayerDistances>();
                if (playerWaypoints == null)
                    playerWaypoints = GetComponent<PlayerWaypoints>();
                if (playerPathways == null)
                    playerPathways = GetComponent<PlayerPathways>();
                if (playerMovement == null)
                    playerMovement = GetComponent<PlayerMovement>();
            }

            void Start()
            {
                capsuleCollider.enabled = isServer;
                
            }

            public override void OnStartLocalPlayer()
            {
                characterController.enabled = true;
            }

            void OnDisable()
            {
            }

            void Update()
            {
                if (!isLocalPlayer) return;
                if (!hasAuthority) return;
                if (!characterController.enabled) return;
                
                int instanceId = gameObject.GetInstanceID();

                //Actions available in Forward Camera Mode only
                #region ForwardMode Commands
                if (cameraController.ForwardMode)
                {
                    #region Movement


                    playerMovement.Horizontal = Input.GetAxis("Horizontal");
                    playerMovement.Vertical = Input.GetAxis("Vertical");

                    // Q and E cancel each other out, reducing the turn to zero
                    if (Input.GetKey(KeyCode.Q))
                        playerMovement.RotateLeft();
                    if (Input.GetKey(KeyCode.E))
                        playerMovement.RotateRight();
                    if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
                        playerMovement.CancelOutRotation();
                    if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                        playerMovement.CancelOutRotation();

                    if (playerMovement.IsGrounded)
                        playerMovement.IsFalling = false;

                    if ((playerMovement.IsGrounded || !playerMovement.IsFalling) && 
                        playerMovement.JumpSpeed < 1f && Input.GetKey(KeyCode.Space))
                    {
                        playerMovement.JumpSpeed = Mathf.Lerp(playerMovement.JumpSpeed, 1f, 0.5f);
                    }
                    else if (!playerMovement.IsGrounded)
                    {
                        playerMovement.IsFalling = true;
                        playerMovement.JumpSpeed = 0;
                    }

                    #endregion
                    
                    #region TargetMode

                    if (Input.GetKeyDown(KeyCode.T))
                    {
                        if (_targetMode)
                        {
                            Cursor.SetCursor(null, Vector2.zero, cursorMode);
                            _targetMode = false;
                        }
                        else
                        {
                            Cursor.SetCursor(targetCursor, targetHotspot, cursorMode);
                            _targetMode = true;
                        }
                    }

                    if (_targetMode && Input.GetMouseButtonDown(0))
                    {
                        GameObject target;
                        if (playerSelections.IsPlayerToken(out target))
                        {
                            NetworkIdentity targetId = target.GetComponent<NetworkIdentity>();
                            if (debug) logger.TLog(this.GetType().Name, "Update|Requesting Ownership");
                            if (debug) logger.TLog(this.GetType().Name, "Update|ID: " + targetId);
                            if (debug) logger.TLog(this.GetType().Name, "Update|Has Auth Already? " + targetId.hasAuthority);


                            if (targetId != null && !targetId.hasAuthority)
                            {
                                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                                _targetMode = false;
                                playerController.CallCmdGrantAuthority(target);
                            }

                        }
                    }

                    #endregion  
                
                }
                #endregion


                //Actions available in Overhead Camera Mode Only
                #region OverheadMode Commands
                if (cameraController.OverheadMode)
                {
                    #region Distance
                    if (Input.GetKeyDown(KeyCode.X))
                    {
                        if (!_distanceMode)
                        {
                            UiBottomRight.OnShow();
                            UiBottomRight.OnMessage("Distance Mode");
                            _distanceMode = true;
                        }
                        else
                        {
                            UiBottomRight.OnHide();
                            _distanceMode = false;
                        }
                    }
                    if(_distanceMode)
                    {
                        StringBuilder str = new StringBuilder();
                        str.Append(playerDistances.DistanceToCursor());
                        if (playerController.HasCurrentWaypoint)
                        {
                            Vector3 point = playerController.CurrentWaypoint.transform.position;
                            str.Append(playerDistances.DistanceToWaypoint(point));
                        }
                        UiBottomRight.OnMessage(str.ToString());
                    }
                    #endregion

                    #region Waypoints
                    if (Input.GetKeyDown(KeyCode.Z))
                    {
                        if (_waypointMode)
                        {
                            
                            Cursor.SetCursor(null, Vector2.zero, cursorMode);
                            _waypointMode = false;
                        }
                        else
                        {
                            TurnOffAllOverheadModes();
                            Cursor.SetCursor(waypointCursor, waypointHotspot, cursorMode);
                            _clickToMoveMode = false;
                            _waypointMode = true;
                        }
                    }
                    if (_waypointMode && Input.GetMouseButtonDown(0))
                    {
                        if(playerController.HasCurrentWaypoint)
                            playerWaypoints.RemoveCurrentWaypoint();

                        playerWaypoints.WaypointToCursor();
                    }

                    if (_waypointMode && Input.GetMouseButtonDown(1))
                    {
                        playerWaypoints.RemoveCurrentWaypoint();
                    }
                    #endregion

                    #region ClickToMove 
                    if (Input.GetKeyDown(KeyCode.C))
                    {
                        if (_clickToMoveMode)
                        {
                            Cursor.SetCursor(null, Vector2.zero, cursorMode);
                        }
                        else
                        {
                            TurnOffAllOverheadModes();
                            Cursor.SetCursor(stepsCursor, stepsHotspot, cursorMode);
                        }
                        _clickToMoveMode = !_clickToMoveMode;
                    }
                    if (_clickToMoveMode && Input.GetMouseButtonDown(0))
                    {

                    }
                    #endregion

                    #region Pathways

                    if (Input.GetKeyDown(KeyCode.V))
                    {
                        if (_pathwayMode)
                        {
                            playerPathways.RemoveCurrentPathway();
                            _pathwayMode = false;
                        }
                        else
                        {
                            _pathwayMode = true;
                        }
                    }

                    //If Pathway Mode and There is a Waypoint but no Pathway yet
                    //Path to Waypoint
                    if (_pathwayMode &&
                        playerController.HasCurrentWaypoint &&
                        !playerController.HasCurrentPathway)
                    {
                        playerPathways.PathwayToCursor();
                    }

                    //If Pathway Mode and a Pathway and Mouse 1 is pressed
                    //Path to Waypoint
                    if (_pathwayMode &&
                        playerController.HasCurrentPathway &&
                        Input.GetMouseButtonDown(1))
                    {
                        playerPathways.RemoveCurrentPathway();
                    }

                    //If Pathway Mode and a Pathway and Mouse 0 is pressed
                    //Path to Waypoint
                    if (_pathwayMode &&
                        playerController.HasCurrentPathway &&
                        Input.GetMouseButtonDown(0))
                    {
                        playerPathways.RemoveCurrentPathway();
                        playerPathways.PathwayToCursor();
                    }


                    //If Pathway Mode and no Pathway and Mouse 0 is pressed
                    //Path to Waypoint
                    if (_pathwayMode &&
                        !playerController.HasCurrentPathway &&
                        Input.GetMouseButtonDown(0))
                    {
                        playerPathways.PathwayToCursor();
                    }



                    #endregion

                }
                #endregion



                #region Family
                if (Input.GetKeyDown(KeyCode.F1))
                {
                    if (debug) logger.TLog(this.GetType().Name, "Update|Switch to Player Soul");
                    playerController.CallCmdChangeFocusToPlayer();
                }
                if (Input.GetKeyDown(KeyCode.F2))
                {
                    if (debug) logger.TLog(this.GetType().Name, "Update|Switch to F2");
                    playerController.CallCmdChangeFocusForwardInFamily();
                }
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    if (debug) logger.TLog(this.GetType().Name, "Update|Switch to F3");
                    playerController.CallCmdChangeFocusBackwardInFamily();
                }
                #endregion

                #region Cameras

                //int id = player.currentFamilyFocus.GetInstanceID();

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    cameraController.SwitchToFirstPersonCam();
                    TurnOffAllOverheadModes();
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    cameraController.SwitchToThirdPersonCam();
                    TurnOffAllOverheadModes();
                }
                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    cameraController.SwitchToDeepCam();
                    TurnOffAllOverheadModes();
                }
                if (Input.GetKeyDown(KeyCode.Alpha4))
                {
                    cameraController.SwitchToIsoCam();
                    TurnOffAllForwardModes();
                }
                if (Input.GetKeyDown(KeyCode.Alpha5))
                {
                    cameraController.SwitchToTopDownCam();
                    TurnOffAllForwardModes();
                }

                if (cameraController.ForwardMode && Input.GetMouseButton(1))
                {
                    cameraController.RotateCamera();
                }
                #endregion


            }


            #endregion

            void TurnOffAllOverheadModes()
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                _clickToMoveMode = false;
                _waypointMode = false;
            }

            void TurnOffAllForwardModes()
            {
                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                _targetMode = false;
            }
        }
    }
}
