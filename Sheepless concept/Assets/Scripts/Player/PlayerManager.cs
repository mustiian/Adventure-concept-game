using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    [HideInInspector]
    public static PlayerManager instance;

    public bool CutsceneState;

    [HideInInspector]
    public PlayerMovement movement;

    public PlayerAnimationManager animationManager;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance == this)
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        animationManager = GetComponentInChildren<PlayerAnimationManager>();
    }


    void LateUpdate()
    {
        if (!CutsceneState)
        {
            movement.Move();
            animationManager.MovementAnimation();
        }
    }
}
