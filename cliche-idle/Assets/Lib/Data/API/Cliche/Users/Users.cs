using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Firewrap;

namespace Cliche.UserManagement
{
    // TODO: rename, merge with USER class
    public class Users
    {
        /// <summary>
        /// Gets the currently logged in user's data.
        /// </summary>
        /// <returns></returns>
        public static async Task<User> GetUserData()
        {
            string userEmail = Authentication.CurrentUser.Email;

            User queryUser = await Firestore.GetDocumentAsync<User>($"users/{userEmail}");

            // TODO: get an actual JSON serializer instead of JsonUtility to make this work
            if (!File.Exists(Application.persistentDataPath + "/user.json"))
            {
                string json = JsonUtility.ToJson(queryUser);
                File.WriteAllText(Application.persistentDataPath + "/user.json", json);
                Debug.Log("User data downloaded: " + json);
            }
            else
            {
                string json = JsonUtility.ToJson(queryUser);
                File.WriteAllText(Application.persistentDataPath + "/user.json", json);
                Debug.Log("User data downloaded: " + json);
                /*
                string json = File.ReadAllText(Application.persistentDataPath + "/user.json");
                Debug.Log("User data read locally: " + json);
                //User localUserData = JsonUtility.FromJson<User>(json);
                */
            }

            return queryUser;
        }

        // TODO: fix this
        public static async Task<User> SyncUserData()
        {
            string userEmail = Authentication.CurrentUser.Email;
            
            User queryUser = await Firestore.GetDocumentAsync<User>($"users/{userEmail}");
            return queryUser;
        }
    }
}