



//using System.Collections.Generic;
//using System.Text;
//using UnityEngine;

//namespace Sazboom
//{
//    public class Distances : MonoBehaviour
//    {
//        //GameMode Set IsMoveMode
//        public GameModeHelper gameModeHelper;

//        //Mouse Movement
//        public RaycastHelper raycastHelper;
//        public RaycastHit raycastHit;

//        //Token Selected Action
//        public SelectAToken selectAToken;
//        public GameObject selectedToken;

//        public List<Vector3> distanceQueue = new List<Vector3>();


//        private void Awake()
//        {
//            //Gamemode Set IsMoveMode
//            gameModeHelper = gameObject.GetComponent<GameModeHelper>();

//            //Used for getting Raycast hit
//            raycastHelper = gameObject.GetComponent<RaycastHelper>();

//            //Selected Token Action
//            SelectAToken.OnTokenSelected += SetSelectedToken;



//        }

//        private void Update()
//        {
//            if (gameModeHelper.IsMove())
//            {
//                //Distance to Mouse Pointer
//                if (Input.GetKey(KeyCode.LeftShift))
//                {
//                    DistanceToCursor();
//                    if (Input.GetMouseButtonDown(0))
//                    {
//                        AddDistanceMarker();
//                    }
//                }

//                //Add Distance Marker
//               // else if (Input.GetMouseButtonDown(0)) AddDistanceMarker();

//                //Remove Distance Marker
//               // else if (Input.GetMouseButtonDown(1)) RemoveDistanceMarker();
               
//            }
//        }

        
//        //Distance To Mouse Pointer
//        private void DistanceToCursor()
//        {
//            if (raycastHelper.IsRaycastHitWalkable(out raycastHit, false) && selectedToken)
//            {

//                updateInfobarWithDistance();
//            }

//        }

//        //Add Distance Marker
//        private void AddDistanceMarker()
//        {
//            if(raycastHelper.IsRaycastHitWalkable(out raycastHit, false))
//            {
//                distanceQueue.Add(raycastHit.point);
//                updateInfobarWithDistance();

//            }
//        }

//        //Remove Distance Marker
//        private void RemoveDistanceMarker()
//        {
//        }

//        //Set Selected Token
//        private void SetSelectedToken(GameObject token)
//        {
//            selectedToken = token;
//        }

//        public float DistanceBetween(Vector3 origin, Vector3 dest)
//        {
//            float distance = Mathf.Ceil(Vector3.Distance(origin, dest));
//            return distance;

//        }

//        public float TotalDistance()
//        {
//            float totalDist = DistanceBetween(selectedToken.transform.position, distanceQueue[0]);
//            for (int x = 0; x < distanceQueue.Count-1; x++)
//            {
//                totalDist += DistanceBetween(distanceQueue[x], distanceQueue[x+1]);
//            }
//            return totalDist;
//        }


//        public void updateInfobarWithDistance()
//        {
//            float distance = 0;
//            float totalDistance = 0;
//            float pathDistance = 0;
//            float moveDistance = 0;
//            if (distanceQueue.Count > 0)
//            {
//                distance = DistanceBetween(distanceQueue[distanceQueue.Count - 1], raycastHit.point);
//                pathDistance = TotalDistance();
//                totalDistance = pathDistance + distance;
//            }
//            else
//            {
//                distance = DistanceBetween(selectAToken.transform.position, raycastHit.point);
//            }
//            StringBuilder sb = new StringBuilder();
//            sb.Append("Leg Dist:");
//            sb.Append(distance.ToString());
//            sb.Append(" | Total Dist:");
//            sb.Append(totalDistance.ToString());
//            sb.Append(" | Path Dist:");
//            sb.Append(pathDistance.ToString());            
//            sb.Append(" | Move Dist:");
//            sb.Append(moveDistance.ToString());

            
//            BottomInfobar.OnBottomInfobarUpdate(sb.ToString());
//        }



//    }

//}
