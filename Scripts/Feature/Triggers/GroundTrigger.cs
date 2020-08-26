using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTrigger : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("COLLISION");
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("TRIGGER");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log("CONTROLLER COLLIDE");
    }
}
