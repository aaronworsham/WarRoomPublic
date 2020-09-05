using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System.Text;

namespace Sazboom.WarRoom
{

        [RequireComponent(typeof(CharacterController))]
        [RequireComponent(typeof(CameraController))]
        //[RequireComponent(typeof(NetworkTransform))]
        //[RequireComponent(typeof(CapsuleCollider))]
        [RequireComponent(typeof(PlayerSelections))]
        //[RequireComponent(typeof(NetworkLogger))]
        [RequireComponent(typeof(PlayerController))]
        //[RequireComponent(typeof(PlayerFamily))]
        //[RequireComponent(typeof(PlayerDistances))]
        //[RequireComponent(typeof(PlayerWaypoints))]
        //[RequireComponent(typeof(PlayerPathways))]
        [RequireComponent(typeof(PlayerMovement))]
        //[RequireComponent(typeof(GridUI))]
        //[RequireComponent(typeof(GmMovement))]
        //[RequireComponent(typeof(ISceneManagable))]
        public class PlayerActions : NetworkBehaviour, IPlayerActionable
        {
        //    readonly bool debug = false;
        //    [SerializeField] private NetworkLogger logger;
            [SerializeField] private CharacterController characterController;
        //    [SerializeField] private CapsuleCollider capsuleCollider;
            [SerializeField] private PlayerSelections playerSelections;
            [SerializeField] private PlayerController playerController;
        //    [SerializeField] private PlayerFamily playerFamily;
            [SerializeField] private CameraController cameraController;
        //    [SerializeField] private PlayerDistances playerDistances;
        //    [SerializeField] private PlayerWaypoints playerWaypoints;
        //    [SerializeField] private PlayerPathways playerPathways;
            [SerializeField] private PlayerMovement playerMovement;
        //    [SerializeField] private GridUI gridUI;
        //    [SerializeField] private GmMovement gmMovement;
        //    [SerializeField] private ISceneManagable sceneManager;

        //    #region Properties

        public enum ClientModes
        {
            PLAYER,
            GM
        }

        //    [Header("Modes")]
        [SerializeField] private bool _readyForAction = false;
        [SerializeField] private bool _targetMode = false;
        [SerializeField] private bool _distanceMode = false;
        [SerializeField] private bool _waypointMode = false;
        [SerializeField] private bool _movementMode = false;
        [SerializeField] private bool _pathwayMode = false;
        [SerializeField] private bool _gridMode = false;
        [SerializeField] private bool _cameraMoveMode = false;
        [SerializeField] private ClientModes clientMode = ClientModes.PLAYER;
        public bool GridMode { get { return _gridMode; } }
        public ClientModes ClientMode { get { return clientMode; } set { clientMode = value; } }


        [Header("Cursors")]
        [SerializeField] private Texture2D targetCursor;
        [SerializeField] private Texture2D waypointCursor;
        [SerializeField] private Texture2D stepsCursor;
        [SerializeField] private CursorMode cursorMode = CursorMode.Auto;
        [SerializeField] private Vector2 targetHotspot;
        [SerializeField] private Vector2 waypointHotspot;
        [SerializeField] private Vector2 stepsHotspot;


        //    #endregion



        //    #region Callbacks
        void Start()
        {
            //        if (logger == null)
            //            logger = GetComponent<NetworkLogger>();
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
            //        if (capsuleCollider == null)
            //            capsuleCollider = GetComponent<CapsuleCollider>();
            if (playerSelections == null)
                playerSelections = GetComponent<PlayerSelections>();
            if (playerController == null)
                playerController = GetComponent<PlayerController>();
            //        if (playerFamily == null)
            //            playerFamily = GetComponent<PlayerFamily>();
            if (cameraController == null)
                cameraController = GetComponent<CameraController>();
            //        if (playerDistances == null)
            //            playerDistances = GetComponent<PlayerDistances>();
            //        if (playerWaypoints == null)
            //            playerWaypoints = GetComponent<PlayerWaypoints>();
            //        if (playerPathways == null)
            //            playerPathways = GetComponent<PlayerPathways>();
            if (playerMovement == null)
                playerMovement = GetComponent<PlayerMovement>();
            //        if (gmMovement == null)
            //            gmMovement = GetComponent<GmMovement>();
            //        if (gridUI == null)
            //            gridUI = GameObject.Find("SceneUI").GetComponent<GridUI>();
            //        if (sceneManager == null)
            //            sceneManager = GameObject.Find("Scene").GetComponent<ISceneManagable>();
        }



