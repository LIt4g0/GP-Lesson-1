using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Score
{
    public int value;
    public string name;
}

public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [Header("Refrences")]
    [SerializeField] Canvas menu;
    [SerializeField] Canvas input;
    [SerializeField] Button play;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI tempScoreText;

    //Local vars
    List<Score> scores = new();
    int tempScore = 0;
    const int MAXSCORES = 10;
    const int MAXSCORECHARACTERS = 3;
    bool inMenu = true;
    bool deadlyWalls = true;

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
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ShowMenu(false);
        }
        else
        {
            ShowMenu(true);
        }

        LoadScores();
    }

    void LoadScores()
    {
        for (int i = 0; i <= MAXSCORES; i++)
        {
            if (PlayerPrefs.HasKey("SavedHighScore" + (char)i))
            {
                Score loadScore = new()
                {
                    value = PlayerPrefs.GetInt("SavedHighScore" + (char)i),
                    name = PlayerPrefs.GetString("ScoreNames" + (char)i)
                };
                scores.Insert(i, loadScore);
            }
        }

        SortAndRefreshScores();
    }

    public void AddScore(int scoreIn)
    {
        tempScore = scoreIn;
        //Check if score is high enough to make the list
        if (scores.Count > MAXSCORES - 1 && tempScore <= scores[MAXSCORES - 1].value)
        {
            NameInput(false);
            SetScores();
        }
        else
        {
            NameInput(true);
        }

        SceneManager.LoadScene(0);
        tempScoreText.text = "Your Score: " + tempScore;
    }

    public void NameInputFinished()
    {
        if (inputField.text.EndsWith("\n"))
        {
            string inputText = inputField.text.Remove(inputField.text.Length - 1); ;
            inputText = Regex.Replace(inputText, @"[^a-zA-Z]", "");
            inputField.text = "";
            if (inputText.Length < MAXSCORECHARACTERS)
            {
                return;
            }
            NameInput(false);
            inputText = inputText[..MAXSCORECHARACTERS];
            inputText = inputText.ToUpper();
            SetScores(tempScore, inputText);
            tempScore = 0;
            input.enabled = false;
        }
    }

    void NameInput(bool inputOrNot)
    {
        if (inputOrNot)
        {
            input.enabled = true;
            inputField.Select();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    void SetScores(int scoreIn = 0, string nameIn = "XXX")
    {
        Score newScore = new()
        {
            value = scoreIn,
            name = nameIn
        };

        scores.Add(newScore);
        SortAndRefreshScores();
        ShowMenu(true);
    }


    void SortAndRefreshScores()
    {
        scores = scores.OrderBy(c => c.value).ToList();
        scores.Reverse();

        if (scores.Count >= MAXSCORES)
            scores.RemoveRange(MAXSCORES, scores.Count - MAXSCORES);

        string tempScoreText = "";
        for (int i = 0; i < scores.Count; i++)
        {
            tempScoreText += i + 1 + ". " + scores[i].value + " - " + scores[i].name + "\n";
            PlayerPrefs.SetInt("SavedHighScore" + (char)i, scores[i].value);
            PlayerPrefs.SetString("ScoreNames" + (char)i, scores[i].name);
        }

        scoreText.text = tempScoreText;
    }

    void ShowMenu(bool showIn)
    {
        inMenu = showIn;
        if (inMenu)
        {
            menu.enabled = true;
            play.Select();
        }
        else
        {
            menu.enabled = false;
            EventSystem.current.SetSelectedGameObject(null);
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
