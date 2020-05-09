using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoBehaviour
{
    public float Delay;

    public bool isDynamic;

    public Transform Position;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator> ();
        animator.Play ("Appear");
        StartCoroutine (WaitForDestroy (Delay));
    }

    private void Update()
    {
        if (isDynamic)
            transform.position = Position.position;
    }

    public void SetPosition(ref Transform position )
    {
        Position = position;
    }

    private IEnumerator WaitForDestroy(float delay)
    {
        yield return new WaitForSeconds (delay);
        animator.Play ("Disappear");
        yield return new WaitForSeconds (animator.GetCurrentAnimatorStateInfo (0).length);
        Destroy (gameObject);
    }
}
