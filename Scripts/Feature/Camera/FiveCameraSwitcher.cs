using Mirror;
using System;
using UnityEngine;

namespace Sazboom.WarRoom
{
    public class FiveCameraSwitcher : NetworkBehaviour, ICameraSwitchable
    {

        readonly bool debug = false;
        string dHeader;
        public NetworkLogger logger;

        public Camera isoCam;
        public Camera firstPersonCam;
        public Camera thirdPersonCam;
        public Camera deepCam;
        public Camera topDownCam;


        #region Callbacks & Events


        public override void OnStartClient()
        {
            base.OnStartServer();
            DisableAllCams();
        }


        #endregion

        public Camera EnableFirstPersonCam()
        {
            if (debug) logger.TLog(this.GetType().Name, "Enabling 1st person cam for " + gameObject.name);
            if (firstPersonCam) firstPersonCam.enabled = true;
            if (thirdPersonCam) thirdPersonCam.enabled = false;
            if (deepCam) deepCam.enabled = false;
            if (isoCam) isoCam.enabled = false;
            if (topDownCam) topDownCam.enabled = false;
            return firstPersonCam;
        }
        public Camera EnableThirdPersonCam()
        {
            if (debug) logger.TLog(this.GetType().Name, "Enabling 3rd person cam for " + gameObject.name);
            if (firstPersonCam) firstPersonCam.enabled = false;
            if (thirdPersonCam) thirdPersonCam.enabled = true;
            if (deepCam) deepCam.enabled = false;
            if (isoCam) isoCam.enabled = false;
            if (topDownCam) topDownCam.enabled = false;
            return thirdPersonCam;

        }

        public Camera EnableDeepCam()
        {
            if (debug) logger.TLog(this.GetType().Name, "Enabling Deep cam for " + gameObject.name);
            if (firstPersonCam) firstPersonCam.enabled = false;
            if (thirdPersonCam) thirdPersonCam.enabled = false;
            if (deepCam) deepCam.enabled = true;
            if (isoCam) isoCam.enabled = false;
            if (topDownCam) topDownCam.enabled = false;
            return deepCam;

        }
        public Camera EnableIsoCam()
        {
            if (debug) logger.TLog(this.GetType().Name, "Enabling Iso cam for " + gameObject.name);
            if (firstPersonCam) firstPersonCam.enabled = false;
            if (thirdPersonCam) thirdPersonCam.enabled = false;
            if (deepCam) deepCam.enabled = false;
            if (isoCam) isoCam.enabled = true;
            if (topDownCam) topDownCam.enabled = false;
            return isoCam;
        }
        public Camera EnableTopDownCam()
        {
            if (debug) logger.TLog(this.GetType().Name, "Enabling Top Down cam for " + gameObject.name);
            if (firstPersonCam) firstPersonCam.enabled = false;
            if (thirdPersonCam) thirdPersonCam.enabled = false;
            if (deepCam) deepCam.enabled = false;
            if (isoCam) isoCam.enabled = false;
            if (topDownCam) topDownCam.enabled = true;
            return topDownCam;
        }

        public void DisableAllCams()
        {
            if (debug) logger.TLog(this.GetType().Name, "Disabling all cams for " + gameObject.name);
            if (firstPersonCam) firstPersonCam.enabled = false;
            if (thirdPersonCam) thirdPersonCam.enabled = false;
            if (deepCam) deepCam.enabled = false;
            if (isoCam) isoCam.enabled = false;
            if (topDownCam) topDownCam.enabled = false;
        }

    }
}


