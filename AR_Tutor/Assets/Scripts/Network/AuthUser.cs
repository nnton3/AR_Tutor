using System.Collections;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.Events;
using System.Threading.Tasks;

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
    }

    public void SetUserID(string _id)
    {
        UserID = _id;
    }

    public Task CreateUser(string email, string password)
    {
        return auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
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
            UserID = newUser?.UserId;
            Debug.LogFormat("Firebase user created successfully: {0} ({1})",
                newUser.DisplayName, newUser.UserId);
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
