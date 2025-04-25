using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginScene : MonoBehaviour
{
    //Managed
    public InputField UsernameInputfield;
    public InputField PasswordInputfield;
    public Button LoginBtn;

    public GameObject LoginInfo;
    public Text LoginInfoText;
    public GameObject LoadingCircle;

    //Using this as a callback flag
    int AuthenticateResult = -1;


    //Animate
    bool IssimulateAuthentication = false;
    float simulateAuthenticationTimer = 0;
    public float simulateAuthenticationDuration = 1.25f;

    void Start()
    {
        LoginBtn.onClick.AddListener(OnLoginBtnClick);

        //should already be disabled, but just to double make sure
        LoginInfo.SetActive(false);
        LoadingCircle.SetActive(false);
    }

    //Event
    public void OnLoginBtnClick()
    {
        string usernameInput = UsernameInputfield.text;
        string passwordInput = PasswordInputfield.text;

        //First we do some basic input validation check

        //If No Username And Password
        if(usernameInput == string.Empty && passwordInput == string.Empty)
        {
            ShowNoUsernamePasswordAlert();
        }
        //If No Username Only
        else if (usernameInput == string.Empty)
        {
            ShowNoUsernameAlert();
        }
        //If No Password Only
        else if (passwordInput == string.Empty)
        {
            ShowNoPasswordAlert();
        }
        //Proceed to Authenticate User
        else
        {
            StartSimulateAuthentication();

            Session.Instance.AuthenticateUser(UsernameInputfield.text, PasswordInputfield.text, AuthenticateCallback);
        }
    }

    void Update()
    {
        if(IssimulateAuthentication)
        {
            simulateAuthenticationTimer += Time.deltaTime;
            if(simulateAuthenticationTimer >= simulateAuthenticationDuration)
            {
                HideLoadingCircle();

                IssimulateAuthentication = false;

                //Check Result after (simulated) db lookup has finish
                if (AuthenticateResult == 0)
                {
                    ShowWrongDetailsAlert();
                }
                else
                {
                    SceneManager.LoadScene("ModeSelectionScene");
                }
            }
        }
    }

    //Helpers
    void StartSimulateAuthentication()
    {
        IssimulateAuthentication = true;
        simulateAuthenticationTimer = 0;
        HideLoginInfo();
        ShowLoadingCircle();
    }

    void ShowLoadingCircle()
    {
        LoadingCircle.SetActive(true);
    }
    void HideLoadingCircle()
    {
        LoadingCircle.SetActive(false);
    }
    void ShowNoUsernamePasswordAlert()
    {
        LoginInfo.gameObject.SetActive(true);
        LoginInfoText.text = "Please Enter Your Username And Password!";
    }
    void ShowNoUsernameAlert()
    {
        LoginInfo.gameObject.SetActive(true);
        LoginInfoText.text = "Please Enter Your Username!";
    }
    void ShowNoPasswordAlert()
    {
        LoginInfo.gameObject.SetActive(true);
        LoginInfoText.text = "Please Enter Your Password!";
    }
    void ShowWrongDetailsAlert()
    {
        LoginInfo.gameObject.SetActive(true);
        LoginInfoText.text = "No Username Or Wrong Password. Try Again!";
    }
    void HideLoginInfo()
    {
        LoginInfo.gameObject.SetActive(false);
    }

    void AuthenticateCallback(bool authResult)
    {
        if(authResult == true)
        {
            AuthenticateResult = 1;
        }
        else
        {
            AuthenticateResult = 0;
        }
    }
}
