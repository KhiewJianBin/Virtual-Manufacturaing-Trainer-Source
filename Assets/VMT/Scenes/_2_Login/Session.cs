using UnityEngine;

public class Session : MonoBehaviour
{
    private static Session instance;
    public static Session Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("Session").AddComponent<Session>();
            }

            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (Instance != this)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this); //For Persisting Though Scenes
    }

    //For Real DB should async and callback func
    public delegate void AuthenticateUserCallback(bool result);
    public void AuthenticateUser(string user, string pass, AuthenticateUserCallback callback) //AKA Login
    {
        Account lookupAccount = Localdatabase.Instance.GetAccountCredentials(user);

        //No account of that entered username
        if (lookupAccount == null)
        {
            callback(false);
        }

        //Check if the user's entered password matches the lookuped account's password
        else if(pass != lookupAccount.Password)
        {
            callback(false);
        }
        else
        {
            InitSession(lookupAccount);
            callback(true);
        }
    }
    public void Logout()
    {
        EndSession();
    }

    enum SessionStatus { ON, OFF }
    SessionStatus sessionstatus;
    string sessionID;
    string sessionLog;

    public bool sessionTrainingMode = false;
    public int sessionScenarioNum = 2;
    public Account currentUser;

    void InitSession(Account user)
    {
        sessionstatus = SessionStatus.ON;
        sessionID = System.Guid.NewGuid().ToString();
        sessionLog = string.Empty;
        currentUser = user;

        WriteLog("Login Successful!");
        WriteLog("Session Started");
        WriteLog("--------------------");
        WriteLog("Session ID: " + sessionID);
        WriteLog("User: " + currentUser.Username);
        WriteLog("--------------------");
        WriteLog(currentUser.Username + " has logged in.");
    }
    void EndSession()
    {
        if (currentUser == null)
        {
            Account invalid = new Account
            {
                Username = "Invalid",
                Password = "Invalid"
            };
            currentUser = invalid;
        }

        WriteLog(currentUser.Username + " has logged out.");
        WriteLog("Session " + sessionID + " Has Ended");
        WriteLog("--------------------");

        Localdatabase.Instance.SaveLog(sessionLog);

        sessionstatus = SessionStatus.OFF;
        sessionID = "NULL";
        sessionLog = string.Empty;
        currentUser = null;
    }

    public void WriteLog(string message)
    {
        sessionLog += message + "\n";
        Debug.Log(message);
    }

    void OnApplicationQuit()
    {
        //User Quits App Without Logging Out
        if(sessionstatus == SessionStatus.ON)
        {
            WriteLog(currentUser.Username + " has forced quit.");
            WriteLog("--------------------");
            Localdatabase.Instance.SaveLog(sessionLog);
        }
    }
}
