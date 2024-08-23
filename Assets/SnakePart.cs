using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    Vector3 initialPos;
    public Snake snake;
    public int attachAt = 0;
    int initialAttachPos = 0;
    public bool snakeTail = false;
    [SerializeField] bool attached = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!attached)
        {
            transform.localScale = new Vector3(1.1f,1.1f,1.1f);
        }
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        if (attachAt <= 0 && !attached)
        {
            transform.localScale = new Vector3(0.85f,0.85f,0.85f);
            attached = true;
            snake.AttachPart(this);
        }
        Debug.Log(attachAt);
    }

    public void MovePart(Vector3 posIn, float rotation)
    {
        transform.position = posIn;
        transform.localRotation = Quaternion.Euler(0,rotation,0);
        //reset scale;
    }

    public void AttachCount()
    {
        
        attachAt -= 1;
    }

    public void SetStartStuff(int attachIn, Snake snakeIn)
    {
        snake = snakeIn;
        attachAt = attachIn;
        initialAttachPos = attachAt;
    }
}
