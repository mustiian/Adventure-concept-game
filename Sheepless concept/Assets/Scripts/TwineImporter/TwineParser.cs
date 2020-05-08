using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace TwineD
{
    public class TwineParser : EditorWindow
    {
        public TwineDialogue TwineStory;

        public TextAsset FileJSON;

        public string NewFileName;

        private GUILayoutOption[] layoutOptions = { GUILayout.MaxWidth(300.0f), GUILayout.MinWidth(10.0f) };

        [MenuItem("Window/Twine Dialogue Parser")]
        private static void OpenWindow()
        {
            TwineParser window = EditorWindow.GetWindow<TwineParser>();
            window.titleContent = new GUIContent("Twine Parser");
        }

        private void OnGUI()
        {
            OnGUIDisplay();
        }

        private void OnGUIDisplay()
        {
            EditorGUILayout.LabelField("New dialogue prefab name:", layoutOptions);
            NewFileName = EditorGUILayout.TextField(NewFileName);

            EditorGUILayout.LabelField("JSON dialogue file:", layoutOptions);
            FileJSON = (TextAsset)EditorGUILayout.ObjectField(FileJSON, typeof(TextAsset), true);

            if (GUILayout.Button("Complete.") && FileJSON != null)
            {
                Debug.Log("Start parsing");

                // Create new Dialogue controller
                var dialogueObject = new GameObject();
                var dialogueController = dialogueObject.AddComponent<DialogueController>();

                ParseFile(dialogueController);

                Debug.Log("End parsing");

                //SaveObjectAsPrefab(dialogueObject);

                //DestroyImmediate(dialogueObject);
            }
        }

        private void ParseFile(DialogueController dialogueController)
        {
            TwineStory = TwineDialogue.CreateFromJSON(FileJSON.text);

            dialogueController.name = TwineStory.name;
            dialogueController.startDialogue = TwineStory.startnode;
            dialogueController.Dialogues = new Dialogue[TwineStory.passages.Length];

            // Create new DIalogues 
            for (int i = 0; i < dialogueController.Dialogues.Length; i++)
            {
                // Create new Dialogue object with correct parent
                var dialogueObject = new GameObject();
                var dialogue = dialogueObject.AddComponent<Dialogue>();
                dialogueObject.transform.parent = dialogueController.transform;
                dialogueObject.name = TwineStory.passages[i].name;
                dialogueController.Dialogues[i] = dialogue;

                // Set Dialogue params
                dialogueController.Dialogues[i].Name = TwineStory.passages[i].name;
                dialogueController.Dialogues[i].ID = TwineStory.passages[i].pid;
                dialogueController.Dialogues[i].Text = TwineStory.passages[i].text;
            }

            // Parse dialogue text 
            for (int i = 0; i < dialogueController.Dialogues.Length; i++)
            {
                // Save all links for the current Dialogue
                if (TwineStory.passages[i].links != null)
                {
                    for (int j = 0; j < TwineStory.passages[i].links.Length; j++)
                    {
                        dialogueController.Dialogues[i].Links.Add(TwineStory.passages[i].links[j]);
                    }
                }
                   
                ParseDialogue(dialogueController.Dialogues[i]);
            }
        }
        
        private void SaveObjectAsPrefab(GameObject obj)
        {
            obj.name = NewFileName;

            PrefabUtility.SaveAsPrefabAsset(obj, "Assets" + @"\" + NewFileName + ".prefab");
            Debug.Log("Prefab file saved to the Assets folder");
        }

        private void ParseDialogue(Dialogue dialogue)
        {
            string[] lines = dialogue.Text.Split(new char[] { '\n' });

            foreach (var line in lines)
            {
                Debug.Log(line);
            }

            Regex regexMoveExpr = new Regex(@"\[\[.*\]\]");
            MatchCollection matches = regexMoveExpr.Matches(dialogue.Text);
        }
    }
}