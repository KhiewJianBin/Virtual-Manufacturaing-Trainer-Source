using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;
using Random = System.Random;

public class Game : MonoBehaviour
{
    /* hardcoded scenario
    
    1. Make 3 Objects
        c1 Spawn 3 Objects
        c9 Finish

    2. Make 3 [RandomColor] Object
        c1 Spawn 3 Objects
        c2 Paint Objects as [RandomColor] : 3 colors
        c9 Finish

    3. Make 3 [RandomSize] Object
        c1 Spawn 3 Objects
        c2 Resize Objects to [RandomSize] : 3 size
        c9 Finish
    */

    //Managed
    public Player player;
    public FirstPersonController firstPersonController;
    public Console console;
    public GameObject TrainingArrow;
    public TextMeshPro TrainingArrowText;

    public Text SceanarioCheckListText;
    public Text ModeText;
    public Animator ChecklistAnimator;
    public Button HideShowButton;
    public Button GiveUpButton;
    public GameOverUI gameOverUI;

    public AudioSource GameAudioSource;

    //Assign
    List<ChecklistItem> checkList = new List<ChecklistItem>();
    public Transform CheckListItemContainer;
    public GameObject CheckListItemPrefab;

    List<VMT_Object> spawnedObjects = new List<VMT_Object>();
    public BoxCollider ObjectSpawnArea;
    public GameObject BoxPrefab;
    public GameObject BallPrefab;
    public GameObject PipePrefab;

    public AudioClip CorrectSnd;
    public AudioClip WrongSnd;

    public GameObject GameOverCamera;

    //Game Infos
    int currentScenarioNum;
    bool isTrainingMode;

    int targetObjectsSize = -1;
    VMT_Object.Colors targetObjectsColor = VMT_Object.Colors.White;

    int currentObjectsSize = 1;
    const int MIN_ObjectsSize = 1;
    const int MAX_ObjectsSize = 3;
    VMT_Object.Colors currentObjectsColor = VMT_Object.Colors.White;

    //Our Sceneario are basic, for now we do a simple number climb in checkng for condition
    int checkListClearedAmt = 0;

    bool isGameOver;

    void Start()
    {
        HideShowButton.onClick.AddListener(OnHideShowButtonClick);
        GiveUpButton.onClick.AddListener(OnGiveUpButtonClick);

        console.button1ClickEvent += SpawnOneRandomObject;
        console.button2ClickEvent += PaintAllObject_RandomColor;
        console.button3ClickEvent += IncreaseAllObjectSize;
        console.button4ClickEvent += DecreaseAllObjectSize;
        console.button5ClickEvent += FinishManufacturing;

        gameOverUI.quitbtnClickEvent += ReturnToScenarioScene;
        gameOverUI.returnbtnClickEvent += QuitGame;

        //Set Game Defaults, just to make sure in case the scene was not already setup in default
        console.ChangeColorIndicator(currentObjectsColor);
        console.ChangeSizeIndicator(currentObjectsSize);

        //Setup Game - mode, scenario & checklist
        currentScenarioNum = Session.Instance.sessionScenarioNum;
        isTrainingMode = Session.Instance.sessionTrainingMode;
        SceanarioCheckListText.text = "Sceanario " + currentScenarioNum;
        if (isTrainingMode)
        {
            ModeText.text = "[Training Mode]";
            Session.Instance.WriteLog(string.Format("Player has selected {0} mode and Scenario {1}", "[Training Mode]", currentScenarioNum));
        } 
        else
        {
            ModeText.text = "[Assessment Mode]";
            Session.Instance.WriteLog(string.Format("Player has selected {0} mode and Scenario {1}", "[Assessment Mode]", currentScenarioNum));
        }

        ChecklistItem newItem;
        if (currentScenarioNum == 1) // Make 3 Object
        {
            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.   Spawn 3 Objects", checkList.Count + 1));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);

            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.   Finish", checkList.Count + 1));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);
        }
        else if (currentScenarioNum == 2) // Make 3[RandomColor] Object
        {
            //Set Objective as - random color except white
            Array values = Enum.GetValues(typeof(VMT_Object.Colors));
            VMT_Object.Colors randomColor = (VMT_Object.Colors)values.GetValue(new Random().Next(1, values.Length));
            targetObjectsColor = randomColor;

            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.   Spawn 3 Objects", checkList.Count + 1));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);

            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.   Paint Objects With {1} Color", checkList.Count + 1, Enum.GetName(targetObjectsColor.GetType(), targetObjectsColor)));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);

            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.   Finish", checkList.Count + 1));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);
        }
        if (currentScenarioNum == 3) // Make 3[RandomSize] Object
        {
            //Set Objective as - random size
            targetObjectsSize = new Random().Next(MIN_ObjectsSize, MAX_ObjectsSize);

            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.  Spawn 3 Objects", checkList.Count + 1));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);

            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.   Resize Objects to Size {1}", checkList.Count + 1, targetObjectsSize));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);

            newItem = Instantiate(CheckListItemPrefab, CheckListItemContainer).GetComponent<ChecklistItem>();
            newItem.SetText(string.Format("{0}.   Finish", checkList.Count + 1));
            newItem.SetCheckBoxStatus(false);
            checkList.Add(newItem);
        }
        
        //doing this at start to trigger training mode
        VerifyCheckList();
    }

