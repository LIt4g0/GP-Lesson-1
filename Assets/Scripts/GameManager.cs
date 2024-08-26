using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Object.DontDestroyOnLoad test;

//[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{
    // private static GameManager m_Instance = null;
    // public static GameManager Instance
    // {
    //     get
    //     {
    //         if (m_Instance == null)
    //         {
    //             m_Instance = FindObjectOfType<GameManager>();
    //             // fallback, might not be necessary.
    //             if (m_Instance == null)
    //                 m_Instance = new GameObject(typeof(GameManager).Name).AddComponent<GameManager>();
    //             DontDestroyOnLoad(m_Instance);
    //         }
    //         return m_Instance;
    //     }
    // }

    [SerializeField] bool deadlyWalls;
    [SerializeField] Canvas menu;
    bool inMenu = true;
    Snake snake;


    void Awake()
    {
        //Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
         if (SceneManager.GetActiveScene().name == "Snake")
         {
            //ShowMenu(false);
         }
         else
         {
            ShowMenu(true);
         }
    }

    void Update()
    {
        
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
        Debug.Log("Add scores to list "+ scoreIn);
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
        //snake = FindAnyObjectByType<Snake>();
        //Debug.Log(snake);

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
