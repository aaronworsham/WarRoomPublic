using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace Sazboom.WarRoom
{
    [RequireComponent(typeof(NetworkLogger))]
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private NetworkLogger logger;

        [Header("Movement Settings")]
        
        //Move Speed
        [SerializeField] private float _moveSpeed = 8f;
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

        void OnValidate()
        {
            if (logger == null)
                logger = GetComponent<NetworkLogger>();
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
        }

        void FixedUpdate()
        {
            #region Movement
            if (!isLocalPlayer || characterController == null)
                return;

            transform.Rotate(0f, _turn * Time.fixedDeltaTime, 0f);

            Vector3 direction = new Vector3(_horizontal, _jumpSpeed, _vertical);
            direction = Vector3.ClampMagnitude(direction, 1f);
            direction = transform.TransformDirection(direction);
            direction *= _moveSpeed;

            if (_jumpSpeed > 0)
                characterController.Move(direction * Time.fixedDeltaTime);
            else
                characterController.SimpleMove(direction);

            _isGrounded = characterController.isGrounded;
            _velocity = characterController.velocity;


            #endregion
        }

        public void RotateLeft()
        {
            _turn = Mathf.MoveTowards(_turn, -_maxTurnSpeed, _turnSensitivity);
        }

        public void RotateRight()
        {
            _turn = Mathf.MoveTowards(_turn, _maxTurnSpeed, _turnSensitivity);
        }

        public void CancelOutRotation()
        {
            _turn = Mathf.MoveTowards(_turn, 0, _turnSensitivity);
        }
    }
}


