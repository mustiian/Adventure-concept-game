using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject BubblePrefab;

    private float rotation = 45;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Spawn (ref Transform position, string text, float delay, bool isDynamic)
    {
        position.rotation = Quaternion.Euler (rotation, 0, 0);

        GameObject bubbleInstance = (GameObject)Instantiate (BubblePrefab, position.transform.position, position.transform.rotation);

        TextMeshProUGUI UItext = bubbleInstance.GetComponentInChildren<TextMeshProUGUI> ();
        Bubble bubble = bubbleInstance.GetComponentInChildren<Bubble> ();

        bubble.Delay = delay;
        bubble.isDynamic = isDynamic;
        bubble.SetPosition (ref position);
        UItext.SetText (text);
    }

}
