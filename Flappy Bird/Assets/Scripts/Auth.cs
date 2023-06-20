using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Auth : MonoBehaviour
{
    private FirebaseAuth auth;

    [Header("Links")] 
    [SerializeField] InputField EmailField;
    [SerializeField] InputField PasswordField;
    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        
    }
    private void Auth_StateChanged(object sender, System.EventArgs e)
    {
        if (auth.CurrentUser != null && SceneManager.GetActiveScene().name == "Login")
        {
            SceneManager.LoadScene("Menu");
        }
        else if (auth.CurrentUser == null && SceneManager.GetActiveScene().name != "Login")
        {
            SceneManager.LoadScene("Login");
        }
    }
    public void ButtonLogin()
    {
        auth.SignInWithEmailAndPasswordAsync(EmailField.text, PasswordField.text);
        if (SceneManager.GetActiveScene().name == "Login")
        {
            auth.SignOut();
            auth.StateChanged += Auth_StateChanged;
            Auth_StateChanged(this, null);
            auth.SignOut();
        }
    }

    public void ButtonRegister()
    {
        auth.CreateUserWithEmailAndPasswordAsync(EmailField.text, PasswordField.text);
        if (SceneManager.GetActiveScene().name == "Login")
        {
            auth.SignOut();
            auth.StateChanged += Auth_StateChanged;
            Auth_StateChanged(this, null);
            auth.SignOut();
        }
    }

    public void ButtonLeave()
    {
        auth.SignOut();
    }
    
}
