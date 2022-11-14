using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.Events;

public class UserAccountManager : MonoBehaviour
{
    public static UserAccountManager Instance;

    public static UnityEvent OnSignInSuccess = new UnityEvent();

    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();

    public static UnityEvent<string> OnCreateAccountFailed = new UnityEvent<string>();

    void Awake() {
        Instance = this;
    }

    // Create Account
    public void CreateAccount (string username, string emailAddress, string password) {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest() {
                Email = emailAddress,
                Password = password,
                Username = username,
                RequireBothUsernameAndEmail = true
            },
            respons => {
                Debug.Log($"Sucessfull Create Account: {username}, {emailAddress}");
                // Once account created -- Call fn SignIn for auto login
                SignIn(username, password);
            },
            error => {
                Debug.Log($"Error when Create Account: {username}, {emailAddress} \n {error.ErrorMessage}");
                OnCreateAccountFailed.Invoke(error.ErrorMessage);
            }
        );
    }

    public void SignIn (string username, string password) {
        PlayFabClientAPI.LoginWithPlayFab (
            new LoginWithPlayFabRequest () {
                Username = username,
                Password = password
            },
            response => {
                Debug.Log($"Login Sucessfully : {username}");
                OnSignInSuccess.Invoke();
            },
            error => {
                Debug.Log($"Error While Login: {username} \n Error: {error.ErrorMessage}");
                OnSignInFailed.Invoke(error.ErrorMessage);
            }
        );
    }
}
