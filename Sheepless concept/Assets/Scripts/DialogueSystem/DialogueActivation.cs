using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueActivation : MonoBehaviour
{
    public Dialogue StartDialogue;

    public DialogueActivationCondition Condition;

    private DialogueManager dm;

    void Start()
    {
        dm = DialogueManager.instance;

    }

    // Update is called once per frame
    void Update()
    {
        if (!dm.IsActive && Condition.IsAccepted ())
        {
            dm.StartDialogueSequence (StartDialogue);
        }
    }
}
