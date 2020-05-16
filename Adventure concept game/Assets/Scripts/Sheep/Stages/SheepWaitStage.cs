using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;

public class SheepWaitStage : Stage
{
    public float WaitTime;
    public Animator animator;

    private bool finishStage = false;

    public override bool ConditionToFinish()
    {
        return finishStage;
    }

    public override void InitStage()
    {
        finishStage = false;
        animator.SetTrigger("Idle");

        StartCoroutine(EndStageCoroutine(WaitTime));
    }

    public override void UpdateStage()
    {
        if (ConditionToFinish())
            ExitStage();
    }

    private IEnumerator EndStageCoroutine(float time)
    {
        yield return new WaitForSeconds(time);

        finishStage = true;
    }
}
