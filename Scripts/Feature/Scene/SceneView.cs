using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Sazboom.WarRoom
{
    public class SceneView : MonoBehaviour
    {

        TextMeshProUGUI colorTxt;
        TextMeshProUGUI tokenTxt;
        TextMeshProUGUI nameTxt;
        GameObject loadPanel;
        SceneModel sceneModel; 

        private void OnEnable()
        {
            Debug.Log("MainUILoadScene| OnEnable");


            tokenTxt = transform.Find("LoadPanel/SelectionsPnl/TokenTxt").GetComponent<TextMeshProUGUI>();
            colorTxt = transform.Find("LoadPanel/SelectionsPnl/ColorTxt").GetComponent<TextMeshProUGUI>();
            nameTxt = transform.Find("LoadPanel/SelectionsPnl/NameTxt").GetComponent<TextMeshProUGUI>();
            loadPanel = gameObject.transform.Find("LoadPanel").gameObject;
        }


        private void OnDisable()
        {
            tokenTxt = null;
            colorTxt = null;
            nameTxt = null;
            loadPanel = null;
        }


        private void OnDestroy()
        {
            tokenTxt = null;
            colorTxt = null;
            nameTxt = null;
            loadPanel = null;
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
            //OnCharacterSelection?.Invoke(token);
        }

        public void SelectColor(string color)
        {
            Debug.Log("Color: " + color);
            colorTxt.text = color;
            //OnColorSelection?.Invoke(color);
        }

        public void EnterName()
        {
            string name = transform.Find("LoadPanel/EnterNameFld").GetComponent<TMP_InputField>().text;
            Debug.Log("Name: " + name);
            nameTxt.text = name;
            //OnNameEntered?.Invoke(name);
        }

        public void DoneWithLoadScreen()
        {
            HideLoadUI();
            //OnLoadPanelDone?.Invoke();

        }


    }
}


