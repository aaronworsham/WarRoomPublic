using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AuthorityCoordinator : NetworkBehaviour
{

    readonly bool debug = false;
    string dHeader;
    public NetworkLogger logger;

    public void Awake()
    {
        dHeader = this.GetType().Name + "|";
        logger = gameObject.GetComponent<NetworkLogger>();
    }
    
    public bool PlayerOwnsIt(NetworkIdentity id)
    {
        return id.connectionToClient != null;
    }

    public bool PlayerDoesNotOwnIt(NetworkIdentity id)
    {
        return id.connectionToClient == null;
    }
    
    public void WhoHasAuthority()
    {
        foreach (KeyValuePair<uint, NetworkIdentity> spawn in NetworkIdentity.spawned)
        {
            if (debug) logger.TLog(this.GetType().Name, "WhoHasAuthority|Key: " + spawn.Key);
            if (debug) logger.TLog(this.GetType().Name, "WhoHasAuthority|Value: " + spawn.Value);
            if (debug) logger.TLog(this.GetType().Name, "WhoHasAuthority|Client Owned Objects");
            if (spawn.Value.connectionToClient == null)
            {
                if (debug) logger.TLog(this.GetType().Name, "WhoHasAuthority|Client Connection Owned");
            }
            else
            {
                if (debug) logger.TLog(this.GetType().Name, "WhoHasAuthority|Client Connection Owned (Someone Has Authority)");
            } 
        }
    }

}
