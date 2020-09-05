using Sazboom.WarRoom;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace Sazboom.WarRoom
{
    public class SceneModel : MonoBehaviour, ISceneModelable
    {

        [SerializeField] private ISceneViewable isceneView;
        [SerializeField] private ISceneControllable isceneController;
        [SerializeField] private IPlayerModelable iplayerModel;


        [SerializeField] private bool playerReady = false;
        [SerializeField] private string tokenString;
        [SerializeField] private string colorString;
        [SerializeField] private string nameEntered;



        //#region Events

        public event TokenStringChangeAction OnTokenStringChangeEvent;
        event TokenStringChangeAction ISceneModelable.OnTokenStringChange
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
        event ColorStringChangeAction ISceneModelable.OnColorStringChange
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
        event NameChangeAction ISceneModelable.OnNameChange
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

        public void Start()
        {
            isceneView = GetComponent<ISceneViewable>();
            isceneController = GetComponent<ISceneControllable>();
        }


        public void RegisterPlayerModel(IPlayerModelable playerModel)
        {
            playerReady = true;
            iplayerModel = playerModel;
            playerModel.OnColorStringChange += PlayerModel_OnColorStringChange;
            playerModel.OnNameChange += PlayerModel_OnNameChange;
            playerModel.OnTokenStringChange += PlayerModel_OnTokenStringChange;

            isceneController.ModelReady();

        }

        private void PlayerModel_OnTokenStringChange(string token)
        {
            Debug.Log("SceneModel|TokenStringChange " + token);

        }

        private void PlayerModel_OnNameChange(string name)
        {
            Debug.Log("SceneModel|NameChange " + name);
        }

        private void PlayerModel_OnColorStringChange(string color)
        {
            Debug.Log("SceneModel|ColorStringChange " + color);
        }

        public void NameFromSceneView(string name)
        {
            nameEntered = name;
            OnNameChangeEvent?.Invoke(name);
        }
        public void ColorFromSceneView(string color)
        {
            colorString = color;
            OnColorStringChangeEvent?.Invoke(color);
        }
        public void TokenFromSceneView(string token)
        {
            tokenString = token;
            OnTokenStringChangeEvent?.Invoke(token);
        }




    }
}


