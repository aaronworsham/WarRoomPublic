using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;
using System.Linq;
using System.Text;

namespace Sazboom.WarRoom
{
    public class PlayerFamily : NetworkBehaviour
    {
        bool debug = true;
        public NetworkLogger logger;

        private List<GameObject> playerFamily = new List<GameObject>();

        #region CommandBar Call [RUN ON CLIENT]

        void details()
        {
            StringBuilder str = new StringBuilder();
            str.Append("Player Family \n");
            foreach(GameObject obj in playerFamily)
            {
                str.Append("-" + obj.name + "\n");
            }
            Debug.Log(str);
        }

        #endregion

        #region Callbacks & Events
        public void Awake()
        {
            logger = GetComponent<NetworkLogger>();
        }


        //Add event handlers here
        public override void OnStartClient()
        {
            base.OnStartClient();
        }

        public override void OnStopClient()
        {
            base.OnStopClient();


        }


        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();


        }

        public void AddTokenToFamily(GameObject target)
        {

        }

        #endregion

        #region Instance Methods

        public void AddToFamiliy(GameObject target)
        {
            if (!playerFamily.Contains(target))
            {
                playerFamily.Add(target);
                if (debug) details();
            }
        }

        public void ChangeToPlayer()
        {
            //if (debug) logger.TLog(this.GetType().Name, "ChangeFamilyFocus|F1 Key");
            //OnChangeFocus?.Invoke(currentFocus, gameObject);
            //this.currentFocus = gameObject;
        }

        public void ChangeForwardInFamily()
        {

        }

        public void ChangeBackwardInFamily()
        {

        }

        //public void ChangeFamilyFocus(int index)
        //{
        //    if (!hasAuthority) return;
        //    if (index > 10) return;
        //    if (this.family.Count <= index) return;

        //    if (debug) logger.TLog(this.GetType().Name, "ChangeFamilyFocus|Index: " + index);
        //    if (debug) logger.TLog(this.GetType().Name, "ChangeFamilyFocus|Obj: " + this.family[index]);

        //    OnChangeFocus?.Invoke(this.family[index], gameObject);
        //    this.currentFocus = this.family[index];
        //}

        #endregion
    }
}


