using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

namespace Sazboom.WarRoom
{
    [RequireComponent(typeof(CameraController))]
    [RequireComponent(typeof(NetworkLogger))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Sazboom.WarRoom.GridUI))]
    [RequireComponent(typeof(PlayerActions))]
    public class PlayerMovement : NetworkBehaviour
    {
        readonly bool debug = true;
        [SerializeField] private CharacterController characterController;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private NetworkLogger logger;
        [SerializeField] private Sazboom.WarRoom.GridUI gridUI;
        [SerializeField] private PlayerActions playerActions;

        [Header("Movement Settings")]
        
        //Move Speed
        [SerializeField] private float _moveSpeed = 2f;
        public float MoveSpeed { get { return _moveSpeed; } }

        //Turn Sensitivity
        [SerializeField] private float _turnSensitivity = 5f;
        public float TurnSensitivity { get { return _turnSensitivity; } }

        //MaxTurnSpeed
        [SerializeField] private float _maxTurnSpeed = 150f;
        public float MaxTurnSpeed { get { return _maxTurnSpeed; } }


        [Header("Diagnostics")]
        //Horizontal
        [SerializeField] private float _horizontal;
        public float Horizontal { get { return _horizontal; } set { _horizontal = value; } }
        
        //Vertical
        [SerializeField] private float _vertical;
        public float Vertical { get { return _vertical; } set { _vertical = value; } }
        
        //Turn
        [SerializeField] private float _turn;
        public float Turn { get { return _turn; } set { _turn = value; } }
        
        //Jump Speed
        [SerializeField] private float _jumpSpeed;
        public float JumpSpeed { get { return _jumpSpeed; } set { _jumpSpeed = value; } }
        
        //Is Grounded
        [SerializeField] private bool _isGrounded = true;
        public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
        
        //Is Falling
        [SerializeField] private bool _isFalling;
        public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
        
        //Velocity
        [SerializeField] private Vector3 _velocity;
        public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }

        private Coroutine clickToMove;
        [SerializeField] private bool _moving = false;
        [SerializeField] private float _moveUnit = 2.0f;
        [SerializeField] private bool _moveKeyMode = false;
        [SerializeField] private Vector3 _walkableOffset = new Vector3(0, 2, 0);
        public bool MoveKeyMode { get { return _moveKeyMode; } set { _moveKeyMode = value; } }

        [SerializeField] private float _facingDegree = 0;

        void OnValidate()
        {
            if (logger == null)
                logger = GetComponent<NetworkLogger>();
            if (cameraController == null)
                cameraController = GetComponent<CameraController>();
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
            if (playerActions == null)
                playerActions = GetComponent<PlayerActions>();
            if (gridUI == null)
                gridUI = GameObject.Find("SceneUI").GetComponent<Sazboom.WarRoom.GridUI>();
        }

        public void ToggleKeyMoveMode()
        {
            _moveKeyMode = !_moveKeyMode;
        }


        public void MoveForwardLeft()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir += transform.forward;
                dir -= transform.right;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove( dest, dir );
            }
        }

        public void MoveForward()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir += transform.forward;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove(dest, dir);
            }
        }

        public void MoveForwardRight()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir += transform.forward;
                dir += transform.right;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove(dest, dir);
            }
        }
        public void MoveLeft()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir -= transform.right;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove(dest, dir);
            }
        }

        public void MoveRight()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir += transform.right;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove(dest, dir);
            }
        }

        public void MoveBackLeft()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir -= transform.forward;
                dir -= transform.right;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove(dest, dir);
            }
        }

        public void MoveBack()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir -= transform.forward;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove(dest, dir);
            }
        }

        public void MoveBackRight()
        {
            if (!_moving)
            {
                Vector3 dir = Vector3.zero;
                dir -= transform.forward;
                dir += transform.right;
                Vector3 dest = transform.position + (dir * _moveUnit);
                KeyToMove(dest, dir);
            }
        }







        public void RotateLeft()
        {
            transform.Rotate(0f, -45f, 0f, Space.World);
        }

        public void RotateRight()
        {
            transform.Rotate(0f, 45f, 0f, Space.World);
        }

        public void KeyToMove(Vector3 dest, Vector3 dir)
        {
            _moving = true;

            StartCoroutine(MoveToDirection(gridUI.GetNearestPointOnGrid(transform.position, dest), dir));
        }


        public void ClickToMove()
        {
            Vector3 hitPoint;
            if (IsWalkable(out hitPoint))
            {
                _moving = true;
                Vector3 pos = gridUI.GetNearestPointOnGrid(transform.position, hitPoint + _walkableOffset);
                pos.y = hitPoint.y;
                StartCoroutine(MoveTo(pos));

            }

        }

        IEnumerator MoveTo(Vector3 destination)
        {
            while (Vector3.Distance(transform.position, destination) > 0.05f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, _moveSpeed * Time.deltaTime);
                yield return null;
            }
            if (playerActions.GridMode)
                gridUI.DrawGrid(transform.position);
            _moving = false;
        }

        IEnumerator MoveToDirection(Vector3 destination, Vector3 direction)
        {
            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                Debug.Log(Vector3.Distance(transform.position, destination));
                direction = Vector3.ClampMagnitude(direction, 1f);
                direction *= _moveSpeed;
                characterController.Move(direction * Time.deltaTime);
                yield return null;
            }
            transform.position = destination;
            if (playerActions.GridMode)
                gridUI.DrawGrid(transform.position);
            _moving = false;
        }


        public bool IsWalkable(out Vector3 target)
        {
            Ray ray = SetRaycastFromCamera();
            RaycastHit hit;
            int mask = MaskToTokenLayer();

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.transform.CompareTag("Walkable"))
                {
                    target = hit.point;
                    return true;
                }
                target = Vector3.zero;
                return false;
            }
            else
            {
                target = Vector3.zero;
                return false;
            }
        }

        public Ray SetRaycastFromCamera()
        {
            return cameraController.CurrentCamera.ScreenPointToRay(Input.mousePosition);
        }

        public int MaskToTokenLayer()
        {
            //EXAMPLE OF MASKING MORE THAN ONE LAYER
            //int terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
            //int tokenLayer = 1 << LayerMask.NameToLayer("Token");
            //int mask = terrainLayer | tokenLayer;

            int tokenLayer = 1 << LayerMask.NameToLayer("Terrain");
            int mask = tokenLayer;
            return mask;
        }

        public void SetEndPoints(GameObject pw, Vector3 origin, Vector3 dest)
        {
            LineRenderer lr = pw.transform.Find("Line").GetComponent<LineRenderer>();
            if (debug) logger.TLog(this.GetType().Name, "SetEndPoints|Origin: " + origin);
            if (debug) logger.TLog(this.GetType().Name, "SetEndPoints|Dest: " + dest);
            lr.SetPosition(0, origin);
            lr.SetPosition(1, dest);
        }
    }
}

