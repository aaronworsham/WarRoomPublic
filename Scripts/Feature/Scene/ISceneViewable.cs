using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sazboom.WarRoom
{
    public interface ISceneViewable 
    {
        void NameFromModel(string name);
        void TokenStringFromModel(string tokenString);
        void ColorStringFromModel(string colorString);

        void RegisterPlayerView(IPlayerViewable iplayerView);
    }
}


