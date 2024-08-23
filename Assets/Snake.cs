using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Snake : MonoBehaviour
{
    [SerializeField] float moveTime = 0.5f;
    [SerializeField] float stepDist = 1.0f;
    [SerializeField] List<Vector3> moves = new List<Vector3>();
    [SerializeField] List<float> rotations = new List<float>();
    [SerializeField] List<SnakePart> parts = new List<SnakePart>();
    [SerializeField] List<SnakePart> looseParts = new List<SnakePart>();
    [SerializeField] int moveDir = 0;
    [SerializeField] int prevMoveDir = 0;
    [SerializeField] SnakePart snakePart;
    float rotation = 0.0f;
    float timeToMove = 0.0f;
    [SerializeField] int startParts = 1;

    void Start()
    {
        moves.Add(transform.position);
        rotations.Add(0);
        moveDir = 4;
        prevMoveDir = moveDir;
    }

    void Update()
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


        timeToMove += Time.deltaTime;
        if (timeToMove >= moveTime)
        {
            Vector3 moveVector = new Vector3(0,0,0);
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
            foreach (SnakePart loose in looseParts)
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
                attached.MovePart(moves[i],rotations[i]);
                i += 1;
            }

            moves.Insert(0, moveVector + moves[0]);
            rotations.Insert(0, rotation);
            transform.position = moves[0];
            transform.localRotation = Quaternion.Euler(0,rotations[0],0);
            
            //Move parts

            timeToMove = 0.0f;
            prevMoveDir = moveDir;
            //moveDir = 0;
        }
        //Debug.Log(moves[0]);
    }

    private int TryMoveDir(int moveIn)
    {
        if (Mathf.Abs(moveIn - prevMoveDir)  >= 2)
        {
            Debug.Log("Move Allowed");
            return moveIn;
        }
        else
        {
            Debug.Log("Tried to turn directly" );
            return prevMoveDir;
        }    
    }

    public void AddPart()
    {
        Invoke("SpawnPart", moveTime);
        transform.localScale = new Vector3(1.35f,1.35f,1.35f);
    }

    public void AttachPart(SnakePart partIn)
    {
        Debug.Log("Attaching Part");
        parts.Insert(0,partIn);
        looseParts.Remove(partIn);
    }

    void SpawnPart()
    {
        Debug.Log("Adding Part");
        SnakePart tempPart = Instantiate(snakePart,moves[1],transform.rotation);
        looseParts.Add(tempPart);
        tempPart.SetStartStuff(startParts, this);
        startParts += 1;
        transform.localScale = new Vector3(1f,1f,1f);
        //tempPart.snake = this;
    }

    void OnTriggerEnter(Collider collision)
    {
        if (!collision.CompareTag("Food"))
        {
            Debug.Log("DIE");
            SceneManager.LoadScene("Menu");
        }
    }
}
