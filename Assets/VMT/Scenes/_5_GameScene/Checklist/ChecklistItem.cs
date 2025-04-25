using UnityEngine;
using UnityEngine.UI;

public class ChecklistItem : MonoBehaviour
{
    //Managed
    public Text ChecklistText;
    public Image CheckBox;

    //Assign
    public Sprite CheckBoxEmptySprite;
    public Sprite CheckBoxTickSprite;

    public void SetText(string inText)
    {
        ChecklistText.text = inText;
    }
    public void SetCheckBoxStatus(bool isTick)
    {
        if(isTick)
        {
            CheckBox.sprite = CheckBoxTickSprite;
        }
        else
        {
            CheckBox.sprite = CheckBoxEmptySprite;
        }
    }
}
