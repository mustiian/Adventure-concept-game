using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;

public class ChangeObjectPositionStage : Stage
{
    public GameObject Object;

    public GameObject Position;

    public override bool ConditionToFinish()
    {
        return !isFinished;
    }

    public override void InitStage()
    {
        Object.transform.position = Position.transform.position;
        ExitStage();
    }

    public override void UpdateStage()
    {
    }
}
