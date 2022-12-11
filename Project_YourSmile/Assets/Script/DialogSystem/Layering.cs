using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Layering : MonoBehaviour
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            sprite.sortingOrder = 7;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            sprite.sortingOrder = 21;
        }
    }
}
