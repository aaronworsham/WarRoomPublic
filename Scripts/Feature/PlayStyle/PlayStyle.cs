using UnityEngine;

public class PlayStyle : MonoBehaviour
{

    public enum PlayStyles
    {
        TBS,
        RTS,
        FPS
    };

    [SerializeField]
    private PlayStyles currentPlayStyle = PlayStyles.TBS; 

    public PlayStyles GetCurrentStyle()
    {
        return currentPlayStyle;
    }
    
    public bool SetToTBS()
    {
        currentPlayStyle = PlayStyles.TBS;
        return true;
    }

    public bool SetToFPS()
    {
        currentPlayStyle = PlayStyles.FPS;
        return true;
    }

    public bool SetToRTS()
    {
        currentPlayStyle = PlayStyles.RTS;
        return true;
    }

    public bool IsTBS()
    {
        return currentPlayStyle == PlayStyles.TBS;
    }

    public bool IsRTS()
    {
        return currentPlayStyle == PlayStyles.RTS;
    }

    public bool IsFPS()
    {
        return currentPlayStyle == PlayStyles.FPS;
    }

}
