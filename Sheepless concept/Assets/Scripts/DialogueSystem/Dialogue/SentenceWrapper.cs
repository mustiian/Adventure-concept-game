using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DialogueActorType { NPC, Player }

[System.Serializable]
public class SentenceWrapper
{
    [TextArea]
    public string Sentence;

    public Transform Position;

    public DialogueActorType Character;

    public SentenceWrapper(string sentence, Transform position, DialogueActorType actorType)
    {
        Sentence = sentence;
        Position = position;
        Character = actorType;
    }

    public SentenceWrapper()
    {
        Sentence = "";
        Position = null;
        Character = DialogueActorType.NPC;
    }
}