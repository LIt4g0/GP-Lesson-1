using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Food : MonoBehaviour
{

    public FoodSpawner spawner;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        collision.GetComponent<Snake>().AddPart();
        //Debug.Log("COLLISIION");
        spawner.RemoveFood(this);
        Destroy(gameObject);
    }
}
