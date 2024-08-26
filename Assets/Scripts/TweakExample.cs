using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakExample : MonoBehaviour
{
    // Start is called before the first frame update
    public float x;
    public float y;
    public float z;
    public float diameter = 0.2f;
    public float speed = 0.05f;
    public float rotationRate = 0.05f;
    public float scaleSpeed = 0.1f;
    void Start()
    {
        x = transform.position.x;
        y = transform.position.y;
        z = transform.position.z;
        //rotation = 0;//transform.localRotation.y;
    }
    // Update is called once per frame
    void Update()
    {
        //x += 0.005f;
        // transform.position = new Vector3(x,y);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            diameter += scaleSpeed;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            diameter -= scaleSpeed;
        }
        diameter = Clamp(0.1f, 5.0f, diameter);
        transform.localScale = new Vector3(diameter, diameter, diameter);
        float rotation = 0;
        if (Input.GetKey(KeyCode.Q))
        {
            rotation -= 0.05f;
        }

        if (Input.GetKey(KeyCode.E))
        {
            rotation += 0.05f;
        }
        rotation *= rotationRate;

        x = Clamp(-50.0f, 50.0f, x);
        z = Clamp(-50.0f, 50.0f, z);

        //transform.position = new Vector3(x,y,z);
        //transform.forward = new Vector3(x,y,z);
        // transform.rotation.y = rotation;
        transform.Rotate(0,rotation,0);
        Transform tempPos = transform;
        if (Input.GetKey(KeyCode.W))
        {
            tempPos.position += transform.forward * Time.deltaTime * speed*diameter;
        }

        if (Input.GetKey(KeyCode.S))
        {
            tempPos.position -= transform.forward * Time.deltaTime * speed*diameter;
        }

        if (Input.GetKey(KeyCode.D))
        {
            tempPos.position += transform.right * Time.deltaTime * speed*diameter;
        }

        if (Input.GetKey(KeyCode.A))
        {
            tempPos.position -= transform.right * Time.deltaTime * speed*diameter;
        }

        transform.position = tempPos.position;

    }


    public float Clamp(float min, float max, float value)
    {
        if (value < min) {
            value = min;
        }
        else if (value > max) {
            value = max;
        }
        return value;
    }
}
