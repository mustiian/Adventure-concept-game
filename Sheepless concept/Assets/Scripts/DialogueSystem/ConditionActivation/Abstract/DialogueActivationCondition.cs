using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DialogueActivationCondition : MonoBehaviour
{
    public abstract bool IsAccepted();
}
