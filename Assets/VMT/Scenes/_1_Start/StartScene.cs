using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Manages Unity StartScene
/// </summary>
public class StartScene : MonoBehaviour
{
    //Managed
    public Button StartBtn;

    void Start()
    {
        StartBtn.onClick.AddListener(OnStartBtnClick);
    }

    //Event
    public void OnStartBtnClick()
    {
        SceneManager.LoadScene("LoginScene");
    }
}
