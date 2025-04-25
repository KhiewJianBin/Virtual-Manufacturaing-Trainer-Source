using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeSelection : MonoBehaviour
{
    //Managed
    public Button TrainingModeButton;
    public Button AssessmentModeButton;
    public Button LogoutButton;

    void Start()
    {
        TrainingModeButton.onClick.AddListener(OnTrainingModeClicked);
        AssessmentModeButton.onClick.AddListener(OnAssessmentModeClicked);
        LogoutButton.onClick.AddListener(OnLogoutButtonClicked);
    }

    void OnTrainingModeClicked()
    {
        Session.Instance.sessionTrainingMode = true;
        SceneManager.LoadScene("ScenarioSelectionScene");
    }
    void OnAssessmentModeClicked()
    {
        Session.Instance.sessionTrainingMode = false;
        SceneManager.LoadScene("ScenarioSelectionScene");
    }
    void OnLogoutButtonClicked()
    {
        Session.Instance.Logout();
        SceneManager.LoadScene("LoginScene");
    }
}
