using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking.Types;
using Sazboom.WarRoom;

namespace Sazboom.WarRoom
{
    [RequireComponent(typeof(NetworkLogger))]
    [RequireComponent(typeof(CameraController))]
    public class PlayerSelections : NetworkBehaviour
    {

        readonly bool debug = false;
        [SerializeField] private NetworkLogger logger;
        [SerializeField] private CameraController cameraController;



        [SerializeField] private bool _targetMode = false;

        //    //Current Camera from CameraController
        //    public Camera currentCamera;



        #region Callbacks & Events

        void OnValidate()
        {
            if (logger == null)
                logger = GetComponent<NetworkLogger>();
            if (cameraController == null)
                cameraController = GetComponent<CameraController>();
        }

        #endregion


        #region Instance Methods

        public bool IsPlayerToken(out GameObject target)
        {
            Ray ray = SetRaycastFromCamera();
            RaycastHit hit;
            int mask = MaskToTokenLayer();

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                if (hit.transform.CompareTag("Player_Token"))
                {
                    target = hit.collider.gameObject;
                    return true;
                }
                target = null;
                return false;
            }
            else
            {
                target = null;
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

            int tokenLayer = 1 << LayerMask.NameToLayer("Token");
            int mask = tokenLayer;
            return mask;
        }


        public void MergePlayerAndTarget(GameObject target)
        {


            if (debug) logger.TLog(this.GetType().Name, "MergePlayerAndTarget");
            target.GetComponent<TokenController>().HandleTokenSelected(gameObject);
        }

        #endregion


    }

}

