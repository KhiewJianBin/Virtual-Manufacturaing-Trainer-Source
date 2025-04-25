using UnityEngine;

public class ConsoleButton : MonoBehaviour
{
    //Managed
    public Animator buttonAnimator;
    public AudioSource audioSource;
    public AudioClip buttonPressSnd;

    public delegate void Activate();
    public Activate activateButtonEvent;

    //Function to trigger by player when button is being clicked on
    public void ActivateButton()
    {
        //Play Sound
        audioSource.PlayOneShot(buttonPressSnd);

        //Play Animation
        buttonAnimator.SetTrigger("PUSH");

        //Tell the console that button has been clicked
        activateButtonEvent();
    }
}
