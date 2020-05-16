using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguesHandler : MonoBehaviour
{
    public DialogueEditorData data;

    public Dialogue StartDialogue;

    [HideInInspector]
    public List<Actor> dialogueActors = new List<Actor>();

    public int GetActor(string name, DialogueActorType type)
    {
        for (int i = 0; i < dialogueActors.Count; i++)
        {
            if (dialogueActors[i].Name == name)
                return i;
        }

        dialogueActors.Add(new Actor(null, name, type));
        return dialogueActors.Count - 1;
    }

    [System.Serializable]
    public class Actor 
    {
        public Actor() { }
        public Actor(Transform transformPosition, string name, DialogueActorType type) {
            ObjectTransform = transformPosition;
            Name = name;
            ActorType = type;
        }

        public Transform ObjectTransform;
        public string Name;
        public DialogueActorType ActorType;
    }
}
