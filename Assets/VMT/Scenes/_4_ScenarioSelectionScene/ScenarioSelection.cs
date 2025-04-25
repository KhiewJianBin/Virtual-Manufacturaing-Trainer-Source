using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenarioSelection : MonoBehaviour
{
    //Managed
    public Animator ModePanelAnimator;
    public Button Scenario1Button;
    public Button Scenario2Button;
    public Button Scenario3Button;
    public Button BackButton;
    public Button LogoutButton;

    void Start()
    {
        Scenario1Button.onClick.AddListener(OnScenario1Click);
        Scenario2Button.onClick.AddListener(OnScenario2Click);
        Scenario3Button.onClick.AddListener(OnScenario3Click);
        BackButton.onClick.AddListener(OnBackButtonClicked);
        LogoutButton.onClick.AddListener(OnLogoutButtonClicked);

        //Opening UI Animation
        if(Session.Instance.sessionTrainingMode == true)
        {
            ModePanelAnimator.Play("FromTraining");
        }
        else
        {
            ModePanelAnimator.Play("FromAssessment");
        }
    }

    void OnScenario1Click()
    {
        Session.Instance.sessionScenarioNum = 1;
        AsyncSceneLoader.NextSceneToLoad = "GameScene";
        SceneManager.LoadScene("LoadingScene");
    }
    void OnScenario2Click()
    {
        Session.Instance.sessionScenarioNum = 2;
        AsyncSceneLoader.NextSceneToLoad = "GameScene";
        SceneManager.LoadScene("LoadingScene");
    }
    void OnScenario3Click()
    {
        Session.Instance.sessionScenarioNum = 3;
        AsyncSceneLoader.NextSceneToLoad = "GameScene";
        SceneManager.LoadScene("LoadingScene");
    }
    void OnBackButtonClicked()
    {
        SceneManager.LoadScene("ModeSelectionScene");
    }
    void OnLogoutButtonClicked()
    {
        Session.Instance.Logout();
        SceneManager.LoadScene("LoginScene");
    }
}
