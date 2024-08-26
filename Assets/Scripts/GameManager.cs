using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Object.DontDestroyOnLoad test;

[DisallowMultipleComponent]
public class GameManager : MonoBehaviour
{

    [SerializeField] bool deadlyWalls = true;
    [SerializeField] Canvas menu;
    bool inMenu = true;

    private static GameManager _instance;
    public static GameManager Instance {
        get { return _instance; }
        set {
            if (_instance == null) {
                _instance = value;
            } else {
                Debug.Log("Destroying extra GameManager" + value.gameObject);
                Destroy(value.gameObject);
            }
        }
    }

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
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

    }
    public void Quit()
    {
        Application.Quit();
        
    }
}
