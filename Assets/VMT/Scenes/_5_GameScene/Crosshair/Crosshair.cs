using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    //Managed
    public Image CrosshairUI;

    //Assigned
    public Sprite DefaultIcon;
    public Sprite InteractableHandIcon;
    public Sprite InteractingHandGrabIcon;

    public enum CrosshairType { Default,Hand,Grab}

    public void SetCrosshairType(CrosshairType type)
    {
        switch (type)
        {
            case CrosshairType.Hand:
                CrosshairUI.sprite = InteractableHandIcon;
                break;
            case CrosshairType.Grab:
                CrosshairUI.sprite = InteractingHandGrabIcon;
                break;
            default:
                CrosshairUI.sprite = DefaultIcon;
                break;
        }
    }
}