    void SpawnOneRandomObject()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 1));

        //Decide what shape - random it
        Array values = Enum.GetValues(typeof(VMT_Object.Shapes));
        VMT_Object.Shapes randomShape = (VMT_Object.Shapes)values.GetValue(new Random().Next(values.Length));

        //Create Unity Go from Prefab
        GameObject newObject;
        if (randomShape == VMT_Object.Shapes.Box)
        {
            newObject = Instantiate(BoxPrefab);

            Session.Instance.WriteLog(string.Format("Box Created"));
        }
        else if (randomShape == VMT_Object.Shapes.Ball)
        {
            newObject = Instantiate(BallPrefab);

            Session.Instance.WriteLog(string.Format("Ball Created"));
        }
        else
        {
            newObject = Instantiate(PipePrefab);

            Session.Instance.WriteLog(string.Format("Pipe Created"));
        }

        newObject.transform.position = Helper.RandomPointInBounds(ObjectSpawnArea.bounds);

        spawnedObjects.Add(newObject.GetComponent<VMT_Object>());

        Session.Instance.WriteLog(string.Format("{0} Created ",Enum.GetName(randomShape.GetType(), randomShape)));

        VerifyCheckList();
    }
    void PaintAllObject_RandomColor()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 2));

        //Decide what color except white - random it
        Array values = Enum.GetValues(typeof(VMT_Object.Colors));
        VMT_Object.Colors randomColor = (VMT_Object.Colors)values.GetValue(new Random().Next(1,values.Length));

        currentObjectsColor = randomColor;

        //Change Objects Colors
        foreach (VMT_Object obj in spawnedObjects)
        {
            obj.ChangeColor(currentObjectsColor);
        }

        //Change Console Color Indicator
        console.ChangeColorIndicator(currentObjectsColor);

        Session.Instance.WriteLog(string.Format("Object Colors Changed To {0}", Enum.GetName(currentObjectsColor.GetType(), currentObjectsColor)));

        VerifyCheckList();
    }
    void IncreaseAllObjectSize()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 3));

        currentObjectsSize = Mathf.Clamp(++currentObjectsSize,MIN_ObjectsSize, MAX_ObjectsSize);
        foreach (VMT_Object obj in spawnedObjects)
        {
            obj.ChangeSize(currentObjectsSize);
        }

        //Change Console Size Indicator
        console.ChangeSizeIndicator(currentObjectsSize);

        Session.Instance.WriteLog(string.Format("Object Size Increased To {0}", currentObjectsSize));

        VerifyCheckList();
    }
    void DecreaseAllObjectSize()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 4));

        currentObjectsSize = Mathf.Clamp(--currentObjectsSize, MIN_ObjectsSize, MAX_ObjectsSize);
        foreach (VMT_Object obj in spawnedObjects)
        {
            obj.ChangeSize(currentObjectsSize);
        }

        //Change Console Size Indicator
        console.ChangeSizeIndicator(currentObjectsSize);

        Session.Instance.WriteLog(string.Format("Object Size Decreased To {0}", currentObjectsSize));

        VerifyCheckList();
    }
    void FinishManufacturing()
    {
        Session.Instance.WriteLog(string.Format("Player has clicked button {0}", 5));

        //Destroy all GameObject
        foreach (VMT_Object obj in spawnedObjects)
        {
            obj.Kill();
        }

        //Freeze
        player.enabled = false;
        firstPersonController.enabled = false;
        isGameOver = true;

        //Check if this is the last checklist to clear,
        if(checkListClearedAmt == checkList.Count-1)
        {
            checkListClearedAmt++;
        }

        if (Session.Instance.sessionTrainingMode)
        {
            //Training End Condition Check (checklist completed)
            if(checkListClearedAmt == checkList.Count)
            {
                gameOverUI.GameoverText.text = "Training Passed";
                gameOverUI.CommentText.text = string.Format("Player has [Completed] Scenario {0} training.", currentScenarioNum);
                gameOverUI.show();

                PlayCorrectAnswerSound();

                Session.Instance.WriteLog(string.Format("Player has [Completed] Scenario {0} training.", currentScenarioNum));
            }
            else
            {
                gameOverUI.GameoverText.text = "Training Failed";
                gameOverUI.CommentText.text = string.Format("Player has [Failed] Scenario {0} training.", currentScenarioNum);
                gameOverUI.show();

                PlayWrongAnswerSound();

                Session.Instance.WriteLog(string.Format("Player has [Failed] Scenario {0} training.", currentScenarioNum));
            }
        }
        else
        {
            //Assessment End Condition Check (checklist cleared/completed)
            if (checkListClearedAmt == checkList.Count)
            {
                gameOverUI.GameoverText.text = "Assessment Passed";
                gameOverUI.CommentText.text = string.Format("Player has [Completed] Scenario {0} assessment.", currentScenarioNum);
                gameOverUI.show();

                PlayCorrectAnswerSound();

                Session.Instance.WriteLog(string.Format("Player has [Completed] Scenario {0} assessment.", currentScenarioNum));
            }
            else
            {
                gameOverUI.GameoverText.text = "Assessment Failed";
                gameOverUI.CommentText.text = string.Format("Player has [Failed] Scenario {0} assessment.", currentScenarioNum);
                gameOverUI.show();

                PlayWrongAnswerSound();

                Session.Instance.WriteLog(string.Format("Player has [Failed] Scenario {0} assessment.", currentScenarioNum));
            }
        }
    }

    void VerifyCheckList()
    {
        //Check depending on senario and checklistclearamt
        switch(currentScenarioNum)
        {
            case 1:
                {
                    //Check if checklist is cleared to promote
                    if (checkListClearedAmt == 0)
                    {
                        //Check If Spawn 3 Objects
                        if(spawnedObjects.Count >= 3)
                        {
                            checkListClearedAmt++;
                        }
                    }

                    //Do Training
                    if (isTrainingMode)
                    {
                        if (checkListClearedAmt == 0)
                        {
                            console.DisableAllButtons();
                            console.EnableButton(1);
                            TrainingArrow.transform.position = console.button1.transform.position;
                            TrainingArrowText.text = "Push To Spawn Object";
                        }
                        else if (checkListClearedAmt == 1)
                        {
                            console.DisableAllButtons();
                            console.EnableButton(5);
                            TrainingArrow.transform.position = console.button5.transform.position;
                            TrainingArrowText.text = "Push To Submit Your Work";
                        }
                    }
                }
                break;
            case 2:
                {
                    //Check if checklist is cleared to promote
                    if (checkListClearedAmt == 0)
                    {
                        //Check If Spawn 3 Objects
                        if (spawnedObjects.Count >= 3)
                        {
                            checkListClearedAmt++;
                        }
                    }
                    if (checkListClearedAmt == 1)
                    {
                        //Check If Current Color is correct
                        if(currentObjectsColor == targetObjectsColor)
                        {
                            checkListClearedAmt++;
                        }
                    }
                    if (checkListClearedAmt == 2)
                    {
                        //Check if Current Color is wrong
                        if (currentObjectsColor != targetObjectsColor)
                        {
                            checkListClearedAmt--;
                        }
                    }

                    //Do Training
                    if (isTrainingMode)
                    {
                        if (checkListClearedAmt == 0)
                        {
                            console.DisableAllButtons();
                            console.EnableButton(1);
                            TrainingArrow.transform.position = console.button1.transform.position;
                            TrainingArrowText.text = "Push To Spawn Object";
                        }
                        else if (checkListClearedAmt == 1)
                        {
                            console.DisableAllButtons();
                            console.EnableButton(2);
                            TrainingArrow.transform.position = console.button2.transform.position;
                            TrainingArrowText.text = "Push To Paint Objects";
                        }
                        else if (checkListClearedAmt == 2)
                        {
                            console.DisableAllButtons();
                            console.EnableButton(5);
                            TrainingArrow.transform.position = console.button5.transform.position;
                            TrainingArrowText.text = "Push To Submit Your Work";
                        }
                    }
                }
                break;
            case 3:
                {
                    //Check if checklist is cleared to promote
                    if (checkListClearedAmt == 0)
                    {
                        //Check If Spawn 3 Objects
                        if (spawnedObjects.Count >= 3)
                        {
                            checkListClearedAmt++;
                        }
                    }
                    if (checkListClearedAmt == 1)
                    {
                        //Check If Current Size is correct
                        if (currentObjectsSize == targetObjectsSize)
                        {
                            checkListClearedAmt++;
                        }
                    }
                    if (checkListClearedAmt == 2)
                    {
                        //Check if Current size is wrong
                        if (currentObjectsSize != targetObjectsSize)
                        {
                            checkListClearedAmt--;
                        }
                    }

                    //Do Training
                    if (isTrainingMode)
                    {
                        if (checkListClearedAmt == 0)
                        {
                            console.DisableAllButtons();
                            console.EnableButton(1);
                            TrainingArrow.transform.position = console.button1.transform.position;
                            TrainingArrowText.text = "Push To Spawn Object";
                        }
                        else if (checkListClearedAmt == 1)
                        {
                            //check if we should increase or decrease size
                            if(currentObjectsSize<targetObjectsSize)
                            {
                                console.DisableAllButtons();
                                console.EnableButton(3);
                                TrainingArrow.transform.position = console.button3.transform.position;
                                TrainingArrowText.text = "Push To Increase Objects Size";
                            }
                            else
                            {
                                console.DisableAllButtons();
                                console.EnableButton(4);
                                TrainingArrow.transform.position = console.button4.transform.position;
                                TrainingArrowText.text = "Push To Decrease Objects Size";
                            }
                        }
                        else if (checkListClearedAmt == 2)
                        {
                            console.DisableAllButtons();
                            console.EnableButton(5);
                            TrainingArrow.transform.position = console.button5.transform.position;
                            TrainingArrowText.text = "Push To Submit Your Work";
                        }
                    }
                }
                break;
        }

        //Activate/swap checklist checkbox sprite texture
        for (int i = 0; i < checkList.Count; i++)
        {
            if (i < checkListClearedAmt)
                checkList[i].SetCheckBoxStatus(true);
            else
                checkList[i].SetCheckBoxStatus(false);
        }
    }

    void Update()
    {
        if(isGameOver)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

            GameOverCamera.SetActive(true);
        }
        else
        {
            //Extra action to hide/show checklist
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                OnHideShowButtonClick();
            }
        }
    }



    //Helper
    void PlayCorrectAnswerSound()
    {
        GameAudioSource.PlayOneShot(CorrectSnd);
    }
    void PlayWrongAnswerSound()
    {
        GameAudioSource.PlayOneShot(WrongSnd);
    }



    void OnHideShowButtonClick()
    {
        ChecklistAnimator.SetTrigger("HIDESHOW");
        player.enabled = !player.enabled;
        firstPersonController.enabled = !firstPersonController.enabled;
        Cursor.visible = !player.enabled;
        Cursor.lockState = CursorLockMode.None;
    }
    void OnGiveUpButtonClick()
    {
        //Freeze
        player.enabled = false;
        firstPersonController.enabled = false;
        isGameOver = true;

        if (Session.Instance.sessionTrainingMode)
        {
            gameOverUI.GameoverText.text = "Training Failed";
            gameOverUI.CommentText.text = string.Format("Player has [Failed] Scenario {0} training.", currentScenarioNum);
            gameOverUI.show();

            PlayWrongAnswerSound();

            Session.Instance.WriteLog(string.Format("Player has [Failed] Scenario {0} training.", currentScenarioNum));
        }
        else
        {
            gameOverUI.GameoverText.text = "Assessment Failed";
            gameOverUI.CommentText.text = string.Format("Player has [Failed] Scenario {0} assessment.", currentScenarioNum);
            gameOverUI.show();

            PlayWrongAnswerSound();

            Session.Instance.WriteLog(string.Format("Player has [Failed] Scenario {0} assessment.", currentScenarioNum));
        }
    }

    void ReturnToScenarioScene()
    {
        SceneManager.LoadScene("ScenarioSelectionScene");
    }
    void QuitGame()
    {
        Session.Instance.Logout();

        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
