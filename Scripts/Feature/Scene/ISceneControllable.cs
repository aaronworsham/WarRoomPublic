using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void OnReadyForPlayerInstance();
public interface ISceneControllable 
{
    event OnReadyForPlayerInstance OnReadyForPlayerInstance;

    void UpdatePlayerDataFromUI(string key, string value);

    

}
