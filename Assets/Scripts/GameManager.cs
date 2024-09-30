using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class Score
{
    public int scoreValue;
    public string scoreName;
}

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    [Header("Refrences")]
    [SerializeField] Canvas menu;
    [SerializeField] Canvas input;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI tempScoreText;
    [SerializeField] bool deadlyWalls = true;
    [SerializeField] List<string> scoreNames = new List<string>();
    [SerializeField] List<int> scores = new List<int>();
    [SerializeField] List<Score> scoreClass = new List<Score>();

    //Local vars
    bool inputtingName = false;
    int tempScore = 0;
    bool inMenu = true;

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this);
        }
        else if (manager != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
         if (SceneManager.GetActiveScene().name == "Snake")
         {
            ShowMenu(false);
         }
         else
         {
            ShowMenu(true);
         }

         LoadScores();
    }

    void Update()
    {
        if (inputtingName)
        {
            input.enabled = true;
            inputField.Select();
        }
    }

    public void NameInputFinished()
    {
        if (inputField.text.EndsWith("\n"))
        {
            string text = inputField.text.Remove(inputField.text.Length - 1);;
            inputField.text = "";
            text = Regex.Replace(text, @"[^a-zA-Z]", "");
            if (text.Length < 3)
            {
                Debug.Log("Not enough letters");

                return;
            }
            inputtingName = false;
            text = text[..3];
            text = text.ToUpper();
            SetScores(tempScore,text);
            tempScore = 0;
            Debug.Log(text);
            input.enabled = false;
        }
    }

    private void SetScores(int scoreIn,string nameIn)
    {
        Debug.Log("Name and score should be set");
        Score newScore = new()
        {
            scoreValue = scoreIn,
            scoreName = nameIn
        };

        scoreClass.Add(newScore);
        Debug.Log("Added player "+ newScore.scoreName + " With score of: "+ newScore.scoreValue);
        scores.Add(scoreIn);
        SortAndRefreshScores();

        ShowMenu(true);
    }

    private void LoadScores()
    {
        for (int i = 0; i <= 10; i++)
        {
            if (PlayerPrefs.HasKey("SavedHighScore"+(char)i))
            {
                Score loadScore = new Score();
                loadScore.scoreValue = PlayerPrefs.GetInt("SavedHighScore"+(char)i);
                loadScore.scoreName = PlayerPrefs.GetString("ScoreNames"+(char)i);
                scoreClass.Insert(i,loadScore);
            }
        }

        SortAndRefreshScores();
    }

    void SortAndRefreshScores()
    {
        string scoreString = "";
        int i = 0;
        scoreClass = scoreClass.OrderBy(c => c.scoreValue).ToList();
        scoreClass.Reverse();

        if (scoreClass.Count >= 10)
        {
            scoreClass.RemoveRange(10,scoreClass.Count-10);
        }
        
        //Save score to playerprefs and set list;
        foreach (var score in scoreClass)
        {
            scoreString += i+1 + ". " + scoreClass[i].scoreValue + " - " + scoreClass[i].scoreName + "\n";
            PlayerPrefs.SetInt("SavedHighScore"+(char)i,scoreClass[i].scoreValue);
            PlayerPrefs.SetString("ScoreNames"+(char)i,scoreClass[i].scoreName);
            i ++;
        }

        scoreText.text = scoreString;
    }

    void ShowMenu(bool showIn)
    {
        inMenu = showIn;
        if (inMenu)
        {
            menu.enabled = true;
        }
        else
        {
            menu.enabled = false;
        }
    }

    public void AddScore(int scoreIn)
    {
        SceneManager.LoadScene("Menu");
        tempScore = scoreIn;
        tempScoreText.text = "Your Score: " + tempScore;
        
        if (scoreClass.Count > 9)
        {
            if (tempScore <= scoreClass[9].scoreValue)
            {
                inputtingName = false;
                SetScores(0,"XXX");
            }
            else
            {
                inputtingName = true;
            }
        }
        else
        {
            inputtingName = true;
        }
    }

    public void SetWalls()
    {
        deadlyWalls = !deadlyWalls;
    }

    public void Play()
    {
        ShowMenu(false);
        SceneManager.LoadScene("Snake");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
