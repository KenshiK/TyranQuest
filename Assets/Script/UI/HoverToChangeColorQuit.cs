using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverToChangeColorQuit : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text theText;

    public void OnPointerEnter(PointerEventData eventData)
    {

       theText.color = Color.white; //Or however you do your color

        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color mycolor = new Color32(0xFF, 0xFF, 0xFF, 0xB4);
        theText.color = mycolor; //Or however you do your color
    }
}