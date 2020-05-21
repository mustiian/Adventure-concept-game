using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeStartDialogueAction : DialogueAction
{
    public DialogueActivation DialogueActivation;
    public Dialogue NewStartDialogue;

    public override void Activate()
    {
        DialogueActivation.StartDialogue = NewStartDialogue;
    }
}
