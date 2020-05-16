using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public BubbleSpawner BubbleSpawner;

    public ResponseButtonsController buttonsController;

    private Dialogue ActiveDialogue;

    public static DialogueManager instance;

    public event DialogueEventHandler DialogueStartEvent;
    public event DialogueEventHandler DialogueEndEvent;

    [HideInInspector]
    public bool IsActive { get; private set; }

    private void OnLevelWasLoaded(int level)
    {
        DialogueStartEvent = null;
        DialogueEndEvent = null;
    }

    private void Awake()
    {
        if (instance == null)
        { 
            instance = this; 
        }
        else if (instance == this)
        { 
            Destroy (gameObject); 
        }

        IsActive = false;
    }

    public void StartDialogueSequence(Dialogue start)
    {
        IsActive = true;
        ActiveDialogue = start;
        if (DialogueStartEvent != null)
            DialogueStartEvent.Invoke(this, new DialogueEventArguments(ActiveDialogue.ActiveSentence));
        ActicateDialogue ();
    }

    public void ActicateDialogue()
    {
        ActiveDialogue.StartDialogue (BubbleSpawner, buttonsController);        
    }

    public void EndDialogueSequence()
    {
        if (DialogueEndEvent != null)
            DialogueEndEvent.Invoke(this, null);
        IsActive = false;
    }

    public void NextDialogue(Dialogue dialogue)
    {
        ActiveDialogue.EndDialogue ();
        ActiveDialogue = dialogue;

        if (ActiveDialogue == null)
            EndDialogueSequence ();
        else
            ActicateDialogue ();
    }
}

public delegate void DialogueEventHandler(DialogueManager sender, DialogueEventArguments e);
