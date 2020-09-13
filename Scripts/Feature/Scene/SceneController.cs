using UnityEngine;
using System;

namespace Sazboom.WarRoom
{
    public abstract class SceneController : MonoBehaviour, ISceneControllable
    {
        public abstract void ViewReady(bool isGM);
        public abstract void ModelReady();

        public abstract void RegisterPlayerController(IPlayerControllable playerController);
        public abstract void ExitScene();

    }
}


