using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layering2 : MonoBehaviour
{
     private SpriteRenderer sprite;
    public int sortingOrder = 0;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = sortingOrder;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            sprite.sortingOrder = 10;
        }

         else if (Input.GetKeyDown(KeyCode.E))
        {
            sprite.sortingOrder = 30;
        }
    }
}
