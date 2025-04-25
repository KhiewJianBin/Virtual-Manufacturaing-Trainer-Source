using System;
using System.Collections.Generic;

[Serializable]
public class LocalDatabase
{
    public List<Account> Accounts = new List<Account>();
}

[Serializable]
public class Account
{
    public string Username;
    public string Password;
}