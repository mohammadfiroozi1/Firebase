using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text feedbackText;
    
    private FirebaseAuth auth;

    [SerializeField] Button loginBtn;
    void Start()
    {
        InitializeFirebase();
        loginBtn.onClick.AddListener(OnLoginButtonClicked);
    }

    void InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
            Debug.Log("Firebase initialized");
        });
    }

    public void OnLoginButtonClicked()
    {
        string username = usernameInputField.text;
        string password = passwordInputField.text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            feedbackText.text = "Please fill in both fields.";
            return;
        }

        SignInUser(username, password);
    }

    void SignInUser(string email, string password)
    {
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                feedbackText.text = "Sign-in canceled.";
                return;
            }
            if (task.IsFaulted)
            {
                feedbackText.text = "Sign-in error: " + task.Exception;
                return;
            }

            FirebaseUser newUser = task.Result.User;
            feedbackText.text = "User signed in successfully: " + newUser.DisplayName + " (" + newUser.Email + ")";
        });
    }
}
