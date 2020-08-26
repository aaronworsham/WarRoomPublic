using Sazboom.WarRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TootipButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TooltipPopup tooltipPopup;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipPopup.DisplayInfo();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipPopup.HideInfo();
    }
}
