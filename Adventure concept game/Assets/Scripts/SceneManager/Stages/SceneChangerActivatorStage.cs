using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StageManager;
public class SceneChangerActivatorStage : Stage
{
    public SceneChangerCondition Condition;

    public override bool ConditionToFinish()
    {
        return Condition.ConditionSatisfied();
    }

    public override void InitStage()
    {
    }

    public override void UpdateStage()
    {
        if (ConditionToFinish())
            ExitStage();
    }
}
