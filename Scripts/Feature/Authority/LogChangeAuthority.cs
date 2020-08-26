using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogChangeAuthority : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log("Just Got Authority");
        Debug.Log("Called from: " +gameObject.name);
    }
}
