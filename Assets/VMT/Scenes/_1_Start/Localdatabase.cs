using System.IO;
using UnityEngine;

public class Localdatabase : MonoBehaviour
{
    private static Localdatabase instance;
    public static Localdatabase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("Localdatabase").AddComponent<Localdatabase>();
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

        LoadDB();
    }

    public void Start()
    {
        //SaveTempAccounts_Override();
        //LoadAndPrintAccounts();
    }

    LocalDatabase LocalDB = new LocalDatabase();
    void LoadDB()
    {
        string LOGPATH = Application.persistentDataPath;
        string LOGFILENAME = "VMT_TRAINEE_ACCOUNT";
        string LOGFILEEXTENSION = ".txt";
        string LOGFULLPATH = Path.Combine(LOGPATH, LOGFILENAME + LOGFILEEXTENSION);

        // Check If Log File does not exist
        if (!File.Exists(LOGFULLPATH))
        {
            //Create file + Load some default accounts and save
            SaveTempAccounts_Override();
        }
        else
        {
            LocalDB = JSONExportImporter.ImportFromJson<LocalDatabase>(LOGFULLPATH);
        }
    }
    void SaveDB()
    {
        string LOGPATH = Application.persistentDataPath;
        string LOGFILENAME = "VMT_TRAINEE_ACCOUNT";
        string LOGFILEEXTENSION = ".txt";
        string LOGFULLPATH = Path.Combine(LOGPATH, LOGFILENAME + LOGFILEEXTENSION);

        JSONExportImporter.ExportToJson<LocalDatabase>(LOGFULLPATH, LocalDB);
    }
    // I simply use username as the UID for Account Lookup
    // aka username is Unique
    public Account GetAccountCredentials(string requestingUsername) 
    {
        foreach (Account account in LocalDB.Accounts)
        {
            if(account.Username == requestingUsername)
            {
                return account;
            }
        }
        return null;
    }

    public void SaveLog(string logToSave)
    {
        string LOGPATH = Application.persistentDataPath;
        string LOGFILENAME = "VMT_TRAINEE_LOG";
        string LOGFILEEXTENSION = ".txt";
        string LOGFULLPATH = Path.Combine(LOGPATH, LOGFILENAME + LOGFILEEXTENSION);

        // Check If Log File does not exist
        if (!File.Exists(LOGFULLPATH))
        {
            using (StreamWriter sw = File.CreateText(LOGFULLPATH))
            {
                sw.WriteLine(logToSave);
                sw.Close();
            }
        }
        else
        {
            // Append To Text file
            using (StreamWriter sw = File.AppendText(LOGFULLPATH))
            {
                sw.WriteLine(logToSave);
                sw.Close();
            }
        }
    }

    //Testing Code for Local DB Accounts
    void SaveTempAccounts_Override()
    {
        Account a1 = new Account
        {
            Username = "user1",
            Password = "pass1"
        };

        Account a2 = new Account
        {
            Username = "user2",
            Password = "pass2"
        };

        Account a3 = new Account
        {
            Username = "user3",
            Password = "pass3"
        };

        LocalDB = new LocalDatabase();
        LocalDB.Accounts.Add(a1);
        LocalDB.Accounts.Add(a2);
        LocalDB.Accounts.Add(a3);

        SaveDB();
    }
    void LoadAndPrintAccounts()
    {
        LoadDB();

        print("Account: [Username,Password]\n");
        foreach (Account account in LocalDB.Accounts)
        {
            print(string.Format("Account: [{0},{1}]\n", account.Username, account.Password));
        }
    }
}
