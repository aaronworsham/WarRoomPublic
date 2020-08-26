using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Mirror;
using Mirror.Cloud.ListServerService;

namespace Sazboom
{
    namespace WarRoom
    {
        public class ServerPlayerFamily : NetworkBehaviour
        {

            //Debugger
            readonly bool debug = true;

            Dictionary<uint, List<GameObject>> playerFamily = new Dictionary<uint, List<GameObject>>();

            #region Callbacks

            public override void OnStartServer()
            {
                base.OnStartServer();
            }

            #endregion


            #region Server Methods
            [Server]
            void details()
            {
                StringBuilder str = new StringBuilder();
                str.Append("Server Player Families \n");
                foreach (KeyValuePair<uint, List<GameObject>> pair in playerFamily)
                {
                    str.Append("-" + pair.Key + "\n");
                    foreach(GameObject obj in pair.Value)
                    {
                        str.Append(" |-" + obj.name + "\n");
                    }
                }
                Debug.Log(str);
            }


            [Server]
            public void AddToFamily(uint id, GameObject target)
            {
                if (debug) Debug.Log("AddToFamily");
                if (!playerFamily.ContainsKey(id))
                {
                    if (debug) Debug.Log("AddToFamily|Adding New List with Target:"+target.name);
                    playerFamily[id] = new List<GameObject> { target };
                    if (debug) Debug.Log("AddToFamily|# Server Player Families:"+playerFamily.Count);
                    if(debug) details();
                }
                else
                {
                    if(!playerFamily[id].Contains(target))
                    {
                        if (debug) Debug.Log("AddToFamily|Adding to Existing List with Target:"+target.name);
                        playerFamily[id].Add(target);
                    }
                }
            }

            #endregion



        }

    }
}

