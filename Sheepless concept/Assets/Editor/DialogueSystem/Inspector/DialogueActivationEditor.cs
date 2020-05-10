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
    private string labelStartDialogue = "Start Dialogue";
    private string[] dialogueOptions;
    private int selectedOption = 0;
    private int previousSelected = 0;

    private DialogueActivation activation;
    private DialoguesHandler[] allDialogueHandlers;
    private DialogueActivationCondition dialogueConditions;
    public override void OnCustomEnable()
    {
        allDialogueHandlers = FindObjectsOfType<DialoguesHandler>();

        activation = (DialogueActivation)target;

        if (dialogueConditions == null)
            dialogueConditions = activation.GetComponent<DialogueActivationCondition>();

        dialogueOptions = new string[allDialogueHandlers.Length];

        for (int i = 0; i < allDialogueHandlers.Length; i++)
        {
            dialogueOptions[i] = allDialogueHandlers[i].name;
        } 
    }

    public override void OnCustomInspectorGUI()
    {
        selectedOption = EditorGUILayout.Popup(labelStartDialogue, selectedOption, dialogueOptions);

        dialogueConditions = (DialogueActivationCondition)EditorGUILayout.ObjectField("Condition", 
                dialogueConditions, typeof(DialogueActivationCondition), true);

        activation.StartDialogue = allDialogueHandlers[selectedOption].StartDialogue;

        if (previousSelected != selectedOption)
        {
            EditorUtility.SetDirty(activation);
            EditorSceneManager.MarkSceneDirty(activation.gameObject.scene);
        }

        if (activation.StartDialogue == null)
        {
            Debug.LogError("No Start Dialogue in the " + allDialogueHandlers[selectedOption].name);
        }
    }
}
