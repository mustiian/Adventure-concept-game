using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEventArguments 
{
    public SentenceWrapper Sentence { private set; get; }

    public DialogueEventArguments(SentenceWrapper sentence)
    {
        Sentence = sentence;
    }
}
