using Firebase.Auth;
using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class StartGame : MonoBehaviour
{
    private FirebaseAuth auth;
    private DatabaseReference dbRef;
    public UnityEngine.UI.Text textLeaders;
    public GameObject MainPanel;
    public GameObject RatingPanel;
    public UnityEngine.UI.Text maxPoint;
    public UnityEngine.UI.Text lastPoint;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        StartCoroutine(MaxPoint());
        StartCoroutine(LastPoint());

    }

    
    private IEnumerator MaxPoint()
    {
        string email = auth.CurrentUser.Email.Replace(".", "");
        DatabaseReference scoreRef = dbRef.Child("LeaderBoard").Child(email).Child("Score");

        Task<DataSnapshot> task = scoreRef.GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
        }
        else if (task.Result.Value == null)
        {
            Debug.Log("Значение 'Score' не найдено в базе данных");
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            Debug.Log("Значение 'Score' для пользователя " + email + ": " + snapshot.Value.ToString());
            maxPoint.text ="max value: " + snapshot.Value.ToString();
        }
    }
    private IEnumerator LastPoint()
    {
        string email = auth.CurrentUser.Email.Replace(".", "");
        DatabaseReference scoreRef = dbRef.Child("LeaderBoard").Child(email).Child("Score1");

        Task<DataSnapshot> task = scoreRef.GetValueAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception != null)
        {
            Debug.LogError(task.Exception);
        }
        else if (task.Result.Value == null)
        {
            Debug.Log("Значение 'Score' не найдено в базе данных");
        }
        else
        {
            DataSnapshot snapshot = task.Result;
            Debug.Log("Значение 'Score' для пользователя " + email + ": " + snapshot.Value.ToString());
            lastPoint.text ="last value: " + snapshot.Value.ToString();
        }
    }
    public void OpenLeaderBoard()
    {
        MainPanel.SetActive(false);
        RatingPanel.SetActive(true);
        StartCoroutine(GetLeaders());
    }

    
    public IEnumerator GetLeaders()
    {
        var leadersTask = dbRef.Child("LeaderBoard").OrderByChild("Score").GetValueAsync();
        yield return new WaitUntil(() => leadersTask.IsCompleted);

        if (leadersTask.Exception != null)
        {
            Debug.LogError($"Error loading leaderboard: {leadersTask.Exception}");
            yield break;
        }

        DataSnapshot snapshot = leadersTask.Result;

        int number = 1;
        foreach (DataSnapshot dataChildSnapshot in snapshot.Children.Reverse())
        {
            string email = dataChildSnapshot.Child("Email").Value.ToString();
            string score = dataChildSnapshot.Child("Score").Value.ToString();

            textLeaders.text += $"\n{number}.  {score}";
            number++;
        }
    }
    public void BackButton()
    {
        RatingPanel.SetActive(false);
        MainPanel.SetActive(true);
        textLeaders.text = "";
    }

    public void GoOut()
    {
        SceneManager.LoadScene("Login");
    }

    public void Game()
    {
        SceneManager.LoadScene("Game");
    }
}
