using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;
using UnityEngine.AI;

public class ChangePlayerPositionStage : Stage
{
    public GameObject Position;

    private GameObject player;

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().gameObject;
    }

    public override bool ConditionToFinish()
    {
        return !isFinished;
    }

    public override void InitStage()
    {
        var agent = player.GetComponent<NavMeshAgent>();
        agent.enabled = false;
        player.transform.position = Position.transform.position;

        agent.enabled = true;
        ExitStage();
    }

    public override void UpdateStage()
    {
    }
}
