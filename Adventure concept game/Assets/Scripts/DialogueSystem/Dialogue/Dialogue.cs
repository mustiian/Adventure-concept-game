using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    public SentenceWrapper ActiveSentence = new SentenceWrapper();

    public Dialogue NextDialogue { get; private set; }

    public bool isActive { get; private set; }

    [Range(0, 10)]
    public float Delay = 2f;

    public List<Dialogue> Responses = new List<Dialogue>();

    private ResponseButtonsController responseButtons;

    private DialogueAction[] allActions;

    private void Start()
    {
        allActions = GetComponents<DialogueAction>();
    }

    public void StartDialogue(BubbleSpawner bubble, ResponseButtonsController buttons)
    {
        isActive = true;

        bubble.Spawn (ref ActiveSentence.Position, ActiveSentence.Sentence, Delay, true);

        ActivateActions(DialogueAction.ActionType.Start);

        if (ActiveSentence.Character == DialogueActorType.NPC)
        {
            if (NextActorIsPlayer())
            {
                responseButtons = buttons;
                StartCoroutine(ResponseButtonsEnumarator(Delay));
            }
            else
            {
                StartCoroutine(ResponseEnumarator(Delay, 0));
            }
            
        } 
        else
        {
            StartCoroutine (ResponseEnumarator (Delay, 0));
        }
    }

    public void EndDialogue()
    {
        if (NextActorIsPlayer())
            responseButtons.DeleteButtons ();

        ActivateActions(DialogueAction.ActionType.End);

        isActive = false;
    }

    public void AddResponse(Dialogue responce)
    {
        Responses.Add (responce);
    }

    public void DeleteResponse(Dialogue responce)
    {
        Responses.Remove (responce);
    }

    public string GetSentenceText()
    {
        return ActiveSentence.Sentence;
    }

    public void ChooseDialogue()
    {
        DialogueManager.instance.NextDialogue (this);
    }

    private bool NextActorIsPlayer()
    {
        if (Responses.Count == 0)
            return false;

        for (int i = 0; i < Responses.Count; i++)
        {
            if (Responses[i].ActiveSentence.Character == DialogueActorType.NPC)
                return false;
        }

        return true;
    }

    private IEnumerator ResponseEnumarator(float delay, int index)
    {
        yield return new WaitForSeconds (delay);
        if (Responses.Count > 0)
            Responses[index].ChooseDialogue ();
        else
        {
            DialogueManager.instance.NextDialogue (null);
        }
    }

    private IEnumerator ResponseButtonsEnumarator(float delay)
    {
        yield return new WaitForSeconds (delay);
        responseButtons.UpdateButtons (Responses);
    }

    private void ActivateActions(DialogueAction.ActionType type)
    {
        for (int i = 0; i < allActions.Length; i++)
        {
            if (allActions[i].ActionDialogueTimeActivation == type)
                allActions[i].Activate();
        }
    }
}