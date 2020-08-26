using System;
using System.Collections;
using TMPro;
using UnityEngine;


namespace Sazboom.WarRoom
{
    public class UiBottomRight : MonoBehaviour
    {

        [SerializeField] private GameObject rightCorner;
        [SerializeField] private GameObject rightCornerText;
        [SerializeField] private TextMeshProUGUI rightCornerTMP;
        
        
        public static Action OnShow;
        public static Action<String> OnMessage;
        public static Action OnHide;

        private void Awake()
        {
            rightCorner = GameObject.Find("/MainCanvas/RightCorner");
            rightCornerText = GameObject.Find("/MainCanvas/RightCorner/RightCornerText");
            rightCornerTMP = rightCornerText.GetComponent<TextMeshProUGUI>();

            rightCorner.SetActive(false);

            OnShow += HandleShow;
            OnMessage += HandleMessage;
            OnHide += HandleHide;
        }

        public void HandleShow()
        {
            rightCorner.SetActive(true);
        }


        public void HandleMessage(String str)
        {
            rightCornerTMP.text = str;
        }

        public void HandleHide()
        {
            rightCornerTMP.text = String.Empty;
            rightCorner.SetActive(false);
        }
    }
}

