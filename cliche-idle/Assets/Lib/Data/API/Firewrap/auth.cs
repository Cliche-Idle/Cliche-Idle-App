using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Firebase.Auth;
using Tooling.Firebase;

namespace Firewrap
{
    public class Authentication
    {
        /// <summary>
        /// The Default Firebase Auth Instance used to provide authentication functions.
        /// </summary>
        public static FirebaseAuth FirebaseAuthInstance = FirebaseAuth.DefaultInstance;

        /// <summary>
        /// The currently logged in user's data.
        /// </summary>
        public static FirebaseUser CurrentUser;

        /// <summary>
        /// Checks whether the user have been logged in automatically by the Firebase API, and if a valid login has been found, starts the login success flow.
        /// 
        /// Unity Editor only: Check whether a test account has been selected as the default debug account, and automatcally logs in with it on startup.
        /// </summary>
        public static void AttemptSessionRestoreAfterRestart()
        {
            if (Application.isEditor)
            {
                /*
                    ! This part is only for Editor time execution.
                
                    Checks for a valid accounts.json file, created by the custom Firabse Account Management tool.
                    This pretty much copies the automatic login the Firebase SDK does in production (which is disabled in development),
                    with the added functionality of being able to choose which account to use during testing.

                */
                FirebaseAccount debugAccount = FirebaseAccountManager.GetCurrentlySelectedAccountData();
                if (debugAccount != null)
                {
                    // Missing async-await warning here; it can be safely ignored.
                    #pragma warning disable CS4014
                    LoginUserAsync(debugAccount.username, debugAccount.password);
                    #pragma warning restore CS4014
                }
            }
            else
            {
                // ? See this issue for more details on why this works:
                // ? https://github.com/firebase/quickstart-unity/issues/139
                if (FirebaseAuthInstance.CurrentUser != null)
                {
                    CurrentUser = FirebaseAuthInstance.CurrentUser;
                    Debug.Log($"<color=green>Automatic login successful ({CurrentUser.Email}).</color>");
                    OnLogin.Invoke(null, new AuthEventArgs(CurrentUser, null, null));
                }
                else
                {
                    Debug.LogWarning($"<color=yellow>Automatic login failed (no saved profile found).</color>");
                }
            }
        }

        /// <summary>
        /// An Event that fires when the user has successfully logged in.
        /// </summary>
        public static event EventHandler<AuthEventArgs> OnLogin;

        /// <summary>
        /// Login with an user account, using an email address and password. This is used for logging in with an app-specific account.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static async Task LoginUserAsync(string email, string password)
        {
            Firebase.FirebaseException loginException = new Firebase.FirebaseException();
            try
            {
                CurrentUser = await FirebaseAuthInstance.SignInWithEmailAndPasswordAsync(email, password);
            }
            catch (AggregateException e)
            {
                loginException = (Firebase.FirebaseException)e.GetBaseException();
            }
            // Fire update event
            if (CurrentUser != null)
            {
                Debug.Log($"<color=green>Login successful ({CurrentUser.Email}).</color>");
                OnLogin.Invoke(null, new AuthEventArgs(CurrentUser, null, null));
            }
            else
            {
                
                string userMessage = GetUserFriendlyErrorMessage((AuthError)loginException.ErrorCode);
                Debug.LogError($"<color=red>Login failed ({email}): {userMessage}.</color>");
                OnLogin.Invoke(null, new AuthEventArgs(null, userMessage, loginException));
            }
        }

        /// <summary>
        /// An Event that fires when the user has successfully registered in.
        /// </summary>
        public static event EventHandler<AuthEventArgs> OnRegister;

        /// <summary>
        /// Register an user with a new account, using an email address and password. This creates an app-specific account.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public static async Task RegisterUserAsync(string email, string password)
        {
            Firebase.FirebaseException registerEx = new Firebase.FirebaseException();
            try
            {
                CurrentUser = await FirebaseAuthInstance.CreateUserWithEmailAndPasswordAsync(email, password);
            }
            catch (AggregateException e)
            {
                registerEx = (Firebase.FirebaseException)e.GetBaseException();
            }
            // Fire update event
            if (CurrentUser != null)
            {
                Debug.Log($"<color=green>Registration successful ({CurrentUser.Email}).</color>");
                OnRegister.Invoke(null, new AuthEventArgs(CurrentUser, null, null));
            }
            else
            {
                string userMessage = GetUserFriendlyErrorMessage((AuthError)registerEx.ErrorCode);
                Debug.LogError($"<color=red>Registration failed ({email}): {userMessage}.</color>");
                OnRegister.Invoke(null, new AuthEventArgs(null, userMessage, registerEx));
            }
        }

        /// <summary>
        /// An Event that fires when the user has successfully logged out.
        /// </summary>
        public static event EventHandler OnLogout;
        
        /// <summary>
        /// Logs out the current user from the system.
        /// </summary>
        public static void LogoutUser()
        {
            string userName = CurrentUser.Email;
            // Sign out in the instance
            FirebaseAuthInstance.SignOut();
            // Remove user reference
            CurrentUser.Dispose();
            // Fire update event
            Debug.Log($"<color=green>Logout successful ({userName}).</color>");
            OnLogout.Invoke(null, EventArgs.Empty);
        }

        private static string GetUserFriendlyErrorMessage(AuthError errorCode)
        {
            var message = "";
            switch (errorCode)
            {
                case AuthError.MissingPassword:
                    message = "Please enter a valid password.";
                    break;
                case AuthError.WeakPassword:
                    message = "The given password is too weak.";
                    break;
                case AuthError.WrongPassword:
                    message = "The given username or password are incorrect.";
                    break;
                case AuthError.EmailAlreadyInUse:
                    message = "This username is already in use.";
                    break;
                case AuthError.InvalidEmail:
                    message = "The given email is invalid.";
                    break;
                case AuthError.MissingEmail:
                    message = "Please enter a valid email address.";
                    break;
                default:
                    message = $"An unknown error occured. Please check your internet connection and try again later. (Code: {errorCode})";
                    break;
            }
            return message;
        }
    }

    public class AuthEventArgs : EventArgs
    {
        private FirebaseUser user;
        private Firebase.FirebaseException error;
        private string userFriendlyMessage;

        public AuthEventArgs(FirebaseUser eventUser, string frontEndStatusMessage, Firebase.FirebaseException ex)
        {
            user = eventUser;
            userFriendlyMessage = frontEndStatusMessage;
            error = ex;
        }

        /// <summary>
        /// The user's data if the authentication was successful.
        /// </summary>
        public FirebaseUser User 
        {
            get { return user; }
        }

        /// <summary>
        /// An user friendly status message. This can be displayed to the user.
        /// </summary>
        public string UserFriendlyMessage 
        {
            get { return userFriendlyMessage; }
        }

        /// <summary>
        /// The internal error if the authentication failed.
        /// </summary>
        public Firebase.FirebaseException Error
        {
            get { return error; }
        }
    }
}