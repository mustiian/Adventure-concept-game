using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;

public class WaitStage : Stage
{
    public float Time;

    public override bool ConditionToFinish()
    {
        return !isFinished;
    }

    public override void InitStage()
    {
        StartCoroutine(WaitCoroutine(Time));
    }

    public override void UpdateStage()
    {
    }

    private IEnumerator WaitCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        ExitStage();
    }
}