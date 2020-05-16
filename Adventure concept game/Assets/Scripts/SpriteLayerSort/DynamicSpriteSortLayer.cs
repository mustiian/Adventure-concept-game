using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpriteSortLayer : MonoBehaviour
{
    void Awake()
    {
        var sortingLayerController = GameObject.FindObjectOfType<SortingLayerController>();

        if (sortingLayerController)
            sortingLayerController.AddSprite(this.GetComponent<SpriteRenderer>());
    }
}
