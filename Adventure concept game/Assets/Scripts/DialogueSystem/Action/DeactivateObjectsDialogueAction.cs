using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateObjectsDialogueAction : DialogueAction
{
    public GameObject[] Objects;

    public override void Activate()
    {
        foreach (var obj in Objects)
        {
            obj.SetActive(false);
        }
    }
}
