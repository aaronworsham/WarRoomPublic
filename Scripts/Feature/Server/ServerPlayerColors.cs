using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text;
using Mirror;

namespace Sazboom
{
    namespace WarRoom
    {
        public class ServerPlayerColors : NetworkBehaviour 
        {

            //Debugger
            readonly bool debug = false;

            Dictionary<uint, string> assignedMaterial = new Dictionary<uint, string>();
            readonly List<string> colorNames = new List<string>
            {
                "red",
                "blue",
                "green",
                "yellow",
                "orange",
                "purple",
                "cyan"
            };
            
            #region Server Methods
            
            [Server]
            public string GetAssignedColor(uint id)
            {
                if (debug) Debug.Log("GetAssignedColor|ID: " + id);
                if (assignedMaterial.ContainsKey(id))
                {
                    return assignedMaterial[id];
                }
                else
                {
                    string color = PickColor(id);
                    LogPlayerWithColor(id, color);
                    return color;
                }
            }
            
            [Server]
            public void LogPlayerWithColor(uint id, string color)
            {
                if (debug) Debug.Log("LogPlayerWithColor|ID: "+id+" Color:"+color);
                if (!assignedMaterial.ContainsKey(id))
                {
                    if (debug) Debug.Log("LogPlayerWithColor|Logging Color to ID");
                    assignedMaterial.Add(id, color);
                }
            }

            [Server]
            public string PickColor(uint id)
            {
                if (debug) Debug.Log("PickColor");
                List<string> colors = UnassignedColors();
                return colors[0];
            }

            [Server]
            public List<string> UnassignedColors()
            {
                if (debug) Debug.Log("unassignedColors");
                List<string> colors = new List<string>();
                foreach (string c in colorNames)
                {
                    if (!assignedMaterial.ContainsValue(c))
                    {
                        colors.Add(c);

                    }
                }
                return colors;
            }

            #endregion



        }

    }
}

