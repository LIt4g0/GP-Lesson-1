using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    [SerializeField] bool deadlyWalls = true;
    [SerializeField] Canvas menu;
    bool inMenu = true;
    Snake snake;
    [SerializeField] float totalScore;

    void Awake()
    {
        if (manager == null)
        {
            manager = this;
            DontDestroyOnLoad(this);
        }
        else if (manager != this)
        {
            Debug.Log("DESTROYING EXTRA MANAGER: " + gameObject.name);
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
        Debug.Log("Added, total is: "+ totalScore);
        totalScore += scoreIn;
        SceneManager.LoadScene("Menu");
        ShowMenu(true);
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
