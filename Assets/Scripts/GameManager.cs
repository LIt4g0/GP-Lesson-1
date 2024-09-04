using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using OpenCover.Framework.Model;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System.Text.RegularExpressions;


public class Score
    {
        public int scoreValue;
        public string scoreName;
    }

public class GameManager : MonoBehaviour
{
    public static GameManager manager;
    
    [SerializeField] bool deadlyWalls = true;
    [SerializeField] Canvas menu;
    
    [SerializeField] Canvas input;
    [SerializeField] TMP_InputField inputField;
    bool inMenu = true;
    Snake snake;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI tempScoreText;
    [SerializeField] List<string> scoreNames = new List<string>();
    [SerializeField] List<int> scores = new List<int>();
    [SerializeField] List<Score> scoreClass = new List<Score>();
    bool inputtingName = false;
    int tempScore = 0;

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
        //Clear scores
        // if (Input.GetKeyDown(KeyCode.C))
        // {

        //     scoreClass.Clear();
        //     PlayerPrefs.DeleteAll();
        // }

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
    private void SortAndRefreshScores()
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
        
        //inputtingName = true;
        if (scoreClass.Count > 9)
        {
            if (tempScore <= scoreClass[9].scoreValue)
            {
                inputtingName = false;
                SetScores(0,"XXX");
                //NameInputFinished();
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
        //NameInputFinished();

    }

    // private void InputScoreName(int scoreIn)
    // {
    //     SceneManager.LoadScene("InputNameScene");
    // }


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

    public bool SetSnake(Snake snakeIn)
    {
        snake = snakeIn;
        return deadlyWalls;
    }


}
