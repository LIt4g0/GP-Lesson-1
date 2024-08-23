//using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] Food food;
    [SerializeField] List<Food> foods = new List<Food>();
    [SerializeField] float spawnTimer = 1.0f;
    [SerializeField] float width = 10.0f;
    [SerializeField] float heigth = 10.0f;
    [SerializeField] int maxFood = 3;
    
    float timeToSpawn = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        SpawnFood();
    }

    // Update is called once per frame
    void Update()
    {

        timeToSpawn -= Time.deltaTime;
        if (foods.Count <= maxFood && timeToSpawn <= 0)
        {
            timeToSpawn = spawnTimer;
            SpawnFood();
        }
    }

    private void SpawnFood()
    {
        //int iWidth = Random.Range(-width,width);
        Vector3 position = new Vector3((int)Random.Range(-width,width),0,(int)Random.Range(-heigth,heigth));
        var foody = Instantiate(food,position, transform.rotation);
        foody.spawner = this;
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

    public void RemoveFood(Food foodIn)
    {
        foods.Remove(foodIn);
    }
}
