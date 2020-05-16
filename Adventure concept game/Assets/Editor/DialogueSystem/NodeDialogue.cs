using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class NodeDialogue : Node
{
    public int ID;
    public string DialogueText = "Type text ...";
    public int SelectedActor;

    public Rect LabelRect;
    public Rect ActorPopUpRect;
    public Dialogue DialogueObject;

    private Vector2 textOffset = new Vector2(15f, 20f);
    private int prevActor;
    private string[] actorsOptions;

    private Action<NodeDialogue> OnActorChanged;

    public NodeDialogue(int id, Vector2 position, Vector2 size, Dialogue dialogue,
        GUIStyle styleNormal, GUIStyle styleActive, GUIStyle pointStyle, GUIStyle closeStyle,
        Action<NodeConnectioPoint> nodeConnAct, Action<Node> closeAct, Action<NodeDialogue> actionChangeActor) : 
        base(position, size, styleNormal, styleActive, pointStyle, closeStyle, nodeConnAct, closeAct)
    {
        ID = id;
        LabelRect = new Rect(textOffset, new Vector2(WindowRect.width - textOffset.x * 2, 50f));
        DialogueObject = dialogue;

        if (dialogue.ActiveSentence.Sentence.Length != 0)
            DialogueText = dialogue.ActiveSentence.Sentence;

        ActorPopUpRect = new Rect(15f, 80f, WindowRect.width - 30f, 20f);

        OnActorChanged += actionChangeActor;
    }

    public override void Draw()
    {
        base.Draw();
        GUI.BeginGroup(WindowRect);
        DialogueText = GUI.TextArea(LabelRect, DialogueText);
        DialogueObject.ActiveSentence.Sentence = DialogueText;

        ActorPopUp();

        GUI.EndGroup();
    }

    public void UpdateActors(List<DialoguesHandler.Actor> actors)
    {
        actorsOptions = new string[actors.Count];

        for (int i = 0; i < actorsOptions.Length; i++)
        {
            actorsOptions[i] = actors[i].Name;
        }

        if (SelectedActor < actors.Count)
            OnActorChanged.Invoke(this);
    }

    private void ActorPopUp()
    {
        SelectedActor = EditorGUI.Popup(ActorPopUpRect, SelectedActor, actorsOptions);

        if (prevActor != SelectedActor)
        {
            prevActor = SelectedActor;
            OnActorChanged.Invoke(this);
        }
    }

    public override bool HoldEvent(Event e)
    {
        return base.HoldEvent(e);
    }
}
