using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using Mirror;

namespace Sazboom
{
    namespace WarRoom
    {
        public class ServerPlayerNames : NetworkBehaviour 
        {

            //Debugger
            readonly bool debug = false;

            Dictionary<uint, string> assignedPlayerNames = new Dictionary<uint, string>();
            
            #region Server Methods
            
            [Server]
            public string GetAssignedPlayerName(uint id)
            {
                if (debug) Debug.Log("GetAssignedColor|ID: " + id);
                if (assignedPlayerNames.ContainsKey(id))
                {
                    return assignedPlayerNames[id];
                }
                else
                {
                    string name = PickName(id);
                    LogPlayerWithName(id, name);
                    return name;
                }
            }
            
            [Server]
            public void LogPlayerWithName(uint id, string name)
            {
                if (debug) Debug.Log("LogPlayerWithName|ID: "+id+" Name:"+ name);
                if (!assignedPlayerNames.ContainsKey(id))
                {
                    if (debug) Debug.Log("LogPlayerWithName|Logging Name to ID");
                    assignedPlayerNames.Add(id, name);
                }
            }

            [Server]
            public string PickName(uint id)
            {
                if (debug) Debug.Log("PickName");
                string name = "Player " + (assignedPlayerNames.Count + 1);
                return name;
            }

            #endregion



        }

    }
}

