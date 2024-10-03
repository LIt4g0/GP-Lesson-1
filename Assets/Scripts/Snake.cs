using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    [Header("References")]
    [SerializeField] SnakePart snakePart;
    [SerializeField] SnakePart snakeTail;
    TextMeshProUGUI scoreText;
    [Header("Movement")]
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] float stepDist = 1.0f;
    [SerializeField] float speedMultiplier = 2.0f;
    [Header("Map size")]
    public float mapSizeX = 16;
    public float mapSizeZ = 8;

    //Local vars
    Vector3 moveVector = new(0, 0, 0);
    const int STARTPARTS = 1;
    int score = 0;
    int moveDir = 0;
    int prevMoveDir = 0;
    float rotation = 0.0f;
    float timeToMove = 0.0f;
    bool deadlyWalls = true;
    bool dirChanged = false;

    List<Vector3> moves = new();
    List<float> rotations = new();
    List<SnakePart> attachedParts = new();
    List<SnakePart> unattachedParts = new();
    List<SnakePart> recentlyAttachedParts = new();

    float moveHoriInputSys;
    float moveVertInputSys;
    bool testInput = false;
    PlayerControls playerControls;

    void Awake()
    {
        playerControls = new();
    }

    void OnEnable()
    {
        playerControls.Enable();
    }

    void OnDisable()
    {
        playerControls.Disable();
    }

    void Start()
    {
        GetComponent<PlayerInput>();
        moves.Add(transform.position);
        rotations.Add(0);
        moveDir = 4;
        prevMoveDir = moveDir;
        dirChanged = true;
        attachedParts.Add(snakeTail);
        deadlyWalls = GameManager.manager.deadlyWalls;
        scoreText = GameManager.manager.scoreCanvas.GetComponentInChildren<TextMeshProUGUI>();
        scoreText.text = ""+score.ToString("0000");
    }

    void Update()
    {
        Debug.Log(playerControls.Snake.Move.ReadValue<Vector2>());

        SetMoveDir();

        MoveTimer();

        Move();
    }

    private void Move()
    {
        if (timeToMove < 0)
        {

            foreach (SnakePart recentlyAttached in recentlyAttachedParts)
            {
                unattachedParts.Remove(recentlyAttached);
            }

            foreach (SnakePart unattached in unattachedParts)
            {
                unattached.AttachCount();
            }

            int i = 0;
            foreach (SnakePart attached in attachedParts)
            {
                if (moves[i] == null)
                {
                    break;
                }
                attached.MovePart(moves[i], rotations[i]);
                i += 1;
            }

            Vector3 tempMove = moveVector + moves[0];

            if ((tempMove.x > mapSizeX || tempMove.x < -mapSizeX) && !deadlyWalls)
            {
                tempMove = moves[0];
                tempMove.x *= -1;
            }

            if ((tempMove.z > mapSizeZ || tempMove.z < -mapSizeZ) && !deadlyWalls)
            {
                tempMove = moves[0];
                tempMove.z *= -1;
            }

            moves.Insert(0, tempMove);
            rotations.Insert(0, rotation);
            transform.position = moves[0];
            transform.localRotation = Quaternion.Euler(0, rotations[0], 0);

            timeToMove = moveTime;
            prevMoveDir = moveDir;
            dirChanged = false;

            int partsTotal = unattachedParts.Count + attachedParts.Count;

            if (moves.Count - partsTotal >= 15)
            {
                moves.RemoveAt(moves.Count - 1);
                rotations.RemoveAt(rotations.Count - 1);
            }
        }
    }

    private void MoveTimer()
    {
        if (Input.GetButton("Jump"))
        {
            timeToMove -= Time.deltaTime * speedMultiplier;
        }
        else
        {
            timeToMove -= Time.deltaTime;
        }
    }

    private void SetMoveDir()
    {
        if (Input.GetAxisRaw("Vertical") > 0)
        {
            moveDir = TryMoveDir(1);
        }
        else if (Input.GetAxisRaw("Vertical") < 0)
        {
            moveDir = TryMoveDir(2);
        }
        else if (Input.GetAxisRaw("Horizontal") > 0)
        {
            moveDir = TryMoveDir(4);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            moveDir = TryMoveDir(5);
        }

        if (!dirChanged)
        {
            moveDir = prevMoveDir;
            return;
        }

        moveVector = Vector3.zero;
        switch (moveDir)
        {
            case 0:
                break;
            case 1:
                rotation = -90;
                moveVector.z = stepDist;
                break;

            case 2:
                rotation = 90;
                moveVector.z = -stepDist;
                break;

            case 4:
                rotation = 0;
                moveVector.x = stepDist;
                break;

            case 5:
                rotation = 180;
                moveVector.x = -stepDist;
                break;

            default:
                break;
        }
        transform.localRotation = Quaternion.Euler(0, rotation, 0);
    }

    private int TryMoveDir(int moveIn)
    {
        if (Mathf.Abs(moveIn - prevMoveDir)  >= 2)
        {
            dirChanged = true;
            return moveIn;
        }
        else
        {
            return prevMoveDir;
        }    
    }

    public void AddPart()
    {
        AudioController.instance.Play("eat");
        Invoke(nameof(SpawnPart), moveTime);
        transform.localScale = new Vector3(1.35f,1.35f,1.35f);
        score += 1;
        scoreText.text = ""+score.ToString("0000");
    }

    public void AttachPart(SnakePart partIn)
    {
        attachedParts.Insert(0,partIn);
        recentlyAttachedParts.Add(partIn);
    }

    void SpawnPart()
    {
        transform.localScale = new Vector3(1,1,1);

        SnakePart tempPart = Instantiate(snakePart,moves[1],transform.rotation);
        unattachedParts.Add(tempPart);
        if (unattachedParts.Count > 0)
            tempPart.SetStartStuff(attachedParts.Count+unattachedParts.Count-1, this);
        else
            tempPart.SetStartStuff(attachedParts.Count-1, this);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Food"))
        {
            GameManager.manager.AddScore(score);
        }
    }

    public Transform GetSnakePos()
    {
        return transform;
    }
    
}
