using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

using Cliche.UserManagement;
using Firewrap;

public class AppAuthFlowHandler : MonoBehaviour
{
    public UIDocument MainDocument;
    public VisualElement ContentContainer;

    // Start is called before the first frame update
    void Start()
    {
        Authentication.OnLogin += OnLogin;
        Authentication.OnRegister += OnRegister;
        Authentication.AttemptSessionRestoreAfterRestart();

        MainDocument = GetComponent<UIDocument>();
        ContentContainer = MainDocument.rootVisualElement.Q<VisualElement>("Container");
    }

    public async void OnLogin(object sender, AuthEventArgs auth) {
        if (auth.User != null)
        {
            User testUser = await Users.GetUserData();
            Debug.Log(testUser.character.general.name);
            // Check for a valid registration
            //    None    -> Redirect to the registration page
            //    Valid   -> Switch scene to main 
        }
        else
        {
            // Display error message
            Debug.LogError(auth.UserFriendlyMessage);
        }
    }

    public async void OnRegister(object sender, AuthEventArgs auth) {
        if (auth.User != null)
        {
            // Redirect to the registration page
        }
        else
        {
            // Display error message
            Debug.LogError(auth.UserFriendlyMessage);
        }
    }
}
