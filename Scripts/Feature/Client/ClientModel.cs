using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientModel : MonoBehaviour
{
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
    [SerializeField] private string _selectedTokenString;


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
    [SerializeField] private string _selectedColorString;

    [Header("Name")]

    [SerializeField] private string _nameEntered;
    #endregion

    #region Getters/Setters

    //Token Getter/Setters

    public GameObject SelectedToken
    {
        get
        {
            string prefToken = PlayerPrefs.GetString("playerToken");
            
            //Token exists in memory?
            if (_selectedToken != null)
            {
                return _selectedToken;
            }

            //Token string exists in memory for lookup?
            else if(_selectedTokenString != null)
            {
                if (tokenLookup.ContainsKey(_selectedTokenString))
                {
                    GameObject token;
                    tokenLookup.TryGetValue(_selectedTokenString, out token);
                    Debug.Log(token.name);
                    _selectedToken = token;
                    PlayerPrefs.SetString("playerToken", _selectedTokenString);
                    return token;
                }
                //String Doesn't match a token in lookup
                else
                {
                    _selectedToken = aalan;
                    _selectedTokenString = "aalan";
                    PlayerPrefs.SetString("playerToken", "aalan");
                    Debug.LogError("Token String does not exist in lookup");
                    return aalan;
                }

            }
            //Token String exists on disk?
            else if (prefToken != null)
            {
                if (tokenLookup.ContainsKey(prefToken))
                {
                    GameObject token = tokenLookup[prefToken];
                    _selectedToken = token;
                    _selectedTokenString = prefToken;
                    return token;
                }
                //String Doesn't match a token in lookup
                else
                {
                    _selectedToken = aalan;
                    _selectedTokenString = "aalan";
                    PlayerPrefs.SetString("playerToken", "aalan");
                    Debug.LogError("Token String does not exist in lookup");
                    return aalan;
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
            string prefToken = PlayerPrefs.GetString("playerToken");

            //Token String exists in memory?
            if (_selectedTokenString != null)
            {
                return _selectedTokenString;
            }

            //Token String exists on disk?
            else if (prefToken != null)
            {
                if (tokenLookup.ContainsKey(prefToken))
                {
                    _selectedTokenString = prefToken;
                    return prefToken;
                }
                //String Doesn't match a token in lookup
                else
                {
                    _selectedTokenString = "aalan";
                    PlayerPrefs.SetString("playerToken", "aalan");
                    Debug.LogError("Token String does not exist in lookup");
                    return "aalan";
                }

            }
            //Token String is unknowable, set a default
            else
            {
                return null;
            }
        }

        set
        {
            string tokenString = value;
            if (tokenLookup.ContainsKey(tokenString))
            {
                _selectedTokenString = tokenString;
                PlayerPrefs.SetString("playerToken", tokenString);
            }
            else
            {
                _selectedTokenString = "aalan";
                Debug.LogError("Token String does not exist in lookup");

            }
        }
    }

    //Color Getters/Setters

    public Material SelectedDefaultMaterial
    {
        get
        {
            string prefColor = PlayerPrefs.GetString("playerColor");

            //Material exists in memory?
            if (_selectedDefaultMaterial != null)
            {
                return _selectedDefaultMaterial;
            }

            //Material string exists in memory for lookup?
            else if (_selectedColorString != null)
            {
                if (colorLookup.ContainsKey(_selectedColorString))
                {
                    Material mat = colorLookup[_selectedColorString];
                    _selectedDefaultMaterial = mat;
                    PlayerPrefs.SetString("playerColor", _selectedColorString);
                    return mat;
                }
                //String Doesn't match a Material in lookup
                else
                {
                    _selectedDefaultMaterial = blackMat;
                    _selectedColorString = "black";
                    PlayerPrefs.SetString("playerColor", "black");
                    Debug.LogError("Color String does not exist in lookup");
                    return blackMat;
                }

            }
            //Token String exists on disk?
            else if (prefColor != null)
            {
                if (colorLookup.ContainsKey(prefColor))
                {
                    Material mat = colorLookup[prefColor];
                    _selectedDefaultMaterial = mat;
                    _selectedColorString = prefColor;
                    return mat;
                }
                //String Doesn't match a color in lookup
                else
                {
                    _selectedDefaultMaterial = blackMat;
                    _selectedColorString = "black";
                    PlayerPrefs.SetString("playerColor", "black");
                    Debug.LogError("Color String does not exist in lookup");
                    return blackMat;
                }

            }
            //Color String is unknowable, set a default
            else
            {
                return null;
            }
        }
    }

    public string SelectedColorString {
        get
        {
            string prefColor = PlayerPrefs.GetString("playerColor");

            //Material exists in memory?
            if (_selectedColorString != null)
            {
                return _selectedColorString;
            }

            //Token String exists on disk?
            else if (prefColor != null)
            {
                if (colorLookup.ContainsKey(prefColor))
                {
                    _selectedColorString = prefColor;
                    return prefColor;
                }
                //String Doesn't match a color in lookup
                else
                {
                    _selectedColorString = "black";
                    PlayerPrefs.SetString("playerColor", "black");
                    Debug.LogError("Color String does not exist in lookup");
                    return "black";
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
                PlayerPrefs.SetString("playerColor", colorString);
            }
            else
            {
                _selectedColorString = "black";
                Debug.LogError("Color String does not exist in lookup");

            }
        }
    }

    public string NameEntered
    {
        get
        {
            string prefName = PlayerPrefs.GetString("playerName");

            //Material exists in memory?
            if (_nameEntered != null)
            {
                return _nameEntered;
            }

            //Token String exists on disk?
            else if (prefName != null)
            {

                _nameEntered = prefName;
                return prefName;


            }
            //Color String is unknowable, set a default
            else
            {
                return null;
            }
        }

        set
        {
            _nameEntered = value;
        }
    }

    #endregion

    public static Action<string> OnTokenStringChange;
    public static Action<string> OnColorStringChange;
    public static Action<string> OnNameChange;
    public static Action<string> OnReadyForLoadPanel;

    Dictionary<string, GameObject> tokenLookup = new Dictionary<string, GameObject>();
    Dictionary<string, Material> colorLookup = new Dictionary<string, Material>();


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


        LoadUI.OnCharacterSelection += HandleCharacterSelection;
        LoadUI.OnColorSelection += HandleColorSelection;
        LoadUI.OnNameEntered += HandleNameEntered;

    }

    private void Start()
    {
        CheckForTokenFromDisk();
        CheckForColorFromDisk();
        CheckForNameFromDisk();
    }



    void CheckForTokenFromDisk()
    {
        string tokenString = PlayerPrefs.GetString("playerToken");
        if (tokenString != null && tokenLookup.ContainsKey(tokenString))
        {
            SetTokenByString(tokenString);
            OnTokenStringChange?.Invoke(tokenString);
        }


    }

    void CheckForColorFromDisk()
    {
        string colorString = PlayerPrefs.GetString("playerColor");
        if (colorString != null && colorLookup.ContainsKey(colorString))
        {
            SetColorByString(colorString);
            OnColorStringChange?.Invoke(colorString);
        }

    }

    void CheckForNameFromDisk()
    {
        string nameEntered = PlayerPrefs.GetString("playerName");
        if (nameEntered != null)
        {
            SetNameByString(nameEntered);
            OnNameChange?.Invoke(nameEntered);
        }

    }

    void HandleCharacterSelection(string name)
    {
        if (tokenLookup.ContainsKey(name))
        {
            _selectedTokenString = name;
            _selectedToken = tokenLookup[name];
            PlayerPrefs.SetString("playerToken", name);
            PlayerPrefs.Save();
        }
        else
        {
            _selectedTokenString = "black";
            _selectedToken = tokenLookup["aalan"];
            Debug.LogError(name + " not found in token lookup");
        }
            

    }

    public void HandleColorSelection(string color)
    {
        if (colorLookup.ContainsKey(color))
        {
            _selectedColorString = color;
            _selectedDefaultMaterial = colorLookup[color];
            PlayerPrefs.SetString("playerColor", color);
            PlayerPrefs.Save();

        }
        else
        {
            _selectedColorString = "red";
            _selectedDefaultMaterial = colorLookup["red"];
            Debug.LogError(color + " not found in token lookup");
        }

    }

    public void HandleNameEntered(string name)
    {
        _nameEntered = name;
        PlayerPrefs.SetString("playerName", name);
        PlayerPrefs.Save();
    }

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
        if(str != null && tokenLookup.ContainsKey(str)){
            _selectedTokenString = str;
            _selectedToken = tokenLookup[str];
        }
    }

    void SetColorByString(string str)
    {
        if (str != null && colorLookup.ContainsKey(str))
        {
            _selectedColorString = str;
            _selectedDefaultMaterial = colorLookup[str];
        }
    }

    void SetNameByString(string str)
    {
        _nameEntered = str;
    }
}
