using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sazboom.WarRoom
{
    [RequireComponent(typeof(NetworkLogger))]
    [RequireComponent(typeof(TokenModel))]
    public class TokenController : NetworkBehaviour
    {
        readonly bool debug = false;
        [SerializeField] private NetworkLogger logger;
        [SerializeField] private TokenModel tokenModel;

        #region Properties
        [SerializeField] private string _currentColor;
        public string CurrentColor
        {
            get { return _currentColor; }
            set { _currentColor = value; }
        }
        #endregion



        #region Callbacks

        private void Awake()
        {
            logger = gameObject.GetComponent<NetworkLogger>();
        }

        void OnValidate()
        {
            if (logger == null)
                logger = GetComponent<NetworkLogger>();
            if (tokenModel == null)
                tokenModel = GetComponent<TokenModel>();
        }



        public void HandleTokenSelected(GameObject player, string color)
        {
            if (debug) logger.TLog(this.GetType().Name, "HandleTokenSelected");
            
            //Move Token and Change Color for the Server
            tokenModel.MovePlayerOverToken(player);
            tokenModel.ChangeBaseColor(color);
            _currentColor = color;
            
            //Update all clients with new color and change base.
            RpcChangeBaseColor(color);
        }

        public void HandleChangeFocus(GameObject focus, GameObject player)
        {
            //if (debug) logger.TLog(this.GetType().Name, "HandleChangeFocus");
            //if (debug) logger.TLog(this.GetType().Name, "HandleChangeFocus|Focus " + focus.name);
            //if (currentlyInFocus && focus.GetInstanceID() != gameObject.GetInstanceID())
            //{
            //    if (debug) logger.TLog(this.GetType().Name, "HandleChangeFocus| Focus away from this Token");
            //    SeparatePlayerFromToken();
            //}
            //if (!currentlyInFocus && focus.GetInstanceID() == gameObject.GetInstanceID())
            //{
            //    MovePlayerOverToken(player);
            //}
        }
        #endregion

        #region RPCs 
        [ClientRpc]
        void RpcChangeBaseColor(string color)
        {
            if (debug) logger.TLog(this.GetType().Name, "RpcChangeBaseColor|" + color);
            _currentColor = color;
            tokenModel.ChangeBaseColor(color);
        }
        #endregion


        void SeparatePlayerFromToken()
        {
            //gameObject.transform.SetParent(GameObject.Find("Tokens").GetComponent<Transform>());
            //ownedBy.GetComponent<CharacterController>().center = new Vector3(0, 0, 0);
            //ownedBy.transform.position -= new Vector3(0, 0, 6);
            //currentlyInFocus = false;
        }

    }
}

