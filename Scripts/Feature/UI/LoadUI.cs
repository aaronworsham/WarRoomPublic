using Sazboom.WarRoom;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class LoadUI : MonoBehaviour
{
    public static Action<string> OnCharacterSelection;
    public static Action<string> OnColorSelection;
    public static Action<string> OnNameEntered;
    public static Action OnPanelReadyToLoad;

    TextMeshProUGUI colorTxt;
    TextMeshProUGUI tokenTxt;
    TextMeshProUGUI nameTxt;
    GameObject loadPanel;


    private void Awake()
    {
        ClientModel.OnColorStringChange += HandleColorChange;
        ClientModel.OnTokenStringChange += HandleTokenChange;
        ClientModel.OnNameChange += HandleNameChange;
        PlayerController.OnLocalPlayerReady += HandleOnLocalPlayerReady;

        tokenTxt = transform.Find("LoadPanel/SelectionsPnl/TokenTxt").GetComponent<TextMeshProUGUI>();
        colorTxt = transform.Find("LoadPanel/SelectionsPnl/ColorTxt").GetComponent<TextMeshProUGUI>();
        nameTxt = transform.Find("LoadPanel/SelectionsPnl/NameTxt").GetComponent<TextMeshProUGUI>();
        loadPanel = gameObject.transform.Find("LoadPanel").gameObject;
    }



    void HandleColorChange(string color)
    {
        colorTxt.text = color;
    }

    void HandleTokenChange(string token)
    {
        tokenTxt.text = token;
    }

    void HandleNameChange(string name)
    {
        nameTxt.text = name;
    }
    
    void HandleOnLocalPlayerReady()
    {
        HideLoadUI();
    }

    public void HideLoadUI()
    {
        
        Debug.Log("Hiding Canvas");
        loadPanel.SetActive(false);
    }

    public void ShowLoadUI()
    {
        Debug.Log("Showing Canvas");
        loadPanel.SetActive(true);
    }

    public void SelectToken(string token)
    {
        Debug.Log("Token: " + token);
        tokenTxt.text = token;
        OnCharacterSelection?.Invoke(token);
    }

    public void SelectColor(string color) 
    {
        Debug.Log("Color: " + color);
        colorTxt.text = color;
        OnColorSelection?.Invoke(color);
    }

    public void EnterName()
    {
        string name = transform.Find("LoadPanel/EnterNameFld").GetComponent<TMP_InputField>().text;
        Debug.Log("Name: " + name);
        nameTxt.text = name;
        OnNameEntered?.Invoke(name);
    }



}
