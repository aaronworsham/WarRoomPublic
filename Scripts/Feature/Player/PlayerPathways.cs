using System;
using System.Collections.Generic;
using UnityEngine;


namespace Sazboom.WarRoom
{
    [RequireComponent(typeof(CameraController))]
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerWaypoints))]
    [RequireComponent(typeof(NetworkLogger))]
    public class PlayerPathways : MonoBehaviour
    {
        readonly bool debug = true;
        [SerializeField] private CameraController cameraController;
        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerWaypoints playerWaypoints;
        [SerializeField] private NetworkLogger logger;

        [SerializeField] private Material redMat;
        [SerializeField] private Material blackMat;
        [SerializeField] private Material blueMat;
        [SerializeField] private Material greenMat;
        [SerializeField] private Material yellowMat;
        [SerializeField] private Material orangeMat;
        [SerializeField] private Material purpleMat;
        [SerializeField] private Material cyanMat;
        Dictionary<string, Material> materialNames = new Dictionary<string, Material>();
        void OnValidate()
        {
            if (logger == null)
            {
                logger = GetComponent<NetworkLogger>();
            }
            if (cameraController == null)
            {
                cameraController = GetComponent<CameraController>();
            }
            if (playerController == null)
            {
                playerController = GetComponent<PlayerController>();
            }
            if (playerWaypoints == null)
            {
                playerWaypoints = GetComponent<PlayerWaypoints>();
            }
        }


        void Awake()
        {

            materialNames.Add("red", redMat);
            materialNames.Add("blue", blueMat);
            materialNames.Add("green", greenMat);
            materialNames.Add("yellow", yellowMat);
            materialNames.Add("orange", orangeMat);
            materialNames.Add("purple", purpleMat);
            materialNames.Add("cyan", cyanMat);

        }

        public void PathwayToCursor()
        {
            Vector3 hitPoint;
            if (IsWalkable(out hitPoint))
            {
                if (debug) logger.TLog(this.GetType().Name, "PathwayToCursor: " + hitPoint);
                playerController.CallCmdAddPathway(gameObject.transform.position, hitPoint + new Vector3(0, 2, 0));
            }

        }

        public void RemoveCurrentPathway()
        {
            playerController.CallCmdRemoveCurrentPathway();
        }

        public void PathwayToWaypoint()
        {
            Vector3 dest = playerController.CurrentWaypoint.transform.position;
            if (debug) logger.TLog(this.GetType().Name, "PathwayToWaypoint: " + dest);
            playerController.CallCmdAddPathway(gameObject.transform.position, dest);

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

        public void ChangeColor(GameObject wp, string newColor)
        {
            Material newMat;
            if (materialNames.ContainsKey(newColor))
            {
                newMat = materialNames[newColor];
                if (debug) logger.TLog(this.GetType().Name, "ChangeColor|" + newColor);
                LineRenderer lr = wp.transform.Find("Line").GetComponent<LineRenderer>();
                lr.material = newMat;
            }
            else
                if (debug) logger.WLog(this.GetType().Name, "ChangeColor|Cannot find " + newColor);

        }

    }
}

//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.AI;

//namespace Sazboom
//{
//    public class Paths : MonoBehaviour
//    {
//        bool debug = false;

//        //GameMode Set IsMoveMode
//        public GameModeHelper gameModeHelper;

//        //Mouse Movement
//        public RaycastHelper raycastHelper;
//        public RaycastHit raycastHit;

//        //Token Selected Action
//        public SelectAToken selectAToken;
//        public GameObject selectedToken = null;

//        //Multi Waypoint
//        public bool awaitingLateUpdateForMultiPathway;
//        public Vector3 multiPathwayLocation;


//        [SerializeField] private Waypoints waypoints;
//        [SerializeField] private MoveCharacterTBS moveCharacterTBS;
//        [SerializeField] private LineRenderer pathway;

//        [SerializeField] private NavMeshAgent _navMeshAgent;

//        public List<Vector3> pathwayQueue = new List<Vector3>();

//        private void Awake()
//        {
//            //Gamemode Set IsMoveMode
//            gameModeHelper = gameObject.GetComponent<GameModeHelper>();

//            //Used for getting Raycast hit
//            raycastHelper = gameObject.GetComponent<RaycastHelper>();


//            //Selected Token Action
//            SelectAToken.OnTokenSelected += SetSelectedToken;

//            waypoints = FindObjectOfType<Waypoints>();
//            moveCharacterTBS = FindObjectOfType<MoveCharacterTBS>();
//            moveCharacterTBS.OnMoveLegEnded += HandleMoveLegEnded;
//            moveCharacterTBS.OnMoveComplete += HandleMoveComplete;

//            pathway = gameObject.GetComponent<LineRenderer>();

//            _navMeshAgent = GetComponent<NavMeshAgent>();

//        }

//        private void Update()
//        {
//            if (gameModeHelper.IsMove())
//            {

//                if (Input.GetMouseButtonDown(0))
//                {
//                    if (debug) Debug.Log("Path 2");
//                    AddPathwayMarker();
//                }
//                else if (Input.GetMouseButtonDown(1))
//                {
//                    if (debug) Debug.Log("Path 3");
//                    RemoveLastPathwayMarker();
//                }
//            }
//        }


//        private void HandleMoveLegEnded()
//        {
//            if (debug) Debug.Log("Path 13");
//            RemoveNextPathwayMarker();
//        }

//        private void HandleMoveComplete()
//        {
//            if (debug)Debug.Log("Path 12");
//            ClearPathway();
//        }

//        //Set Selected Token
//        private void SetSelectedToken(GameObject token)
//        {
//           if (debug)Debug.Log("Path 8");
//            selectedToken = token;
//            ClearPathway();

//        }



//        //Pathway To Waypoint Marker
//        private void AddPathwayMarker()
//        {
//            if (debug) Debug.Log("Path 4");
//            if (raycastHelper.IsRaycastHitWalkable(out raycastHit, false) && selectedToken) 
//            {
//                if (debug) Debug.Log("Path 5");
//                if (pathwayQueue.Count == 0)
//                {
//                    if (debug) Debug.Log("Path 6");
//                    if (debug) Debug.Log(selectedToken.transform.position);
//                    if (debug) Debug.Log(pathwayQueue.Count);
//                    pathwayQueue.Add(selectedToken.transform.position);
//                }
//                pathwayQueue.Add(raycastHit.point + new Vector3(0, 1, 0));
//                DrawMultiLine();
//            }
//        }

//        //Pathway To Waypoint Marker
//        private void RemoveLastPathwayMarker()
//        {
//            if (debug) Debug.Log("Path 7");
//            pathwayQueue.RemoveAt(pathwayQueue.Count - 1);
//            DrawMultiLine();
//        }

//        private void RemoveNextPathwayMarker()
//        {
//            if (debug)Debug.Log("Path 11");
//            if (pathwayQueue.Count != 0)
//            {
//                pathwayQueue.RemoveAt(0);
//                DrawMultiLine();
//            }

//        }



//        private void DrawMultiLine()
//        {
//            if (debug)Debug.Log("Path 10");
//            pathway.positionCount = pathwayQueue.Count;
//            int index = 0;
//            foreach (Vector3 pos in pathwayQueue)
//            {
//                pathway.SetPosition(index, pos);
//                index++;
//            }

//        }

//        private void ClearPathway()
//        {
//            if (debug)Debug.Log("Path 9");
//            pathway.positionCount = 0;
//            pathwayQueue.Clear();
//        }

 


//    }
//}

