using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakePart : MonoBehaviour
{
    public Snake snake;
    public int attachAt = 0;
    public bool snakeTail = false;
    [SerializeField] bool attached = false;

    void Start()
    {
        if (!attached)
        {
            transform.localScale = new Vector3(1.1f,1.1f,1.1f);
        }
    }

    void Update()
    {
        if (attachAt <= 0 && !attached)
        {

        }
    }

    public void MovePart(Vector3 posIn, float rotation)
    {
        transform.position = posIn;
        transform.localRotation = Quaternion.Euler(0,rotation,0);
    }

    public void AttachCount()
    {
        attachAt -= 1;
        if (attachAt <= 0)
        {
            transform.localScale = new Vector3(0.85f,0.85f,0.85f);
            attached = true;
            snake.AttachPart(this);
        }
    }

    public void SetStartStuff(int attachIn, Snake snakeIn)
    {
        snake = snakeIn;
        attachAt = attachIn;
    }
}
