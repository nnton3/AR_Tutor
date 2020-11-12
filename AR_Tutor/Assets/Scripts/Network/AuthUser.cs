using System.Collections;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.Events;
using System.Threading.Tasks;

public enum SignUpResult { Success, EmailAlreadyUse, Canceled, Faulted}

public class AuthUser : MonoBehaviour
{
    #region Variables
    private FirebaseUser newUser = null;
    public FirebaseUser NewUser => newUser;
    public string UserID { get; private set; }
    private FirebaseAuth auth;
    [SerializeField] private UnityEngine.UI.Text status;
    public UnityEvent UserSignUp;
    #endregion

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        //Test();
    }

    private void Test()
    {
        var task = auth.FetchProvidersForEmailAsync("").ContinueWith((authTask) =>
        {
            if (authTask.IsCanceled)
            {
                Debug.Log("Provider fetch canceled.");
            }
            else if (authTask.IsFaulted)
            {
                Debug.Log("Provider fetch encountered an error.");
                Debug.Log(authTask.Exception.ToString());
            }
            else if (authTask.IsCompleted)
            {
                Debug.Log("Email Providers:");
                foreach (string provider in authTask.Result)
                {
                    Debug.Log(provider);
                }
            }
        });
    }

    public void SetUserID(string _id)
    {
        UserID = _id;
    }

    public Task<SignUpResult> CreateUser(string email, string password)
    {
        return auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
                return SignUpResult.Canceled;

            if (task.IsFaulted)
            {
                if (task.Exception.ToString().Contains("The email address is already in use"))
                    return SignUpResult.EmailAlreadyUse;
                return SignUpResult.Faulted;
            }

            // Firebase user has been created.
            newUser = task.Result;
            UserID = newUser?.UserId;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);

            return SignUpResult.Success;
        });
    }

    public void SignIn(string email, string password)
    {
        if (email == "" || password == "")
        {
            Signals.ShowNotification.Invoke("Ошибка! Не удалось войти в учетную запись, проверьте правильность ввода эл.почты и пароля");
            return;
        }

        StartCoroutine(SignInRoutine(email, password));
    }

    public IEnumerator SignInRoutine(string email, string password)
    {
        var authTask = auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                newUser = null;
                return;
            }

            Debug.Log("save user in variable");
            newUser = task.Result;
        });

        yield return new WaitUntil(() => authTask.IsCompleted);

        if (newUser == null)
            Signals.ShowNotification.Invoke("Ошибка! Не удалось войти в учетную запись, проверьте правильность ввода эл.почты и пароля");
    }

    public void ResetPassword(string _email)
    {
        auth.SendPasswordResetEmailAsync(_email)
            .ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                    Signals.ShowNotification.Invoke("Ошибка! Письмо для сброса пароля не было отправлено на электронную почту.");
                else
                    Signals.ShowNotification.Invoke("Письмо для сброса пароля было отправлено на электронную почту.");
            });
    }
}
