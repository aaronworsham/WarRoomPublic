using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SickDev.CommandSystem;
using UnityEngine.AI;
using Mirror;

namespace Sazboom.WarRoom
{
    public class GmMovement : NetworkBehaviour
    {
        [SerializeField] private CharacterController characterController;
        [SerializeField] private NavMeshAgent navAgent;
        [SerializeField] private PlayerActions playerActions;
        [SerializeField] private PlayerModel playerModel;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private float moveSpeed = 10.0f;
        [SerializeField] private float rotateSpeed = 20.0f;
        void OnValidate()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
            if (navAgent == null)
                navAgent = GetComponent<NavMeshAgent>();
            if (playerModel == null)
                playerModel = GetComponent<PlayerModel>();
            if (playerActions == null)
                playerActions = GetComponent<PlayerActions>();
            if (cameraController == null)
                cameraController = GetComponent<CameraController>();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();
            DevConsole.singleton.AddCommand(new ActionCommand(Me) { className = "GameMaster" });
        }

        

        private void Me()
        {
            playerActions.ClientMode = PlayerActions.ClientModes.GM;
            navAgent.enabled = false;
            characterController.enabled = true;
        }

        public void Move()
        {

            float dt = Time.deltaTime;
            float dy = 0;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                dy = moveSpeed * dt;
            }
            if (Input.GetKey(KeyCode.Space))
            {
                dy -= moveSpeed * dt;
            }
            float dx = Input.GetAxis("Horizontal") * dt * moveSpeed;
            float dz = Input.GetAxis("Vertical") * dt * moveSpeed;

            characterController.Move(transform.TransformDirection(new Vector3(dx, dy, dz)));


        }


        public void RotateLeft()
        {
            float rotationY = rotateSpeed * Time.deltaTime;
            transform.Rotate(0, -rotationY, 0);
        }

        public void RotateRight()
        {
            float rotationY = rotateSpeed * Time.deltaTime;
            transform.Rotate(0, rotationY, 0);
        }

        public void ResetRotation()
        {
            transform.rotation = Quaternion.identity;
        }
    }
}

