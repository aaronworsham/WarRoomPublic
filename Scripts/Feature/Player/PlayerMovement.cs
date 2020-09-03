using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Mirror;
using System;


namespace Sazboom.WarRoom
{
    //[RequireComponent(typeof(CameraController))]
    //[RequireComponent(typeof(NetworkLogger))]
    //[RequireComponent(typeof(CharacterController))]
    //[RequireComponent(typeof(Sazboom.WarRoom.GridUI))]
    //[RequireComponent(typeof(PlayerActions))]
    //[RequireComponent(typeof(NavMeshAgent))]
    //public class PlayerMovement : NetworkBehaviour
    //{
    //    readonly bool debug = true;
    //    [SerializeField] private CharacterController characterController;
    //    [SerializeField] private CameraController cameraController;
    //    [SerializeField] private NetworkLogger logger;
    //    [SerializeField] private Sazboom.WarRoom.GridUI gridUI;
    //    [SerializeField] private PlayerActions playerActions;
    //    [SerializeField] private NavMeshAgent navAgent;
    //    [SerializeField] private GameObject soulOrb;

    //    [Header("Movement Settings")]
        
    //    //Move Speed
    //    [SerializeField] private float _moveSpeed = 10f;
    //    public float MoveSpeed { get { return _moveSpeed; } }

    //    //Turn Sensitivity
    //    [SerializeField] private float _turnSensitivity = 5f;
    //    public float TurnSensitivity { get { return _turnSensitivity; } }

    //    //MaxTurnSpeed
    //    [SerializeField] private float _maxTurnSpeed = 150f;
    //    public float MaxTurnSpeed { get { return _maxTurnSpeed; } }


    //    [Header("Diagnostics")]
    //    //Horizontal
    //    [SerializeField] private float _horizontal;
    //    public float Horizontal { get { return _horizontal; } set { _horizontal = value; } }
        
    //    //Vertical
    //    [SerializeField] private float _vertical;
    //    public float Vertical { get { return _vertical; } set { _vertical = value; } }
        
    //    //Turn
    //    [SerializeField] private float _turn;
    //    public float Turn { get { return _turn; } set { _turn = value; } }
        
    //    //Jump Speed
    //    [SerializeField] private float _jumpSpeed;
    //    public float JumpSpeed { get { return _jumpSpeed; } set { _jumpSpeed = value; } }
        
    //    //Is Grounded
    //    [SerializeField] private bool _isGrounded = true;
    //    public bool IsGrounded { get { return _isGrounded; } set { _isGrounded = value; } }
        
    //    //Is Falling
    //    [SerializeField] private bool _isFalling;
    //    public bool IsFalling { get { return _isFalling; } set { _isFalling = value; } }
        
    //    //Velocity
    //    [SerializeField] private Vector3 _velocity;
    //    public Vector3 Velocity { get { return _velocity; } set { _velocity = value; } }

    //    private Coroutine clickToMove;
    //    [SerializeField] private bool _moving = false;
    //    [SerializeField] private float _moveUnit = 2.0f;
    //    [SerializeField] private bool _moveKeyMode = false;
    //    [SerializeField] private Vector3 _walkableOffset = new Vector3(0, 2, 0);
    //    public bool MoveKeyMode { get { return _moveKeyMode; } set { _moveKeyMode = value; } }

    //    [SerializeField] private float _facingDegree = 0;

    //    [SerializeField] private Coroutine gridRefresher;

    //    void OnValidate()
    //    {
    //        if (logger == null)
    //            logger = GetComponent<NetworkLogger>();
    //        if (cameraController == null)
    //            cameraController = GetComponent<CameraController>();
    //        if (characterController == null)
    //            characterController = GetComponent<CharacterController>();
    //        if (playerActions == null)
    //            playerActions = GetComponent<PlayerActions>();
    //        if (navAgent == null)
    //            navAgent = GetComponent<NavMeshAgent>();
    //        if (soulOrb == null)
    //            soulOrb = transform.Find("SoulOrb").gameObject;
    //        if (gridUI == null)
    //            gridUI = GameObject.Find("SceneUI").GetComponent<Sazboom.WarRoom.GridUI>();
    //    }

    //    public void ToggleKeyMoveMode()
    //    {
    //        _moveKeyMode = !_moveKeyMode;
    //    }



