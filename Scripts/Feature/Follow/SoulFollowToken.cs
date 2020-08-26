using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulFollowToken : NetworkBehaviour
{
    readonly bool debug = false;
    public NetworkLogger logger;

    GameObject tokenToFollow;

    #region Callbacks & Events

    //Add event handlers here

    public void Awake()
    {
        logger = gameObject.GetComponent<NetworkLogger>();
    }

    //public override void OnStartClient()
    //{
    //    base.OnStartServer();
    //    SelectAToken.OnTokenSelected += HandleTokenSelected;
    //    Family.OnChangeFamilyFocus += HandleChangeFamilyFocus;
    //}

    //public override void OnStopClient()
    //{
    //    base.OnStopServer();
    //    SelectAToken.OnTokenSelected -= HandleTokenSelected;
    //    Family.OnChangeFamilyFocus -= HandleChangeFamilyFocus;
    //}

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        if (debug) logger.TLog(this.GetType().Name, "OnStartLocalPlayer");
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasAuthority) return;
        if (!isLocalPlayer) return;
        if (tokenToFollow == null) return;
            
        transform.position = tokenToFollow.transform.position + new Vector3(0, 2, 0);
    }
    private void HandleTokenSelected(GameObject token, GameObject soul)
    {
        tokenToFollow = token;
    }

    private void HandleChangeFamilyFocus(GameObject obj)
    {
        if (debug) logger.TLog(this.GetType().Name, "HandleChangeFamilyFocus|Obj: " + obj.name);

        if (UnityEngine.Object.ReferenceEquals(obj, gameObject))
        {
            //Attempt to fix bug where the soul starts over head of token.
            Vector3 backTwo = tokenToFollow.transform.position + new Vector3(0, 0, -2);
            tokenToFollow = null;
            transform.position = backTwo;
        }
        else
        {
            tokenToFollow = obj;
        }



    }
    #endregion


}
