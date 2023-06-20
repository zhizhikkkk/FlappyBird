using System;
using System.Collections;
using Firebase.Auth;
using Firebase.Database;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;

public class GameManager : MonoBehaviour
{
    public Player player;
    public Text scoreText;
    public Text highScoreText;
    public GameObject playButton;
    public GameObject gameOver;
    public Image image1;
    public Image image2;
    public Image image3;
    [NonSerialized] public DatabaseReference dbRef;
    [NonSerialized] public FirebaseAuth auth;
    private int score;
    private int highScore;
    private Vector3 startPos;

    [SerializeField] private Text textLeaders;

    private void Awake()
    {
        dbRef = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        Application.targetFrameRate = 60;
        startPos = player.transform.position;
        Pause();
    }

    private void Update()
    {
        if (score == 9 && image1 != null)
        {
            Destroy(image1.gameObject);
            image1 = null;
        }
        else if (score == 5 && image2 != null)
        {
            Destroy(image2.gameObject);
            image2 = null;
        }
        else if (score == 2 && image3 != null)
        {
            Destroy(image3.gameObject);
            image3 = null;
        }
    }
    
    public void Play()
    {
        score = 0;

        StartCoroutine(MaxPoint());
        scoreText.text = score.ToString();
        playButton.SetActive(false);
        gameOver.SetActive(false);
        Time.timeScale = 1;
        player.enabled = true;

        Pipes[] pipes = FindObjectsOfType<Pipes>();
        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }
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
            highScore = Int32.Parse(snapshot.Value.ToString());
        }
        Debug.Log(highScore);
    }
    public void Pause()
    {
        player.transform.position = startPos;
        Time.timeScale = 0;
        player.enabled = false;

    }
    public void GameOver()
    {
        dbRef.Child("LeaderBoard").Child(auth.CurrentUser.Email.Replace(".", "")).Child("Score")
            .SetValueAsync(highScore);
        dbRef.Child("LeaderBoard").Child(auth.CurrentUser.Email.Replace(".", "")).Child("Score1")
            .SetValueAsync(score);
        dbRef.Child("LeaderBoard").Child(auth.CurrentUser.Email.Replace(".", "")).Child("Email")
            .SetValueAsync(auth.CurrentUser.Email);
        gameOver.SetActive(true);
        playButton.SetActive(true);
        Pause();
    }
    public void IncreaseScore()
    {
        score++;
        if (score > highScore) highScore = score;
        PlayerPrefs.SetInt("highScore", highScore);

        scoreText.text = score.ToString();
        highScoreText.text = "Max Score : " + highScore.ToString();
    }

    
}
