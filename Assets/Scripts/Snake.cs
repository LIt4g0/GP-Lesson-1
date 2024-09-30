using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] float stepDist = 1.0f;
    [SerializeField] List<Vector3> moves = new List<Vector3>();
    [SerializeField] List<float> rotations = new List<float>();
    [SerializeField] List<SnakePart> parts = new List<SnakePart>();
    [SerializeField] List<SnakePart> unattachedParts = new List<SnakePart>();
    [SerializeField] int moveDir = 0;
    [SerializeField] int prevMoveDir = 0;
    [SerializeField] SnakePart snakePart;
    float rotation = 0.0f;
    float timeToMove = 0.0f;
    [SerializeField] int startParts = 1;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] float speedMultiplier = 2.0f;
    public float mapSizeX = 16;
    public float mapSizeZ = 8;
    public int score = 0;
    bool deadlyWalls = true;

    void Start()
    {
        moves.Add(transform.position);
        rotations.Add(0);
        moveDir = 4;
        prevMoveDir = moveDir;
    }

    void Update()
    {
        InputToMoveDir();

        SpeedBoost();

        if (timeToMove >= moveTime)
        {
            Vector3 moveVector = new Vector3(0, 0, 0);
            switch (moveDir)
            {
                case 0:
                    moveDir = prevMoveDir;
                    break;

                case 1:
                    moveVector.z = stepDist;
                    rotation = -90;
                    break;

                case 2:
                    moveVector.z = -stepDist;
                    rotation = 90;
                    break;

                case 4:
                    moveVector.x = stepDist;
                    rotation = 0;
                    break;

                case 5:
                    moveVector.x = -stepDist;
                    rotation = 180;
                    break;

                default:
                    break;
            }
            int j = 0;
            foreach (SnakePart loose in unattachedParts)
            {
                loose.AttachCount();
                j += 1;
            }

            int i = 0;
            foreach (SnakePart attached in parts)
            {
                if (moves[i] == null)
                {
                    break;
                }
                attached.MovePart(moves[i], rotations[i]);
                i += 1;
            }
            float sizeX = mapSizeX;
            float sizeZ = mapSizeZ;
            Vector3 tempMove = moveVector + moves[0];

            if ((tempMove.x > sizeX || tempMove.x < -sizeX) && !deadlyWalls)
            {
                tempMove = moves[0];
                tempMove.x *= -1;
            }

            if ((tempMove.z > sizeZ || tempMove.z < -sizeZ) && !deadlyWalls)
            {
                tempMove = moves[0];
                tempMove.z *= -1;
            }

            moves.Insert(0, tempMove);
            rotations.Insert(0, rotation);
            transform.position = moves[0];
            transform.localRotation = Quaternion.Euler(0, rotations[0], 0);

            timeToMove = 0.0f;
            prevMoveDir = moveDir;

            int partsTotal = unattachedParts.Count + parts.Count;
            
            if (moves.Count - partsTotal >= 10)
            {
                moves.RemoveAt(moves.Count - 1);
                rotations.RemoveAt(rotations.Count - 1);
            }
        }

    }

    private void SpeedBoost()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            timeToMove += Time.deltaTime * speedMultiplier;
        }
        else
        {
            timeToMove += Time.deltaTime;
        }
    }

    private void InputToMoveDir()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDir = TryMoveDir(1);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDir = TryMoveDir(2);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDir = TryMoveDir(4);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDir = TryMoveDir(5);
        }
    }

    private int TryMoveDir(int moveIn)
    {
        if (Mathf.Abs(moveIn - prevMoveDir)  >= 2)
        {
            return moveIn;
        }
        else
        {
            return prevMoveDir;
        }    
    }

    public void AddPart()
    {
        Invoke(nameof(SpawnPart), moveTime);
        transform.localScale = new Vector3(1.35f,1.35f,1.35f);
        score += 1;
        text.text = ""+score.ToString("0000");
    }

    public void AttachPart(SnakePart partIn)
    {
        parts.Insert(0,partIn);
        unattachedParts.Remove(partIn);
    }

    void SpawnPart()
    {
        SnakePart tempPart = Instantiate(snakePart,moves[1],transform.rotation);
        unattachedParts.Add(tempPart);
        tempPart.SetStartStuff(startParts, this);
        startParts += 1;
        transform.localScale = new Vector3(1f,1f,1f);
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
