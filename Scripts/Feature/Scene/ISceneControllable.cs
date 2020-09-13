using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sazboom.WarRoom
{
    public delegate void OnReadyForPlayerInstance();
    public interface ISceneControllable 
    {
        void ViewReady(bool isGM);
        void ModelReady();

        void RegisterPlayerController(IPlayerControllable playerController);
        void ExitScene();
    

    }

}

