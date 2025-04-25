using UnityEngine;

//Purpose : Just to Do Check Interaction with 3D Objects via raycast
public class Player : MonoBehaviour
{
    //Managed
    public Crosshair crosshair;

    //Script Access
    [HideInInspector]
    public bool canPlayerInteract = false;

    RaycastHit raycasthit;
    LayerMask raycastLayerMask = 1 << 8; // 8 - interactable layer

    void Update()
    {
        crosshair.SetCrosshairType(Crosshair.CrosshairType.Default);

        //First Check if player is allow to interact
        if (canPlayerInteract)
        {
            //Lastly cast Ray from middle of screen
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (Physics.Raycast(ray, out raycasthit, raycastLayerMask))
            {
                //Check if the object we hit is a console button (should 100% be, as it is the only interactable object)
                ConsoleButton button = raycasthit.transform.GetComponent<ConsoleButton>();

                if (button != null)
                {
                    crosshair.SetCrosshairType(Crosshair.CrosshairType.Hand);

                    if(Input.GetMouseButton(0))
                    {
                        crosshair.SetCrosshairType(Crosshair.CrosshairType.Grab);
                    }

                    //Lastly Check if Player has left Mouse Clicked
                    if (Input.GetMouseButtonDown(0))
                    {
                        button.ActivateButton();
                    }
                }
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.tag == "Interactable")
        {
            canPlayerInteract = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Interactable")
        {
            canPlayerInteract = true;
        }
    }
}
