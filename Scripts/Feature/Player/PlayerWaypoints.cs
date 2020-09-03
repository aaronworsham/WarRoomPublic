using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Sazboom.WarRoom
{        
    //[RequireComponent(typeof(CameraController))]
    //[RequireComponent(typeof(PlayerController))]
    //[RequireComponent(typeof(NetworkLogger))]
    //public class PlayerWaypoints : MonoBehaviour
    //{
    //    readonly bool debug = true;
    //    [SerializeField] private CameraController cameraController;
    //    [SerializeField] private PlayerController playerController;
    //    [SerializeField] private NetworkLogger logger;
    //    [SerializeField] private Vector3 waypointLocationOffset = new Vector3(0,0,0);

    //    [SerializeField] private Material redMat;
    //    [SerializeField] private Material blackMat;
    //    [SerializeField] private Material blueMat;
    //    [SerializeField] private Material greenMat;
    //    [SerializeField] private Material yellowMat;
    //    [SerializeField] private Material orangeMat;
    //    [SerializeField] private Material purpleMat;
    //    [SerializeField] private Material cyanMat;
    //    Dictionary<string, Material> materialNames = new Dictionary<string, Material>();


    //    void OnValidate()
    //    {
    //        if (logger == null)
    //        {
    //            logger = GetComponent<NetworkLogger>();
    //        }
    //        if (cameraController == null)
    //        {
    //            cameraController = GetComponent<CameraController>();
    //        }
    //        if (playerController == null)
    //        {
    //            playerController = GetComponent<PlayerController>();
    //        }
    //    }

    //    void Awake()
    //    {

    //        materialNames.Add("red", redMat);
    //        materialNames.Add("blue", blueMat);
    //        materialNames.Add("green", greenMat);
    //        materialNames.Add("yellow", yellowMat);
    //        materialNames.Add("orange", orangeMat);
    //        materialNames.Add("purple", purpleMat);
    //        materialNames.Add("cyan", cyanMat);

    //    }

    //    //Distance To Mouse Pointer
    //    public void WaypointToCursor()
    //    {
    //        Vector3 hitPoint;
    //        if (IsWalkable(out hitPoint))
    //        {
    //            if (debug) logger.TLog(this.GetType().Name, "WaypointToCursor: "+hitPoint);
    //            playerController.CallCmdAddWaypoint(hitPoint + waypointLocationOffset);
    //        }

    //    }

    //    public void RemoveCurrentWaypoint()
    //    {
    //        playerController.CallCmdRemoveCurrentWaypoint();
    //    }


    //    public bool IsWalkable(out Vector3 target)
    //    {
    //        Ray ray = SetRaycastFromCamera();
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

    //    public void ChangeColor(GameObject wp, string newColor)
    //    {
    //        Material newMat;
    //        if (materialNames.ContainsKey(newColor))
    //        {
    //            newMat = materialNames[newColor];
    //            if (debug) logger.TLog(this.GetType().Name, "ChangeColor|" + newColor);
    //            wp.transform.Find("Base").GetComponent<MeshRenderer>().material = newMat;
    //            wp.transform.Find("Sphere").GetComponent<MeshRenderer>().material = newMat;
    //        }
    //        else
    //            if (debug) logger.WLog(this.GetType().Name, "ChangeColor|Cannot find " + newColor);

    //    }

    //}
}



//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;
//using Mirror;
//using Mirror.Examples.Basic;

//namespace Sazboom 
//{ 
//    public class Waypoints : NetworkBehaviour

//    {

//        bool debug = false;
//        public Color soulColor;

//        //Mouse Movement
//        public RaycastHelper raycastHelper;
//        public RaycastHit raycastHit;

//        //Cursor Waypoint
//        public GameObject cursorWaypoint;
//        public bool awaitingLateUpdateForCursorWaypoint;
//        public Vector3 cursorWaypointLocation;

//        //Multi Waypoint
//        public bool awaitingLateUpdateForMultiWaypoint;
//        public Vector3 multiWaypointLocation;
//        public List<GameObject> waypointQueue  = new List<GameObject>();
        
//        //Waypoint Marker
//        public GameObject waypointMarker;
//        public int waypointIndex = 0;
//        public Vector3 elevated = new Vector3(0, 1, 0);

//        //TokenData
//        public TokenData tokenData;
//        public GameObject playerSoul;

        


//        public void Awake()
//        {
            
//            //Used for getting Raycast hit
//            raycastHelper = gameObject.GetComponent<RaycastHelper>();
//            tokenData.OnSoulAddedToToken += HandleSoulAddedToToken;
        
//        }



//        private void Update()
//        {
//            if (!hasAuthority) return;

            
//            if (Input.GetKey(KeyCode.LeftShift))
//            {
//                WaypointToCursor();
//                if (Input.GetMouseButtonDown(0))
//                {
//                    MultiWaypoint();
//                }

//            }
//            else if (Input.GetKeyUp(KeyCode.LeftShift))
//            {
                
//                ClearWaypointToCursor();
                    
