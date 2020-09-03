using System;
using System.Text;
using UnityEngine;


namespace Sazboom.WarRoom
{
    //[RequireComponent(typeof(CameraController))]
    //[RequireComponent(typeof(NetworkLogger))]
    //public class PlayerDistances : MonoBehaviour
    //{
    //    readonly bool debug = false;
    //    [SerializeField] private CameraController cameraController;
    //    [SerializeField] private NetworkLogger logger;

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
    //    }

    //    //Distance To Mouse Pointer
    //    public string DistanceToCursor()
    //    {
    //        Vector3 hitPoint;
    //        if (IsWalkable(out hitPoint))
    //        {
    //            StringBuilder str = new StringBuilder();
    //            float dist = DistanceBetween(gameObject.transform.position, hitPoint);
    //            str.Append("---Distances---").AppendLine();
    //            str.Append("To Cursor: " + dist.ToString()).AppendLine();
    //            return str.ToString();
    //        }
    //        return "";

    //    }

    //    public string DistanceToWaypoint(Vector3 point)
    //    {
    //        StringBuilder str = new StringBuilder();
    //        float dist = DistanceBetween(gameObject.transform.position, point);
    //        str.Append("To Waypoint: " + dist).AppendLine();
    //        return str.ToString();
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

    //    public float DistanceBetween(Vector3 origin, Vector3 dest)
    //    {
    //        float distance = Mathf.Ceil(Vector3.Distance(origin, dest));
    //        return distance;

    //    }




    //}


}


