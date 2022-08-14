using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Tooling.Firebase
{
    public class FirebaseAccountManager
    {
        public static FirebaseAccount GetCurrentlySelectedAccountData()
        {
            Debug.Log($"<color=green>Editor-time automatic login tool started.</color>");
            if (File.Exists(Application.persistentDataPath + "/accounts.json"))
            {
                // Read tool data
                string accountsJson = File.ReadAllText(Application.persistentDataPath + "/accounts.json");
                FirebaseAccountsContainer AccountList = JsonUtility.FromJson<FirebaseAccountsContainer>(accountsJson);
                if (AccountList.Accounts == null)
                {
                    // File exists but it's empty
                    Debug.LogWarning($"<color=yellow>Editor-time automatic login file empty.</color>");
                }
                else
                {
                    // File exists with a valid structure; check to see if there is a selected account
                    FirebaseAccount Account = AccountList.Accounts.Find(account => account.selected == true);
                    if (Account != null)
                    {
                        // Login with the selected account
                        Debug.Log($"<color=green>Editor-time automatic login configuration found; login flow started.</color>");
                        return Account;
                    }
                    else
                    {
                        Debug.LogWarning($"<color=yellow>Could not find selected Editor-time automatic login account; manual login will be required.</color>");
                    }
                }        
            }
            else
            {
                Debug.LogWarning($"<color=yellow>Could not find Editor-time automatic login file; manual login will be required.</color>");
            }
            return null;
        }

        public void SaveToAccountsFile(FirebaseAccountsContainer contents)
        {
            contents.lastModifiedDate = DateTime.UtcNow.ToString();
            string accountsJson = JsonUtility.ToJson(contents);
            File.WriteAllText(Application.persistentDataPath + "/accounts.json", accountsJson);
        }

        public void AddAccount()
        {

        }
    }

    [Serializable]
    public class FirebaseAccountsContainer
    {
        public string lastModifiedDate;
        public List<FirebaseAccount> Accounts;
    }

    [Serializable]
    public class FirebaseAccount
    {
        public string addedDate;
        public string updatedDate;
        public string username;
        public string password;
        public bool selected;
    }

}