using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Sazboom.WarRoom
{
    public abstract class SceneView : MonoBehaviour, ISceneViewable
    {

        public abstract void RegisterPlayerView(IPlayerViewable playerView);
        public abstract void NameFromModel(string name);
        public abstract void TokenStringFromModel(string tokenString);
        public abstract void ColorStringFromModel(string colorString);

    }
}


