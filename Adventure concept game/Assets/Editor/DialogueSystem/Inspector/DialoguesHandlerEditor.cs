using System.Collections;
using System.IO;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEditor;

[CustomEditor(typeof(DialoguesHandler))]
public class DialoguesHandlerEditor : DefaultEditor<DialoguesHandler>
{
    public NodeEditorDialogue dialogueWindow;

    private DialoguesHandler dialoguesHanlder;

    private bool loaded = false;

    private string[] actorsOptions;

    private GUILayoutOption[] layoutOptions = { GUILayout.MaxWidth(300.0f), GUILayout.MinWidth(10.0f) };

    public override void OnCustomEnable()
    {
        dialoguesHanlder = (DialoguesHandler)target;

        if (dialogueWindow != null)
            Debug.Log("Window is not null");

        actorsOptions = new string[] {
            DialogueActorType.NPC.ToString(),
            DialogueActorType.Player.ToString()
        };

        if (WasSaved())
        {
            loaded = true;
        }
    }

    public override void OnCustomInspectorGUI()
    {
        if (!loaded)
        {
            if (GUILayout.Button("Create new Dialogues"))
            {
                EditorUtility.SetDirty(dialoguesHanlder);
                EditorSceneManager.MarkSceneDirty(dialoguesHanlder.gameObject.scene);

                dialogueWindow = EditorWindow.GetWindow<NodeEditorDialogue>();
                dialogueWindow.titleContent = new GUIContent("Dialogue Editor");
                dialoguesHanlder.data = new DialogueEditorData();
                dialogueWindow.Init(dialoguesHanlder);
                dialoguesHanlder.dialogueActors.Clear();
                loaded = true;

                var player = GameObject.FindGameObjectWithTag("Player");
                if(player != null)
                {
                    dialoguesHanlder.dialogueActors.Add(new DialoguesHandler.Actor(player.transform, "Player", DialogueActorType.Player));
                    dialogueWindow.UpdateActors(dialoguesHanlder.dialogueActors);
                }
            }
        }
        else
        {
            if (GUILayout.Button("Open Dialogues"))
            {
                if (dialogueWindow == null)
                {
                    dialogueWindow = EditorWindow.GetWindow<NodeEditorDialogue>();
                    dialogueWindow.titleContent = new GUIContent("Dialogue Editor");
                    dialogueWindow.Init(dialoguesHanlder);
                }
            }
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            DrawActorsPanel();
        } 
    }

    private void DrawActorsPanel()
    {
        EditorGUILayout.LabelField("Actors: ");

        DrawActors();

        EditorGUILayout.BeginHorizontal();

        if (GUILayout.Button("+"))
        {
            dialoguesHanlder.dialogueActors.Add(new DialoguesHandler.Actor());

            EditorUtility.SetDirty(dialoguesHanlder);
            EditorSceneManager.MarkSceneDirty(dialoguesHanlder.gameObject.scene);
        }

        if (GUILayout.Button("-"))
        {
            if (dialoguesHanlder.dialogueActors.Count > 0)
            {
                dialoguesHanlder.dialogueActors.RemoveAt(dialoguesHanlder.dialogueActors.Count - 1);
                UpdateActors(dialoguesHanlder.dialogueActors.Count - 1);

                EditorUtility.SetDirty(dialoguesHanlder);
                EditorSceneManager.MarkSceneDirty(dialoguesHanlder.gameObject.scene);
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    private void DrawActors()
    {
        string tmpName;
        Transform tmpTransform;
        DialogueActorType tmpType;

        EditorGUILayout.BeginVertical();

        if (dialoguesHanlder.dialogueActors.Count > 0)
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Bubble position", layoutOptions);
            EditorGUILayout.LabelField("Name", layoutOptions);
            EditorGUILayout.LabelField("Type", layoutOptions);

            EditorGUILayout.EndHorizontal();
        }

        for (int i = 0; i < dialoguesHanlder.dialogueActors.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            tmpTransform = (Transform)EditorGUILayout.ObjectField(
                dialoguesHanlder.dialogueActors[i].ObjectTransform, 
                typeof(Transform), true);

            tmpName = EditorGUILayout.TextField(dialoguesHanlder.dialogueActors[i].Name);

            tmpType = (DialogueActorType)EditorGUILayout.Popup((int)dialoguesHanlder.dialogueActors[i].ActorType, actorsOptions);

            if ((dialoguesHanlder.dialogueActors[i].ObjectTransform != tmpTransform ||
                dialoguesHanlder.dialogueActors[i].Name            != tmpName       ||
                dialoguesHanlder.dialogueActors[i].ActorType       != tmpType)      &&
                (tmpName != null || tmpTransform != null))
            {
                dialoguesHanlder.dialogueActors[i].ObjectTransform = tmpTransform;
                dialoguesHanlder.dialogueActors[i].Name = tmpName;
                dialoguesHanlder.dialogueActors[i].ActorType = tmpType;

                UpdateActors(i);

                EditorUtility.SetDirty(dialoguesHanlder);
                EditorSceneManager.MarkSceneDirty(dialoguesHanlder.gameObject.scene);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
    }

    private void UpdateActors(int newActorIndex)
    {
        for (int i = 0; i < dialoguesHanlder.data.nodes.Count; i++)
        {
            if (dialoguesHanlder.data.nodes[i].ActorIndex == newActorIndex)
            {
                dialoguesHanlder.data.nodes[i].CurrentDialogue.ActiveSentence.Position = dialoguesHanlder.dialogueActors[newActorIndex].ObjectTransform;
            }
        }
    }

    private bool WasSaved()
    {
        if (dialoguesHanlder.data != null)
            if (dialoguesHanlder.transform.childCount > 0)
                return true;

        return false;
    }
}