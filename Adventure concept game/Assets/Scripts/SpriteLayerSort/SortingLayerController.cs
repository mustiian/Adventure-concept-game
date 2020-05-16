using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayerController : MonoBehaviour
{
    [SerializeField]
    private List<SpriteRenderer> spritesObjects = new List<SpriteRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        CheckAllObjects();
    }

    public void AddSprite(SpriteRenderer sprite)
    {
        spritesObjects.Add(sprite);
    }

    void CheckAllObjects()
    {
        StartCoroutine(CheckAllObjectsCoroutine());
    }

    IEnumerator CheckAllObjectsCoroutine()
    {
        for (int i = 0; i < spritesObjects.Count; i++)
        {
            spritesObjects[i].sortingOrder = (-1) * (int)spritesObjects[i].transform.position.z;
        }

        yield return new WaitForSeconds(0.01f);

        CheckAllObjects();
    }
}
