using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangerDrivenCondition : SceneChangerCondition
{
    private bool activated = false;

    public void SetActivation(bool value)
    {
        activated = value;
    }

    public override bool ConditionSatisfied()
    {
        return activated;
    }
}
