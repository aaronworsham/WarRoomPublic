//using Mirror;
//using UnityEngine;

//public class RaycastHelper : NetworkBehaviour
//{

//    bool debug = false;
//    string dHeader;
//    public NetworkLogger logger;


//    #region Callbacks & Events
//    public void Awake()
//    {
//        dHeader = gameObject.name;
//        logger = gameObject.GetComponent<NetworkLogger>();
//    }

//    public override void OnStartClient()
//    {
//        base.OnStartClient();
//    }

//    public override void OnStopClient()
//    {
//        base.OnStopClient();
//    }
//    public override void OnStartLocalPlayer()
//    {
//        base.OnStartLocalPlayer();
//        dHeader = LoggerHelper.DebugHeader(gameObject, this.GetType().Name);
//    }

//    #endregion

//    public bool IsRaycastHitWalkable(out RaycastHit raycastHit, bool debug)
//    {
//        Ray ray = SetRaycastFromCamera(debug);
//        int terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
//        int tokenLayer = 1 << LayerMask.NameToLayer("Token");
//        int mask = terrainLayer | tokenLayer;

//        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, mask))
//        {
//            if (debug) logger.TLog(this.GetType().Name, "RayWalk2");
//            if(raycastHit.transform.tag == "Player_Token")
//            {
//                if (debug) logger.TLog(this.GetType().Name, "Raywalk4");
//                return false;
//            }
            
//            else if(raycastHit.transform.tag == "Walkable")
//            {
//                if(debug)logger.TLog(this.GetType().Name, "RayWalk3");
//                return true;
//            }
//            else
//            {
//                if (debug) logger.TLog(this.GetType().Name, "Is not Walkable");
//                if (debug) logger.TLog(this.GetType().Name, raycastHit.transform.name);
//                if (debug) logger.TLog(this.GetType().Name, raycastHit.transform.tag);
//                return false;
//            }
//        }
//        else return false;
        
//    }

//    public bool IsRaycastHitAPlayerToken(out RaycastHit raycastHit, bool debug)
//    {
//        Ray ray = SetRaycastFromCamera(debug);
//        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity,
//             (1 << LayerMask.NameToLayer("Terrain") |
//             (1 << LayerMask.NameToLayer("Token")))))
//        {
//            if (debug) logger.TLog(this.GetType().Name, "RayToken2");
//            return raycastHit.transform.tag == "Player_Token";
//        }
//        else
//        {
//            if (debug) logger.TLog(this.GetType().Name, "Not Player Token");
//            if(raycastHit.transform) Debug.Log(raycastHit.transform.name);
//            return false;
//        }
//    }

//    //Get mouse World Location
//    public Ray SetRaycastFromCamera(bool debug)
//    {
//        if (debug) logger.TLog(this.GetType().Name, "RayCamera1");
//        if (debug) logger.TLog(this.GetType().Name, "Current Camera: " + player.currentCamera.name);
//        return player.currentCamera.ScreenPointToRay(Input.mousePosition);
//    }

//}
