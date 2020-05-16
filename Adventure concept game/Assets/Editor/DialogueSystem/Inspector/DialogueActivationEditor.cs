using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Reflection;
using System;
using System.Linq;

[CustomEditor(typeof(DialogueActivation))]
public class DialogueActivationEditor : DefaultEditor<DialogueActivation>
{
    private Dialogue selectedOption;

    private DialogueActivation activation;
    private DialoguesHandler dialogueHandler;
    private DialogueActivationCondition dialogueConditions;
    public override void OnCustomEnable()
    {
        activation = (DialogueActivation)target;

        foreach (var handler in FindObjectsOfType<DialoguesHandler>())
        {
            if (handler.StartDialogue == activation.StartDialogue)
            {
                dialogueHandler = handler;
                break;
            }
        }

        selectedOption = dialogueHandler.StartDialogue;
        dialogueConditions = activation.Condition;

        if (dialogueConditions == null)
            dialogueConditions = activation.GetComponent<DialogueActivationCondition>();
    }

    public override void OnCustomInspectorGUI()
    {
        dialogueHandler = (DialoguesHandler)EditorGUILayout.ObjectField("Start Dialogue",
                dialogueHandler, typeof(DialoguesHandler), true);

        dialogueConditions = (DialogueActivationCondition)EditorGUILayout.ObjectField("Condition", 
                dialogueConditions, typeof(DialogueActivationCondition), true);

        if (dialogueHandler != null)
            activation.StartDialogue = dialogueHandler.StartDialogue;

        activation.Condition = dialogueConditions;

        if (dialogueHandler != null && dialogueHandler.StartDialogue != selectedOption)
        {
            selectedOption = dialogueHandler.StartDialogue;
            EditorUtility.SetDirty(activation);
            EditorSceneManager.MarkSceneDirty(activation.gameObject.scene);
        }

        if (activation.StartDialogue == null)
        {
            Debug.LogError("No Start Dialogue in the " + dialogueHandler.name);
        }
    }
}
