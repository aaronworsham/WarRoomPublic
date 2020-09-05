using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sazboom.WarRoom
{

    //Some Delegates in IPlayerModelable


    public interface ISceneModelable
    {
        event TokenStringChangeAction OnTokenStringChange;
        event ColorStringChangeAction OnColorStringChange;
        event NameChangeAction OnNameChange;

        void RegisterPlayerModel(IPlayerModelable playerModel);

        void NameFromSceneView(string name);
        void ColorFromSceneView(string color);
        void TokenFromSceneView(string token);
        
    }
}


