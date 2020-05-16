using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticSpriteSortLayer : MonoBehaviour
{
    void Start()
    {
        var sprite = GetComponent<SpriteRenderer>();
        sprite.sortingOrder = (-1) * (int)transform.position.z;
    }
}