    //    public void MoveForward()
    //    {

    //        Vector3 dir = Vector3.zero;
    //        dir += transform.forward;
    //        Vector3 dest = transform.position + (dir * _moveUnit);
    //        KeyToMove(dest, dir);
            
    //    }

    //    public void MoveBack()
    //    {
    //        Vector3 dir = Vector3.zero;
    //        dir -= transform.forward;
    //        Vector3 dest = transform.position + (dir * _moveUnit);
    //        KeyToMove(dest, dir);
    //    }

    //    public void MoveLeft()
    //    {
    //        Vector3 dir = Vector3.zero;
    //        dir -= transform.right;
    //        Vector3 dest = transform.position + (dir * _moveUnit);
    //        KeyToMove(dest, dir);
    //    }

    //    public void MoveRight()
    //    {
    //        Vector3 dir = Vector3.zero;
    //        dir += transform.right;
    //        Vector3 dest = transform.position + (dir * _moveUnit);
    //        KeyToMove(dest, dir);
    //    }



    //    public void RotateLeft()
    //    {
    //        transform.Rotate(0f, -45f, 0f, Space.World);
    //    }

    //    public void RotateRight()
    //    {
    //        transform.Rotate(0f, 45f, 0f, Space.World);
    //    }

    //    public void KeyToMove(Vector3 dest, Vector3 dir)
    //    {
    //        Vector3 hitPoint;
    //        Ray ray = SetRaycastFromOrb(dir);
    //        navAgent.updateRotation = false;
    //        navAgent.speed = 10f;
    //        if (IsWalkable(ray, out hitPoint))
    //        {
    //            Vector3 pos = gridUI.GetNearestPointOnGrid(transform.position, hitPoint + _walkableOffset);
    //            navAgent.destination = pos;

    //        }

    //    }


    //    public void ClickToMove()
    //    {
    //        Vector3 hitPoint;
    //        Ray ray = SetRaycastFromCamera();
    //        navAgent.updateRotation = false;
    //        navAgent.speed = 10f;
    //        if (IsWalkable(ray, out hitPoint))
    //        {
    //            _moving = true;
    //            Vector3 pos = gridUI.GetNearestPointOnGrid(transform.position, hitPoint + _walkableOffset);
    //            navAgent.destination = pos;
    //            if (playerActions.GridMode)
    //            {
    //                StopAllCoroutines();
    //                gridRefresher = StartCoroutine("GridRefresher");
    //            }

    //        }

    //    }

    //    IEnumerator GridRefresher()
    //    {
    //        if (navAgent.pathPending)
    //        {
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //        while (navAgent.remainingDistance >= 1)
    //        {
    //            Debug.Log("Tier 1: " + navAgent.remainingDistance);
    //            yield return new WaitForSeconds(0.1f);
    //        }
    //        while (navAgent.remainingDistance >= .1)
    //        {
    //            yield return null;
    //        }
    //        gridUI.DrawGrid(transform.position);

    //    }



    //    public bool IsWalkable(Ray ray, out Vector3 target)
    //    {
            
    //        RaycastHit hit;
    //        int mask = MaskToTokenLayer();

    //        if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
    //        {
    //            if (hit.transform.CompareTag("Walkable"))
    //            {
    //                target = hit.point;
    //                return true;
    //            }
    //            target = Vector3.zero;
    //            return false;
    //        }
    //        else
    //        {
    //            target = Vector3.zero;
    //            return false;
    //        }
    //    }

    //    public Ray SetRaycastFromCamera()
    //    {
    //        return cameraController.CurrentCamera.ScreenPointToRay(Input.mousePosition);
    //    }

    //    public Ray SetRaycastFromOrb(Vector3 dir)
    //    {
    //        Vector3 orbPos = soulOrb.transform.position;
    //        dir.y += -1;
    //        return new Ray(orbPos, dir);
    //    }

    //    public int MaskToTokenLayer()
    //    {
    //        //EXAMPLE OF MASKING MORE THAN ONE LAYER
    //        //int terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
    //        //int tokenLayer = 1 << LayerMask.NameToLayer("Token");
    //        //int mask = terrainLayer | tokenLayer;

    //        int tokenLayer = 1 << LayerMask.NameToLayer("Terrain");
    //        int mask = tokenLayer;
    //        return mask;
    //    }

    //}
}


