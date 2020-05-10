using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneChangerButtonPressedCondition : SceneChangerCondition
{
    public KeyCode Key;

    public override bool ConditionSatisfied()
    {
        return (Input.GetKeyDown(Key));
    }
}
