using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueAction : GameAction
{
    public enum ActionType {Start, End}

    public ActionType TimeTypeActivation;
}
