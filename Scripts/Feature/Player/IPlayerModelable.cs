using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace Sazboom.WarRoom
{
    public delegate void TokenStringChangeAction(string Token);
    public delegate void ColorStringChangeAction(string color);
    public delegate void NameChangeAction(string name);



    public interface IPlayerModelable 
    {
        event TokenStringChangeAction OnTokenStringChange;
        event ColorStringChangeAction OnColorStringChange;
        event NameChangeAction OnNameChange;

    }
}

