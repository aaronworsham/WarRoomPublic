using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sazboom.WarRoom
{
    public interface IPlayerActionable
    {
        void TokenIsReady();

        void GMIsReady();

        PlayerActions.ClientModes ClientMode { get; set; }
    }

}