//            }
//            else if (Input.GetMouseButtonDown(0))
//            {

//                MultiWaypoint();
                    
//            }
//            else if (Input.GetMouseButtonDown(1))
//            {

//                ClearWaypoints();
                    
//            }

                

//        }

//        private void HandleSoulAddedToToken(GameObject soul)
//        {
//            Debug.Log(name + " | Waypoint | Action handler for HandleSoulAddedToToken");
//            playerSoul = soul;
//            soulColor = GetSoulColor();
//        }

//        public void ClientConnect()
//        {
//            ClientScene.RegisterPrefab(waypointMarker);
//            NetworkClient.RegisterHandler<ConnectMessage>(OnClientConnect);
//            NetworkClient.Connect("localhost");
//        }

//        void OnClientConnect(NetworkConnection conn, ConnectMessage msg)
//        {
//            Debug.Log(name + " | OnClientConnect |Connected to server: " + conn);
//        }


//        //private void HandleMoveLegEnd()
//        //{
//        //        if (debug) Debug.Log("Way7");
//        //    ClearNextWaypoint();
//        //}

//        //private void HandleMoveComplete()
//        //{
//        //        if (debug) Debug.Log("Way8");
//        //    ClearWaypoints();
//        //}

//        //Waypoint To Mouse Pointer
//        private void WaypointToCursor()
//        {
//            if (raycastHelper.IsRaycastHitWalkable(out raycastHit, debug))
//            {
//                cursorWaypointLocation = raycastHit.point;
//                cursorWaypointLocation += elevated;
//                AddCursorWaypoint(cursorWaypointLocation);
//            }
                
//        }

//        private void MultiWaypoint()
//        {
//            if (raycastHelper.IsRaycastHitWalkable(out raycastHit, debug))
//            {
//                multiWaypointLocation = raycastHit.point;
//                multiWaypointLocation += elevated;
//                AddMulitWaypoint(multiWaypointLocation);
//            }
//            else if (debug)
//            {
//                Debug.Log(name + " | MultiWaypoint | Not Walkable Terrain");
//            }
//        }



//        private void ClearWaypointToCursor()
//        {
//            ClearCursorWaypoint();
//        }


//        //Distance To Waypoint Marker
//        private void RemoveWaypointMarker()
//        {
//        }


//        public List<Vector3> HandleOnMoveToStart()
//        {
//            List<Vector3> tempList = new List<Vector3>();
//            foreach(GameObject  obj in waypointQueue)
//            {
//                tempList.Add(obj.transform.position);
//            }
//            return tempList;
//        }

//        //Cursor Waypoints

//        public void AddCursorWaypoint(Vector3 point)
//        {
//            if (cursorWaypoint)
//           {
//                cursorWaypoint.transform.position = point;
//            }
//            else
//            {

//                cursorWaypoint = InstantiateWaypoint(point);
//                cursorWaypoint.GetComponent<Renderer>().material.SetColor("_Color", soulColor);
//            }
            
//        }

//        public void ClearCursorWaypoint() 
//        {
//            Destroy(cursorWaypoint);
//        }


//        //Multi Waypoints
//        public void AddMulitWaypoint(Vector3 point)
//        {
//            //waypointQueue.Add(InstantiateWaypoint(point));
//            SpawnWaypoint(point);
//        }



//        public GameObject InstantiateWaypoint(Vector3 point)
//        {
//            return Instantiate(waypointMarker, point, Quaternion.identity);

//        }

//        public void SpawnWaypoint(Vector3 point)
//        {
//            CmdSpawnWaypoint(point);
//        }



//        [Command]
//        public void CmdSpawnWaypoint(Vector3 point)
//        {
            
//            GameObject waypoint = Instantiate(waypointMarker, point, Quaternion.identity);
//            waypoint.GetComponent<Renderer>().material.SetColor("_Color", soulColor);
//            waypointQueue.Add(waypoint);
//            NetworkServer.Spawn(waypoint);
//        }

//        public void ClearLastWaypoint()
//        {
//            GameObject loc = waypointQueue[waypointQueue.Count-1];
//            waypointQueue.RemoveAt(waypointQueue.Count-1);
//            Destroy(loc);

//        }        
        
//        public void ClearNextWaypoint()
//        {
//            GameObject loc = waypointQueue[0];
//            waypointQueue.RemoveAt(0);
//            Destroy(loc);

//        }
        
//        public void ClearWaypoints()
//        {
//            CmdClearWaypoints();
//        }

//        [Command] 
//        public void CmdClearWaypoints()
//        {
//            foreach(GameObject point in waypointQueue)
//            {
//                Destroy(point);
//            }
//            waypointQueue.Clear();
//        }

//        //private Color GetSoulColor()
//        //{
//        //    Debug.Log(name + " | Waypoint | GetSoulColor | Getting Soul Color");
//        //    Color color = playerSoul.GetComponent<SoulColorClient>().GetSoulColor();
//        //    Debug.Log(name + " | Waypoint | GetSoulColor | Got Soul Color: " + color);
//        //    return color;
//        //}


//    }


//}

