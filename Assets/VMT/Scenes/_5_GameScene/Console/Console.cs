using TMPro;
using UnityEngine;

public class Console : MonoBehaviour
{
    //Managed
    public ConsoleButton button1;
    public ConsoleButton button2;
    public ConsoleButton button3;
    public ConsoleButton button4;
    public ConsoleButton button5;
    public GameObject colorIndicator;
    public GameObject sizeIndicator;

    void Start()
    {
        //Assign what each button do when activated
        button1.activateButtonEvent += OnButton1Click;
        button2.activateButtonEvent += OnButton2Click;
        button3.activateButtonEvent += OnButton3Click;
        button4.activateButtonEvent += OnButton4Click;
        button5.activateButtonEvent += OnButton5Click;
    }

    public delegate void Button1Click();
    public delegate void Button2Click();
    public delegate void Button3Click();
    public delegate void Button4Click();
    public delegate void Button5Click();
    public Button1Click button1ClickEvent;
    public Button2Click button2ClickEvent;
    public Button3Click button3ClickEvent;
    public Button4Click button4ClickEvent;
    public Button5Click button5ClickEvent;
    
    //Console will futher delegate the click event to the game manager to decide on what to do (behaviour)
    public void OnButton1Click()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 1));
        button1ClickEvent();
    }
    public void OnButton2Click()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 2));
        button2ClickEvent();
    }
    public void OnButton3Click()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 3));
        button3ClickEvent();
    }
    public void OnButton4Click()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 4));
        button4ClickEvent();
    }
    public void OnButton5Click()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 5));
        button5ClickEvent();
    }

    public void ChangeColorIndicator(VMT_Object.Colors newcolor)
    {
        if (newcolor == VMT_Object.Colors.White)
        {
            colorIndicator.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        }
        else if (newcolor == VMT_Object.Colors.Red)
        {
            colorIndicator.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.red);
        }
        else if (newcolor == VMT_Object.Colors.Green)
        {
            colorIndicator.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.green);
        }
        else if (newcolor == VMT_Object.Colors.Blue)
        {
            colorIndicator.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.blue);
        }
    }
    public void ChangeSizeIndicator(int newsize)
    {
        sizeIndicator.GetComponent<TextMeshPro>().text = newsize.ToString();
    }

    //disable collider
    public void DisableAllButtons()
    {
        button1.GetComponent<BoxCollider>().enabled = false;
        button2.GetComponent<BoxCollider>().enabled = false;
        button3.GetComponent<BoxCollider>().enabled = false;
        button4.GetComponent<BoxCollider>().enabled = false;
        button5.GetComponent<BoxCollider>().enabled = false;
    }
    public void EnableButton(int num)
    {
        if(num == 1)
            button1.GetComponent<BoxCollider>().enabled = true;
        else if (num == 2)
            button2.GetComponent<BoxCollider>().enabled = true;
        else if (num == 3)
            button3.GetComponent<BoxCollider>().enabled = true;
        else if (num == 4)
            button4.GetComponent<BoxCollider>().enabled = true;
        else if (num == 5)
            button5.GetComponent<BoxCollider>().enabled = true;
    }
}
