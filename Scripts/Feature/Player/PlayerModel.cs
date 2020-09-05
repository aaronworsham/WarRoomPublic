using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Sazboom.WarRoom
{
    public class PlayerModel : NetworkBehaviour, IPlayerModelable
    {

        [SerializeField] private PlayerData playerData;

        #region Properties
        [Header("Character Tokens")]
        [SerializeField] private GameObject aalan;
        [SerializeField] private GameObject fayzyre;
        [SerializeField] private GameObject galan;
        [SerializeField] private GameObject kreck;
        [SerializeField] private GameObject liam;
        [SerializeField] private GameObject tbom;
        [SerializeField] private GameObject zizkek;

        [SerializeField] private GameObject _selectedToken;
        [SyncVar] private string _selectedTokenString;


        [Header("Colors")]
        [SerializeField] private Material redMat;
        [SerializeField] private Material blackMat;
        [SerializeField] private Material blueMat;
        [SerializeField] private Material greenMat;
        [SerializeField] private Material yellowMat;
        [SerializeField] private Material orangeMat;
        [SerializeField] private Material purpleMat;
        [SerializeField] private Material cyanMat;

        [SerializeField] private Material _selectedDefaultMaterial;
        [SyncVar] private string _selectedColorString;

        [Header("Name")]

        [SyncVar] private string _nameEntered;
        #endregion

        #region Events


        public event TokenStringChangeAction OnTokenStringChangeEvent;
        event TokenStringChangeAction IPlayerModelable.OnTokenStringChange
        {
            add
            {
                OnTokenStringChangeEvent += value;
            }
            remove
            {
                OnTokenStringChangeEvent -= value;
            }

        }

        public event ColorStringChangeAction OnColorStringChangeEvent;
        event ColorStringChangeAction IPlayerModelable.OnColorStringChange
        {
            add
            {
                OnColorStringChangeEvent += value;
            }
            remove
            {
                OnColorStringChangeEvent -= value;
            }

        }

        public event NameChangeAction OnNameChangeEvent;
        event NameChangeAction IPlayerModelable.OnNameChange
        {
            add
            {
                OnNameChangeEvent += value;
            }
            remove
            {
                OnNameChangeEvent -= value;
            }

        }

        #endregion

        #region Dictonaries
        Dictionary<string, GameObject> tokenLookup = new Dictionary<string, GameObject>();
        Dictionary<string, Material> colorLookup = new Dictionary<string, Material>();
        #endregion

        #region Getters/Setters

        //Token Getter/Setters

        public GameObject SelectedToken
        {
            get
            {
                string playerDataToken = playerData.tokenString;

                //Token exists in memory?
                if (_selectedToken != null)
                {
                    return _selectedToken;
                }

                //Token string exists in memory for lookup?
                else if (!String.IsNullOrEmpty(_selectedTokenString))
                {
                    if (tokenLookup.ContainsKey(_selectedTokenString))
                    {
                        GameObject token;
                        tokenLookup.TryGetValue(_selectedTokenString, out token);
                        _selectedToken = token;
                        return token;
                    }
                    //String Doesn't match a token in lookup
                    else
                    {
                        Debug.LogError("Token String does not exist in lookup");
                        return null;
                    }

                }
                //Token exist in SerializableObject
                else if (!String.IsNullOrEmpty(playerDataToken))
                {
                    if (tokenLookup.ContainsKey(playerDataToken))
                    {
                        GameObject token;
                        tokenLookup.TryGetValue(playerDataToken, out token);
                        _selectedTokenString = playerDataToken;
                        _selectedToken = token;
                        return token;
                    }
                    //String Doesn't match a token in lookup
                    else
                    {
                        Debug.LogError("Token String does not exist in lookup: " + playerDataToken);
                        return null;
                    }
                }
                //Token String is unknowable, set a default
                else
                {
                    return null;
                }
            }
        }

        public string SelectedTokenString
        {
            get
            {
                string playerDataToken = playerData.tokenString;

                //Token String exists in memory?
                if (!String.IsNullOrEmpty(_selectedTokenString))
                {
                    return _selectedTokenString;
                }

                //Token exist in SerializableObject
                else if (!String.IsNullOrEmpty(playerDataToken))
                {
                    if (tokenLookup.ContainsKey(playerDataToken))
                    {
                        _selectedTokenString = playerDataToken;
                        return playerDataToken;
                    }
                    //String Doesn't match a token in lookup
                    else
                    {
                        Debug.LogError("Token String does not exist in lookup: " + playerDataToken);
                        return null;
                    }
                }

                //Token String is not known
                else
                {
                    Debug.LogError("Token string should not be empty at this time");
                    return null;
                }
            }

            set
            {
                string tokenString = value;
                if (tokenLookup.ContainsKey(tokenString))
                {
                    _selectedTokenString = tokenString;
                    if (isLocalPlayer)
                    {
                        playerData.tokenString = tokenString;
                    }
                }
                else
                {
                    Debug.LogError("Token String does not exist in lookup");

                }
            }
        }

        //Color Getters/Setters

        public Material SelectedDefaultMaterial
        {
            get
            {
                string playerDataColor = playerData.colorString;

                //Material exists in memory?
                if (_selectedDefaultMaterial != null)
                {
                    return _selectedDefaultMaterial;
                }

                //Material string exists in memory for lookup?
                else if (!String.IsNullOrEmpty(_selectedColorString))
                {
                    if (colorLookup.ContainsKey(_selectedColorString))
                    {
                        Material mat;
                        colorLookup.TryGetValue(_selectedColorString, out mat);
                        _selectedDefaultMaterial = mat;
                        return mat;
                    }
                    //String Doesn't match a Material in lookup
                    else
                    {
                        Debug.LogError("Color String does not exist in lookup");
                        return null;
                    }

                }
                //Token String exists on disk?
                else if (!String.IsNullOrEmpty(_selectedColorString))
                {
                    if (colorLookup.ContainsKey(_selectedColorString))
                    {
                        Material mat;
                        colorLookup.TryGetValue(_selectedColorString, out mat);
                        _selectedDefaultMaterial = mat;
                        return mat;
                    }
                    //String Doesn't match a Material in lookup
                    else
                    {
                        Debug.LogError("Color String does not exist in lookup");
                        return null;
                    }

                }
                //Color String is unknowable, set a default
                else
                {
                    return null;
                }
            }
        }

        public string SelectedColorString
        {
            get
            {
                string playerDataColor = playerData.colorString;

                //Material exists in memory?
                if (!String.IsNullOrEmpty(_selectedColorString))
                {
                    return _selectedColorString;
                }

                else if (!String.IsNullOrEmpty(playerDataColor))
                {
                    if (colorLookup.ContainsKey(playerDataColor))
                    {
                        _selectedColorString = playerDataColor;
                        return playerDataColor;
                    }
                    //String Doesn't match a token in lookup
                    else
                    {
                        Debug.LogError("Color String does not exist in lookup: " + playerDataColor);
                        return null;
                    }
                }
                else
                    return null;
            }
            set
            {
                string colorString = value;
                if (colorLookup.ContainsKey(colorString))
                {
                    _selectedColorString = colorString;
                    if (isLocalPlayer)
                    {
                        playerData.colorString = colorString;
                    }
                }
                else
                {
                    _selectedColorString = "black";
                    Debug.LogError("Color String does not exist in lookup");

                }
            }
        }

        //Name Getter/Setter
        public string NameEntered
        {
            get
            {
                string playerDataName = playerData.nameEntered;


                //Material exists in memory?
                if (!String.IsNullOrEmpty(_nameEntered))
                {
                    return _nameEntered;
                }

                else if (!String.IsNullOrEmpty(playerDataName))
                {
                    _nameEntered = playerDataName;
                    return playerDataName;
                }
                //Color String is unknowable, set a default
                else
                {
                    return null;
                }
            }

            set
            {
                string name = value;
                _nameEntered = name;
                if (isLocalPlayer)
                {
                    playerData.nameEntered = name;
                }
            }
        }

        #endregion

        #region Callbacks


        public void Awake()
        {
            //Token Lookup Entries
            tokenLookup.Add("aalan", aalan);
            tokenLookup.Add("fayzyre", fayzyre);
            tokenLookup.Add("galan", galan);
            tokenLookup.Add("kreck", kreck);
            tokenLookup.Add("liam", liam);
            tokenLookup.Add("tbom", tbom);
            tokenLookup.Add("zizkek", zizkek);

            //Color Lookup Entries
            colorLookup.Add("red", redMat);
            colorLookup.Add("blue", blueMat);
            colorLookup.Add("green", greenMat);
            colorLookup.Add("orange", orangeMat);
            colorLookup.Add("yellow", yellowMat);
            colorLookup.Add("cyan", cyanMat);
            colorLookup.Add("purple", purpleMat);
            colorLookup.Add("black", blackMat);

        }

        private void Start()
        {
            Debug.Log("PlayerModel|Start");
            ISceneModelable sceneModel = GameObject.Find("Scene").GetComponent<ISceneModelable>();
            sceneModel.OnColorStringChange += SceneModel_OnColorStringChange;
            sceneModel.OnNameChange += SceneModel_OnNameChange;
            sceneModel.OnTokenStringChange += SceneModel_OnTokenStringChange;
            
            sceneModel.RegisterPlayerModel(this);

            OnTokenStringChangeEvent?.Invoke("test");
            OnColorStringChangeEvent?.Invoke("test");
            OnNameChangeEvent?.Invoke("test");

            CheckForTokenFromDisk();
            CheckForColorFromDisk();
            CheckForNameFromDisk();
        }

        private void SceneModel_OnTokenStringChange(string token)
        {
            Debug.Log("PlayerModel| TokenChange");
            if (!isLocalPlayer) return;
            if (tokenLookup.ContainsKey(token))
            {
                _selectedTokenString = token;
                _selectedToken = tokenLookup[token];
                playerData.tokenString = token;
            }
            else
            {
                Debug.LogError(token + " not found in token lookup");
            }
        }

        private void SceneModel_OnNameChange(string name)
        {
            Debug.Log("PlayerModel| NameChange");
            if (!isLocalPlayer) return;
            _nameEntered = name;
            playerData.nameEntered = name;


        }

        private void SceneModel_OnColorStringChange(string color)
        {
            Debug.Log("PlayerModel| ColorChange");
            if (!isLocalPlayer) return;
            if (colorLookup.ContainsKey(color))
            {
                _selectedColorString = color;
                _selectedDefaultMaterial = colorLookup[color];
                playerData.colorString = color;

            }
            else
            {
                Debug.LogError(color + " not found in token lookup");
            }
        }





        void CheckForTokenFromDisk()
        {
            if (!isLocalPlayer) return;
            string tokenString = playerData.tokenString;
            if (tokenString != null && tokenLookup.ContainsKey(tokenString))
            {
                SetTokenByString(tokenString);
                OnTokenStringChangeEvent?.Invoke(tokenString);
            }


        }

        void CheckForColorFromDisk()
        {
            if (!isLocalPlayer) return;
            string colorString = playerData.colorString;
            if (colorString != null && colorLookup.ContainsKey(colorString))
            {
                SetColorByString(colorString);
                OnColorStringChangeEvent?.Invoke(colorString);
            }

        }

        void CheckForNameFromDisk()
        {
            if (!isLocalPlayer) return;
            string nameEntered = playerData.nameEntered;
            if (nameEntered != null)
            {
                SetNameByString(nameEntered);
                OnNameChangeEvent?.Invoke(nameEntered);
            }

        }

        #endregion

        public Material LookupMaterialByColor(string color)
        {
            if (colorLookup.ContainsKey(color))
                return colorLookup[color];
            else
                return colorLookup["black"];
        }

        public GameObject LookupTokenByName(string name)
        {
            if (tokenLookup.ContainsKey(name))
                return tokenLookup[name];
            else
                return tokenLookup["aalan"];
        }

        void SetTokenByString(string str)
        {
            if (str != null && tokenLookup.ContainsKey(str))
            {
                if (isLocalPlayer)
                {
                    playerData.tokenString = str;
                }
                _selectedTokenString = str;
                _selectedToken = tokenLookup[str];
            }


        }

        void SetColorByString(string str)
        {
            if (str != null && colorLookup.ContainsKey(str))
            {
                if (isLocalPlayer)
                {
                    playerData.colorString = str;
                }

                _selectedColorString = str;
                _selectedDefaultMaterial = colorLookup[str];
            }
        }

        void SetNameByString(string str)
        {
            if (isLocalPlayer)
            {
                playerData.nameEntered = str;
            }
            _nameEntered = str;
        }
    }
}


