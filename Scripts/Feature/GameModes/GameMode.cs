using System;
using UnityEngine;

public class GameMode : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            SetToMain();
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            SetToMove();
        }


    }

    public Action OnGameModeChange;
    public Action<GameModes> OnGameModeChangeParam;

    public enum GameModes
    {
        MAIN,
        MOVE,
        TARGET,
        ATTACK,
        CAST
    };

   [SerializeField]
    private GameModes _currentGameMode;

    //Public State

    public GameModes CurrentGameMode {
        get 
        {
            return _currentGameMode;
        } 
        set
        {
            _currentGameMode = value;
            //OnGameModeChange();
            OnGameModeChangeParam(_currentGameMode);
        } 
    }


    public GameModes GetCurrentMode()
    {
        return _currentGameMode;
    }

    //Public Setters

    public bool SetToMain()
    {
        _currentGameMode = GameModes.MAIN;
        TopInfobar.OnTopInfobarUpdate("Game Mode: MAIN");
       // OnGameModeChange();
        OnGameModeChangeParam(_currentGameMode);
        return true;
    }

    public bool SetToMove()
    {
        _currentGameMode = GameModes.MOVE;
        TopInfobar.OnTopInfobarUpdate("Game Mode: MOVE");
        //OnGameModeChange();
        OnGameModeChangeParam(_currentGameMode);
        return true;
    }

}
    
