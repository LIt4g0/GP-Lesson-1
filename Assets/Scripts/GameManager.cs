using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;


public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [SerializeField] bool deadlyWalls = true;
    [SerializeField] Canvas menu;
    bool inMenu = true;
    Snake snake;
    [SerializeField] int totalScore;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] List<int> scores = new List<int>();

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this);
        }
        else if (manager != this)
        {
            //Debug.Log("DESTROYING EXTRA MANAGER: " + gameObject.name);
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

         AddScore(PlayerPrefs.GetInt("SavedHighScore1"));
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
        Debug.Log("Added score "+ scoreIn);
        totalScore += scoreIn;
        scores.Add(scoreIn);
        SortAndRefreshScores();
        SceneManager.LoadScene("Menu");
        ShowMenu(true);
    }

    private void SortAndRefreshScores()
    {
        string scoreString = "";
        int i = 0;
        scores.Sort();
        scores.Reverse();
        if (scores.Count >= 10)
        {
            scores.RemoveRange(10,scores.Count-10);
        }
        //scores.OrderByDescending(10.0f<=1.0f);
        foreach (var score in scores)
        {
            scoreString += i+1 + ". " + scores[i] + "\n";
            i ++;
            //if (i >= 10) break;
        }


        if (PlayerPrefs.HasKey("SavedHighScore1"))
        {
            if (PlayerPrefs.GetInt("SavedHighScore1") > scores[0])
            {
                
            }
            else
            {
                PlayerPrefs.SetInt("SavedHighScore1", scores[0]);
            }
        }
        else
        {
            PlayerPrefs.SetInt("SavedHighScore1", scores[0]);
        }


        //Debug.Log(PlayerPrefs.GetInt("SavedHighScore1"));
        scoreText.text = scoreString;
        // "2. " + scores[2] + "/n" +
        // "3. " + scores[3] + "/n" +
        // "4. " + scores[4] + "/n" +
        // "5. " + scores[5] + "/n" +
        // "6. " + scores[6] + "/n" +
        // "7. " + scores[7] + "/n" +
        // "8. " + scores[8] + "/n" +
        // "9. " + scores[9] + "/n" +
        // "10. " + scores[10] + "/n";
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

    public bool SetSnake(Snake snakeIn)
    {
        snake = snakeIn;
        return deadlyWalls;
    }
}
