using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallbackMap : NetworkBehaviour
{
    public override void OnStartServer()
    {
        base.OnStartServer();
        Debug.Log("On Start Server from" + gameObject.name);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log("On Start Client from" + gameObject.name);
        
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        Debug.Log("On Start Authority from " + gameObject.name);
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log("On Start Local Player from " + gameObject.name);
    }
}
