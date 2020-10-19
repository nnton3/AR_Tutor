using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.Events;

public class AuthUser : MonoBehaviour
{
    #region Variables
    private FirebaseUser newUser = null;
    public FirebaseUser NewUser => newUser;
    private FirebaseAuth auth;
    [SerializeField] private UnityEngine.UI.Text status;
    public bool IsSignIn { get; private set; } = false;
    public UnityEvent UserSignUp;
    #endregion

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void CreateUser(string email, string password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                return;
            }

            // Firebase user has been created.
            newUser = task.Result;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
        });
    }

    public void SignIn(string email, string password)
    {
        if (email == "" || password == "")
        {
            Debug.Log("Email or password are not correct");
            return;
        }

        StartCoroutine(SignInRoutine(email, password));
    }

    public IEnumerator SignInRoutine(string email, string password)
    {
        var authTask = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                newUser = null;
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                newUser = null;
                return;
            }
            Debug.Log("save user in variable");
            newUser = task.Result;
        });

        yield return new WaitUntil(() => authTask.IsCompleted);
        Debug.Log("move next");
        if (newUser != null)
        {
            IsSignIn = true;
            Debug.Log("User signed in successfully");
        }
    }
}
