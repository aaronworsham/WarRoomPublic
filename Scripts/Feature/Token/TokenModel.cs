using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



namespace Sazboom
{
    namespace WarRoom
    {

        [RequireComponent(typeof(NetworkLogger))]
        public class TokenModel : NetworkBehaviour
        {

            readonly bool debug = false;
            [SerializeField] private NetworkLogger logger;

            #region Properties
            //Materials
            [SerializeField] private Material redMat;
            [SerializeField] private Material blackMat;
            [SerializeField] private Material blueMat;
            [SerializeField] private Material greenMat;
            [SerializeField] private Material yellowMat;
            [SerializeField] private Material orangeMat;
            [SerializeField] private Material purpleMat;
            [SerializeField] private Material cyanMat;
            Dictionary<string, Material> materialNames = new Dictionary<string, Material>();
            [SerializeField] private string currentTokenColor;

            #endregion

            #region Callbacks
            void OnValidate()
            {
                if (logger == null)
                    logger = GetComponent<NetworkLogger>();
            }

            void Awake()
            {

                materialNames.Add("red", redMat);
                materialNames.Add("blue", blueMat);
                materialNames.Add("green", greenMat);
                materialNames.Add("yellow", yellowMat);
                materialNames.Add("orange", orangeMat);
                materialNames.Add("purple", purpleMat);
                materialNames.Add("cyan", cyanMat);

            }
            #endregion

            #region Instance Methods
            public void ChangeBaseColor(string newColor)
            {
                Material newMat;
                if (materialNames.ContainsKey(newColor))
                {
                    newMat = materialNames[newColor];
                    gameObject.transform.Find("Base").GetComponent<MeshRenderer>().material = materialNames[newColor];
                    if (debug) logger.TLog(this.GetType().Name, "ChangeColor|" + newColor);
                    currentTokenColor = newColor;
                }
                else
                    if (debug) logger.WLog(this.GetType().Name, "ChangeBaseColor|Cannot find " + newColor);

            }


            public void MovePlayerOverToken(GameObject player)
            {
                player.transform.position = gameObject.transform.position;
                gameObject.transform.rotation = player.transform.rotation;
                gameObject.transform.SetParent(player.transform);
                //currentlyInFocus = true;
            }
            #endregion
        }
    }
}