            //    public override void OnStartLocalPlayer()
            //    {
            //        characterController.enabled = true;
            //    }

            //    void OnDisable()
            //    {
            //    }

            void Update()
        {
            if (!isLocalPlayer) return;
            if (!hasAuthority) return;
            if (!characterController.enabled) return;
            if (!_readyForAction) return;

            int instanceId = gameObject.GetInstanceID();

            #region Modes

            //Exclusive Modes

            //Movement
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_movementMode)
                {
                    Cursor.SetCursor(null, Vector2.zero, cursorMode);
                    _movementMode = false;
                }
                else
                {
                    TurnOffAllOverheadModes();
                    Cursor.SetCursor(stepsCursor, stepsHotspot, cursorMode);
                    _movementMode = true;
                }
            }

            //Target
            else if (Input.GetKeyDown(KeyCode.F))
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


            //Waypoints
            else if (Input.GetKeyDown(KeyCode.V))
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
                    _waypointMode = true;
                }
            }

            //Shared Modes

            //Distance
            else if (Input.GetKeyDown(KeyCode.T))
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

            //Pathways

            else if (Input.GetKeyDown(KeyCode.G))
            {
                //if (_pathwayMode)
                //{
                //    playerPathways.RemoveCurrentPathway();
                //    _pathwayMode = false;
                //}
                //else
                //{
                //    _pathwayMode = true;
                //}
            }

            //Grid 

            else if (Input.GetKeyDown(KeyCode.B))
            {
            //    if (!_gridMode)
            //    {
            //        gridUI.DrawGrid(transform.position);
            //        _gridMode = true;
            //    }
            //    else
            //    {
            //        gridUI.DestroyGrid();
            //        _gridMode = false;
            //    }
            }

            //Camera Move Mode
            else if (Input.GetKeyDown(KeyCode.Y))
            {

            }

            #endregion


                if (clientMode == ClientModes.PLAYER)
            {
                #region Movement

                if (_movementMode)
                {

                    //WASDEQ Key Movement
                    if (Input.GetKeyDown(KeyCode.W))
                    {
                        playerMovement.MoveForward();
                    }
                    else if (Input.GetKeyDown(KeyCode.A))
                    {
                        playerMovement.MoveLeft();
                    }
                    else if (Input.GetKeyDown(KeyCode.S))
                    {
                        playerMovement.MoveBack();
                    }
                    else if (Input.GetKeyDown(KeyCode.D))
                    {
                        playerMovement.MoveRight();
                    }
                    else if (Input.GetKeyDown(KeyCode.Q))
                    {
                        playerMovement.RotateLeft();
                    }
                    else if (Input.GetKeyDown(KeyCode.E))
                    {
                        playerMovement.RotateRight();
                    }

                    if (Input.GetMouseButtonDown(0))
                    {
                        playerMovement.ClickToMove();
                    }

                }

                #endregion

                #region TargetMode

                if (_targetMode)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        GameObject target;
                        if (playerSelections.IsPlayerToken(out target))
                        {
                            NetworkIdentity targetId = target.GetComponent<NetworkIdentity>();

                            if (targetId != null && !targetId.hasAuthority)
                            {
                                Cursor.SetCursor(null, Vector2.zero, cursorMode);
                                _targetMode = false;
                                //playerController.CallCmdGrantAuthority(target);
                            }

                        }
                    }
                }

                #endregion
                #region Camera Movement
                if (_cameraMoveMode)
                {

                    if (Input.GetKey(KeyCode.W))
                    {
                        cameraController.MoveCameraForward();
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        cameraController.MoveCameraLeft();
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        cameraController.MoveCameraBack();
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        cameraController.MoveCameraRight();
                    }
                    if (Input.GetKeyDown(KeyCode.Q))
                    {
                        cameraController.RotateOverheadCameraLeft();
                    }
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        cameraController.RotateOverheadCameraRight();
                    }
                    if (Input.GetKey(KeyCode.Z))
                    {
                        cameraController.ZoomCameraIn();
                    }
                    if (Input.GetKey(KeyCode.X))
                    {
                        cameraController.ZoomCameraOut();
                    }
                }
                #endregion

                #region Distance
                if (_distanceMode)
                {
                    
                    //StringBuilder str = new StringBuilder();
                    //str.Append(playerDistances.DistanceToCursor());
                    //if (playerController.HasCurrentWaypoint)
                    //{
                    //    Vector3 point = playerController.CurrentWaypoint.transform.position;
                    //    str.Append(playerDistances.DistanceToWaypoint(point));
                    //}
                    //UiBottomRight.OnMessage(str.ToString());
                    
                }
                #endregion

                    #region Waypoints

                    //if (_waypointMode && Input.GetMouseButtonDown(0))
                    //{
                    //    if (playerController.HasCurrentWaypoint)
                    //        playerWaypoints.RemoveCurrentWaypoint();

                    //    playerWaypoints.WaypointToCursor();
                    //}

                    //if (_waypointMode && Input.GetMouseButtonDown(1))
                    //{
                    //    playerWaypoints.RemoveCurrentWaypoint();
                    //}
                    #endregion

                    

                    #region Pathways


                    ////If Pathway Mode and There is a Waypoint but no Pathway yet
                    ////Path to Waypoint
                    //if (_pathwayMode &&
                    //    playerController.HasCurrentWaypoint &&
                    //    !playerController.HasCurrentPathway)
                    //{
                    //    playerPathways.PathwayToCursor();
                    //}

                    ////If Pathway Mode and a Pathway and Mouse 1 is pressed
                    ////Path to Waypoint
                    //if (_pathwayMode &&
                    //    playerController.HasCurrentPathway &&
                    //    Input.GetMouseButtonDown(1))
                    //{
                    //    playerPathways.RemoveCurrentPathway();
                    //}

                    ////If Pathway Mode and a Pathway and Mouse 0 is pressed
                    ////Path to Waypoint
                    //if (_pathwayMode &&
                    //    playerController.HasCurrentPathway &&
                    //    Input.GetMouseButtonDown(0))
                    //{
                    //    playerPathways.RemoveCurrentPathway();
                    //    playerPathways.PathwayToCursor();
                    //}


                    ////If Pathway Mode and no Pathway and Mouse 0 is pressed
                    ////Path to Waypoint
                    //if (_pathwayMode &&
                    //    !playerController.HasCurrentPathway &&
                    //    Input.GetMouseButtonDown(0))
                    //{
                    //    playerPathways.PathwayToCursor();
                    //}



                    #endregion

                

                #region Grid


                #endregion
            }

            if (clientMode == ClientModes.GM)
            {
                #region Movement
                //gmMovement.Move();
                //if (Input.GetKey(KeyCode.Q))
                //{
                //    gmMovement.RotateLeft();
                //}
                //else if (Input.GetKey(KeyCode.E))
                //{
                //    gmMovement.RotateRight();
                //}

                #endregion
            }




            #region Family
            //if (Input.GetKeyDown(KeyCode.F1))
            //{
            //    if (debug) logger.TLog(this.GetType().Name, "Update|Switch to Player Soul");
            //    playerController.CallCmdChangeFocusToPlayer();
            //}
            //if (Input.GetKeyDown(KeyCode.F2))
            //{
            //    if (debug) logger.TLog(this.GetType().Name, "Update|Switch to F2");
            //    playerController.CallCmdChangeFocusForwardInFamily();
            //}
            //if (Input.GetKeyDown(KeyCode.F3))
            //{
            //    if (debug) logger.TLog(this.GetType().Name, "Update|Switch to F3");
            //    playerController.CallCmdChangeFocusBackwardInFamily();
            //}
            #endregion

            #region Cameras

            //int id = player.currentFamilyFocus.GetInstanceID();

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                cameraController.SwitchToFirstPersonCam();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                cameraController.SwitchToThirdPersonCam();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                cameraController.SwitchToDeepCam();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                cameraController.SwitchToIsoCam();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                cameraController.SwitchToTopDownCam();
            }

            if (cameraController.ForwardMode && Input.GetMouseButton(1))
            {
                cameraController.RotateCamera();
            }
            #endregion


        }


        //    #endregion

        void TurnOffAllOverheadModes()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
            _movementMode = false;
            _waypointMode = false;
        }

        void TurnOffAllForwardModes()
        {
            Cursor.SetCursor(null, Vector2.zero, cursorMode);
            _targetMode = false;
        }

        public void TokenIsReady()
        {
            _readyForAction = true;
        }
    }
    
}
