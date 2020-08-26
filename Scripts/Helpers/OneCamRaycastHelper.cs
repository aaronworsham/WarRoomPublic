using UnityEngine;

public class OneCamRaycastHelper : MonoBehaviour
{

    bool debug = false;

    //Mouse Movement
    public Camera activeCamera;


    public bool IsRaycastHitWalkable(out RaycastHit raycastHit, bool debug)
    {
        Ray ray = SetRaycastFromCamera(debug);
        int terrainLayer = 1 << LayerMask.NameToLayer("Terrain");
        int tokenLayer = 1 << LayerMask.NameToLayer("Token");
        int mask = terrainLayer | tokenLayer;

        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity, mask))
        { if(debug) Debug.Log("RayWalk2");
            if(raycastHit.transform.tag == "Player_Token")
            {
                if (debug) Debug.Log("Raywalk4");
                return false;
            }
            
            else if(raycastHit.transform.tag == "Walkable")
            {
                if(debug)Debug.Log("RayWalk3");
                return true;
            }
            else
            {
                if(debug)Debug.Log("Is not Walkable");
                if(debug)Debug.Log(raycastHit.transform.name);
                if (debug) Debug.Log(raycastHit.transform.tag);
                return false;
            }
        }
        else return false;
        
    }

    public bool IsRaycastHitAPlayerToken(out RaycastHit raycastHit, bool debug)
    {
        Ray ray = SetRaycastFromCamera(debug);
        if (Physics.Raycast(ray, out raycastHit, Mathf.Infinity,
             (1 << LayerMask.NameToLayer("Terrain") |
             (1 << LayerMask.NameToLayer("Token")))))
        {
            if (debug) Debug.Log("RayToken2");
            return raycastHit.transform.tag == "Player_Token";
        }
        else
        {
            Debug.Log("Not Player Token");
            if(raycastHit.transform) Debug.Log(raycastHit.transform.name);
            return false;
        }
    }

    //Get mouse World Location
    public Ray SetRaycastFromCamera(bool debug)
    {
        if(debug)Debug.Log("RayCamera1");
        return activeCamera.ScreenPointToRay(Input.mousePosition);
    }

}
