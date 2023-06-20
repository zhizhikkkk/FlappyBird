using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Firebase.Database;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine.UI;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class db : MonoBehaviour
{
    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    public Text text;

    public InputField email;
    public InputField password;
    public Text textInfo;

    private ButtonController bc;
    private int playerID;
    public Text winnerText;
    void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;

        bc = GetComponent<ButtonController>();

        if (SceneManager.GetActiveScene().name == "Login")
        {
            auth.SignOut();
            auth.StateChanged += Auth_StateChanged;
            Auth_StateChanged(this, null);
            auth.SignOut();
        }
        
    }

    private void Auth_StateChanged(object sender,System.EventArgs e)
    {
        if (auth.CurrentUser != null)
        {
            textInfo.text = "You are logged in as: " + auth.CurrentUser.Email + "! Press log in..." ;
            SceneManager.LoadScene("Menu");
        }
        else
        {
            textInfo.text = "Try again to enter your email and pass correctly";
        }
    }

    //public void SaveData(string str)
    //{

    //    User user = new User(str, 15, "offline");
    //    string json = JsonUtility.ToJson(user);
    //    dbRef.Child("users").Child(str).SetRawJsonValueAsync(json);
    //    //dbRef.Child("users").Child("name").SetValueAsync(str);
    //}

    //public IEnumerator LoadData(string str)
    //{
    //    var user = dbRef.Child("users").Child(str).GetValueAsync();
    //    yield return new WaitUntil(predicate: () => user.IsCompleted);
    //    if (user.Exception != null)
    //    {
    //        Debug.LogError(user.Exception);
    //    }
    //    else if (user.Result == null)
    //    {
    //        Debug.Log("Null");
    //    }
    //    else
    //    {
    //        DataSnapshot snapshot = user.Result;
    //        Debug.Log(snapshot.Child("age").Value.ToString() + snapshot.Child("name").Value.ToString());
    //        text.text = snapshot.Child("age").Value.ToString();
    //    }
    //}

    //public IEnumerator  GetAllUsers(string str)
    //{
    //    // dbRef.Child("users").Child(str).RemoveValueAsync();
    //    var user = dbRef.Child("users").OrderByChild("age").LimitToLast(3).GetValueAsync();
    //    yield return new WaitUntil(predicate: () => user.IsCompleted);
    //    if (user.Exception != null)
    //    {
    //        Debug.LogError(user.Exception);
    //    }
    //    else if (user.Result == null)
    //    {
    //        Debug.Log("Null");
    //    }
    //    else
    //    {
    //        DataSnapshot snapshot = user.Result;
    //        foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse())
    //        {
    //            Debug.Log(childSnapshot.Child("age").Value.ToString());
    //        }
    //    }
    //}

    public void ButtonLogin()
    {
        auth.SignInWithEmailAndPasswordAsync(email.text, password.text);
    }

    public void ButtonRegister()
    {
        auth.CreateUserWithEmailAndPasswordAsync(email.text, password.text);
    }
}

public class User
{
    public string name;
    public int age;
    public string status;

    public User(string name, int age, string status)
    {
        this.name = name;
        this.age = age;
        this.status = status;
    }
}
