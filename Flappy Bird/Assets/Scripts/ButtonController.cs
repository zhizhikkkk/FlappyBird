using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;

public class ButtonController : MonoBehaviour
{
    [SerializeField] Text textScore;
    private int score =0;

    private DatabaseReference dbRef;
    private FirebaseAuth auth;
    void Start()
    {
        score = PlayerPrefs.GetInt("score");
        UpdateScoreText();
        
    }

    private void UpdateScoreText()
    {
        textScore.text = score.ToString();
    }

    private void BackButton()
    {
        SceneManager.LoadScene("Menu");
    }

    private void OpenLeaderBoard()
    {
        SceneManager.LoadScene("Rating");
    }

    
}
