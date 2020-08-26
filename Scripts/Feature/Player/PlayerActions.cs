using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;


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

            #region Properties
            [Header("Movement Settings")]
            [SerializeField] private float moveSpeed = 8f;
            [SerializeField] private float turnSensitivity = 5f;
            [SerializeField] private float maxTurnSpeed = 150f;

            [Header("Diagnostics")]
            [SerializeField] private float horizontal;
            [SerializeField] private float vertical;
            [SerializeField] private float turn;
            [SerializeField] private float jumpSpeed;
            [SerializeField] private bool isGrounded = true;
            [SerializeField] private bool isFalling;
            [SerializeField] private Vector3 velocity;

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


                    horizontal = Input.GetAxis("Horizontal");
                    vertical = Input.GetAxis("Vertical");

                    // Q and E cancel each other out, reducing the turn to zero
                    if (Input.GetKey(KeyCode.Q))
                        turn = Mathf.MoveTowards(turn, -maxTurnSpeed, turnSensitivity);
                    if (Input.GetKey(KeyCode.E))
                        turn = Mathf.MoveTowards(turn, maxTurnSpeed, turnSensitivity);
                    if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
                        turn = Mathf.MoveTowards(turn, 0, turnSensitivity);
                    if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
                        turn = Mathf.MoveTowards(turn, 0, turnSensitivity);

                    if (isGrounded)
                        isFalling = false;

                    if ((isGrounded || !isFalling) && jumpSpeed < 1f && Input.GetKey(KeyCode.Space))
                    {
                        jumpSpeed = Mathf.Lerp(jumpSpeed, 1f, 0.5f);
                    }
                    else if (!isGrounded)
                    {
                        isFalling = true;
                        jumpSpeed = 0;
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
                        playerDistances.DistanceToCursor();
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
                            Cursor.SetCursor(waypointCursor, waypointHotspot, cursorMode);
                            _clickToMoveMode = false;
                            _waypointMode = true;
                        }
                    }
                    if (_waypointMode && Input.GetMouseButtonDown(0))
                    {
                        playerWaypoints.WaypointToCursor();
                    }

                    if (_waypointMode && Input.GetMouseButtonDown(1))
                    {
                        playerController.CallCmdRemoveCurrentWaypoint();
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
                            Cursor.SetCursor(stepsCursor, stepsHotspot, cursorMode);
                        }
                        _clickToMoveMode = !_clickToMoveMode;
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
      
                    if (_pathwayMode && playerController.HasCurrentWaypoint)
                    {
                        playerPathways.PathwayToWaypoint();

                    }
                    else if (_pathwayMode && Input.GetMouseButtonDown(0))
                    {
                        playerPathways.PathwayToCursor();
                    }

                    if (_pathwayMode && Input.GetMouseButtonDown(1))
                    {
                        playerController.CallCmdRemoveCurrentPathway();
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

            void FixedUpdate()
            {
                #region Movement
                if (!isLocalPlayer || characterController == null)
                    return;

                transform.Rotate(0f, turn * Time.fixedDeltaTime, 0f);

                Vector3 direction = new Vector3(horizontal, jumpSpeed, vertical);
                direction = Vector3.ClampMagnitude(direction, 1f);
                direction = transform.TransformDirection(direction);
                direction *= moveSpeed;

                if (jumpSpeed > 0)
                    characterController.Move(direction * Time.fixedDeltaTime);
                else
                    characterController.SimpleMove(direction);

                isGrounded = characterController.isGrounded;
                velocity = characterController.velocity;
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
