using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModeHelper : MonoBehaviour
{
    public bool debug = false;
    public GameMode gameMode;
    public GameMode.GameModes currentGameMode = GameMode.GameModes.MAIN;

    private void Awake()
    {
        gameMode = GameObject.Find("Game").GetComponent<GameMode>();
        //gameMode.OnGameModeChange += HandleGameModeEventTemp;
        gameMode.OnGameModeChangeParam += HandleGameModeEvent;
    }

    private void HandleGameModeEvent(GameMode.GameModes mode)
    {
        if (debug) Debug.Log("GMH1");
        if (debug) Debug.Log(mode);

        currentGameMode = mode;
    }
     
    private void HandleGameModeEventTemp()
    {
        if (debug) Debug.Log("GMH2");

    }

    public bool IsMove()
    {
        //if (debug) Debug.Log("GMH2");
        return currentGameMode == GameMode.GameModes.MOVE;
    }

    public bool IsMain()
    {
        //if (debug) Debug.Log("GMH3");
        return currentGameMode == GameMode.GameModes.MAIN;
    }

}
