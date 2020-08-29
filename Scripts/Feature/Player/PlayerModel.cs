using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;



namespace Sazboom
{
    namespace WarRoom
    {

        [RequireComponent(typeof(NetworkLogger))]
        public class PlayerModel : NetworkBehaviour
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
            [SerializeField] private string currentPlayerName;
            [SerializeField] private string currentPlayerColor;

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
            public void ChangeColor(string newColor)
            {
                Material newMat;
                if (materialNames.ContainsKey(newColor))
                {
                    newMat = materialNames[newColor];
                    if (debug) logger.TLog(this.GetType().Name, "ChangeColor|" + newColor);
                    GameObject orb = gameObject.transform.Find("SoulOrb").gameObject;
                    GameObject playerBase = gameObject.transform.Find("Base").gameObject;
                    orb.GetComponent<MeshRenderer>().material = newMat;
                    playerBase.GetComponent<MeshRenderer>().material = newMat;
                    currentPlayerColor = newColor;
                }
                else
                    if (debug) logger.WLog(this.GetType().Name, "ChangeColor|Cannot find " + newColor);

            }

            public void ChangeName(string newName)
            {
                gameObject.name = newName;
                gameObject.transform.Find("PlayerName").GetComponent<TextMesh>().text = newName;
                currentPlayerName = newName;
            }

            public void HideSoul()
            {
                gameObject.transform.Find("SoulOrb").gameObject.SetActive(false);
                gameObject.transform.Find("Base").gameObject.SetActive(false);
                gameObject.transform.Find("PlayerName").gameObject.SetActive(false);
            }
            #endregion
        }
    }
}

