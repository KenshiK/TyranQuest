using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverToChangeColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text theText;
    public Image image;

    public void Start(){
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color32(0x0, 0x0, 0x0, 0x0);
        theText.color = Color.white; //Or however you do your color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = new Color32(0x0, 0x0, 0x0, 0x0);
        Color myColor = new Color32(0x21, 0x21, 0x21, 0xFF);
        theText.color = myColor; //Or however you do your color
    }
}