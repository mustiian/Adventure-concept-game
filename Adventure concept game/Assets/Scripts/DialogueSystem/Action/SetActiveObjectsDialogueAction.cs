using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObjectsDialogueAction : DialogueAction
{
    public enum ActiveType {True, False}
    public ActiveType ObjectsActiveType;

    public GameObject[] Objects;

    public override void Activate()
    {
        if (ObjectsActiveType == ActiveType.False)
            SetActive(false);
        else
            SetActive(true);
    }

    private void SetActive(bool value)
    {
        foreach (var obj in Objects)
        {
            obj.SetActive(value);
        }
    }
}
