using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    //Managed
    public Button ReturnBtn;
    public Button QuitBtn;
    public Text GameoverText;
    public Text CommentText;

    public delegate void QuitButtonClick();
    public delegate void ReturnButtonClick();
    public QuitButtonClick quitbtnClickEvent;
    public ReturnButtonClick returnbtnClickEvent;

    void Start()
    {
        ReturnBtn.onClick.AddListener(OnReturnbuttonClick);
        QuitBtn.onClick.AddListener(OnQuitbuttonClick);

        gameObject.SetActive(false);
    }
    public void show()
    {
        gameObject.SetActive(true);
    }

    public void OnReturnbuttonClick()
    {
        quitbtnClickEvent();
    }

    public void OnQuitbuttonClick()
    {
        returnbtnClickEvent();
    }
}
