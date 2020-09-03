using UnityEngine;
using System;

namespace Sazboom.WarRoom
{
    public class SceneController : MonoBehaviour
    {
        //[SerializeField] private WRNetworkManager network;
        //[SerializeField] private LoadUI loadUI;

        //public Action OnReadyForPlayerInstance;
        //public Action<string, string> OnPlayerDataUpdateFromUI;

        void OnValidate()
        {
            //if (network == null)
            //    network = GameObject.Find("Network").GetComponent<WRNetworkManager>();
            //if (loadUI == null)
            //    loadUI = GameObject.Find("MainCanvas").GetComponent<LoadUI>();
        }

        private void Awake()
        {
            //PlayerController.OnLocalPlayerReady += HandleLocalPlayerReady;
            //LoadUI.OnLoadPanelDone += HandleLoadPanelDone;
        }

        private void OnDestroy()
        {
            //PlayerController.OnLocalPlayerReady -= HandleLocalPlayerReady;
            //LoadUI.OnLoadPanelDone -= HandleLoadPanelDone;
        }

        void HandleLocalPlayerReady()
        {
           // loadUI.ShowLoadUI();
        }

        void HandleLoadPanelDone()
        {
           // OnReadyForPlayerInstance?.Invoke();
        }

        //public void UpdatePlayerDataFromUI(string key, string value)
        //{

        //}


        //private void OnTriggerEnter(Collider other)
        //{
        //    Debug.Log("Trigger Entered");
        //    network.ServerChangeScene("DangerRoom");

        //}

        //private void OnCollisionEnter(Collision collision)
        //{
        //    Debug.Log("Collision Entered");
        //}
    }
}