//void FixedUpdate()
//{
//    #region Movement
//    if (!isLocalPlayer || characterController == null)
//        return;

//    transform.Rotate(0f, _turn * Time.fixedDeltaTime, 0f);

//    Vector3 direction = new Vector3(_horizontal, _jumpSpeed, _vertical);
//    direction = Vector3.ClampMagnitude(direction, 1f);
//    direction = transform.TransformDirection(direction);
//    direction *= _moveSpeed;

//    if (_jumpSpeed > 0)
//        characterController.Move(direction * Time.fixedDeltaTime);
//    else
//        characterController.SimpleMove(direction);

//    _isGrounded = characterController.isGrounded;
//    _velocity = characterController.velocity;


//    #endregion
//}

//playerMovement.Horizontal = Input.GetAxis("Horizontal");
//            playerMovement.Vertical = Input.GetAxis("Vertical");


// Q and E cancel each other out, reducing the turn to zero
//if (Input.GetKey(KeyCode.Q))
//    playerMovement.RotateLeft();
//if (Input.GetKey(KeyCode.E))
//    playerMovement.RotateRight();
//if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.E))
//    playerMovement.CancelOutRotation();
//if (!Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.E))
//    playerMovement.CancelOutRotation();

//if (playerMovement.IsGrounded)
//    playerMovement.IsFalling = false;

//if ((playerMovement.IsGrounded || !playerMovement.IsFalling) && 
//    playerMovement.JumpSpeed < 1f && Input.GetKey(KeyCode.Space))
//{
//    playerMovement.JumpSpeed = Mathf.Lerp(playerMovement.JumpSpeed, 1f, 0.5f);
//}
//else if (!playerMovement.IsGrounded)
//{
//    playerMovement.IsFalling = true;
//    playerMovement.JumpSpeed = 0;
//}


