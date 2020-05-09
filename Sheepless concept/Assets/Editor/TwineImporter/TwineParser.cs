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

        private int TwineDialogueCount;

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
                var dialogueHandler = dialogueObject.AddComponent<DialoguesHandler>();

                ParseFile(dialogueHandler);

                Debug.Log("End parsing");

                //SaveObjectAsPrefab(dialogueObject);

                //DestroyImmediate(dialogueObject);
            }
        }

        private void ParseFile(DialoguesHandler dialogueHandler)
        {
            TwineStory = TwineDialogue.CreateFromJSON(FileJSON.text);

            dialogueHandler.name = NewFileName;
            dialogueHandler.data = new DialogueEditorData();
            float nodeOffxetX = 0f;

            // Add Player actor as default
            dialogueHandler.dialogueActors.Add(new DialoguesHandler.Actor(null, "Player", DialogueActorType.Player));

            // Create new Dialogues 
            for (int i = 0; i < TwineStory.passages.Length; i++)
            {
                // Create new Dialogue object with correct parent
                var dialogue = CreateDialogueObject(TwineStory.passages[i].name,
                                                    TwineStory.passages[i].text,
                                                    dialogueHandler.transform);

                // Set Dialogue params
                dialogueHandler.data.AddNode(TwineStory.passages[i].pid,
                    new Vector2(nodeOffxetX, 100f),
                    dialogue, 0);

                nodeOffxetX += 250f;
            }

            // Set start dialogue
            dialogueHandler.StartDialogue = dialogueHandler.data.nodes[TwineStory.startnode - 1].CurrentDialogue;
            nodeOffxetX = 0;
            TwineDialogueCount = dialogueHandler.data.nodes.Count;

           
            // Parse dialogue text 
            for (int i = 0; i < TwineDialogueCount; i++)
            {
                ParseDialogue(dialogueHandler.data.nodes[i].CurrentDialogue, dialogueHandler,
                                new Vector2 (nodeOffxetX, 100f), i);
                nodeOffxetX += 250f;
            }
        }

        private void SaveObjectAsPrefab(GameObject obj)
        {
            obj.name = NewFileName;

            PrefabUtility.SaveAsPrefabAsset(obj, "Assets" + @"\" + NewFileName + ".prefab");
            Debug.Log("Prefab file saved to the Assets folder");
        }

        private void ParseDialogue(Dialogue dialogue, DialoguesHandler dialogueHandler, Vector2 newPosition, int index)
        {
            float nodeOffsetY = newPosition.y;
            string[] lines = dialogue.ActiveSentence.Sentence.Split(new char[] { '\n' });
            var newDialogues = DialogueTextParser.GetDialogueFromTextLines(lines);
            int lastID = dialogueHandler.transform.childCount;
            Dialogue latestAddedDialogue = null;
            DialogueEditorData.Node baseNode = dialogueHandler.data.nodes[index];
            
            // Set all basic NPC's dialogues
            if (newDialogues.npcText.Count != 0)
            {
                // Set base Dialogue node
                dialogue.ActiveSentence.Sentence = newDialogues.npcText[0].Sentence;
                latestAddedDialogue = dialogue;
                baseNode.ActorIndex = dialogueHandler.GetActor(newDialogues.npcText[0].ActorName, DialogueActorType.NPC);
                
                for (int i = 1; i < newDialogues.npcText.Count; i++)
                {
                    // Create new Dialogue object with correct parent
                    var newDialogue = CreateDialogueObject(newDialogues.npcText[i].Sentence,
                                                            newDialogues.npcText[i].Sentence,
                                                            dialogueHandler.transform, 
                                                            DialogueActorType.NPC);

                    latestAddedDialogue.AddResponse(newDialogue);
                    latestAddedDialogue = newDialogue;
                    nodeOffsetY += 250;

                    // Set Dialogue params
                    var actorIndex = dialogueHandler.GetActor(newDialogues.npcText[i].ActorName, DialogueActorType.NPC);
                    var newNode = dialogueHandler.data.AddNode(++lastID,
                        new Vector2(newPosition.x, nodeOffsetY),
                        newDialogue, actorIndex);

                    dialogueHandler.data.AddConnection(newNode.Position, baseNode.Position);
                    baseNode = newNode; 
                }
            }

            // Set all Player's responses
            if (newDialogues.playerText.Count != 0)
            {
                for (int i = 0; i < newDialogues.playerText.Count; i++)
                {
                    // Create new Dialogue object with correct parent
                    var newDialogue = CreateDialogueObject(newDialogues.playerText[i].Sentence,
                                                            newDialogues.playerText[i].Sentence,
                                                            dialogueHandler.transform,
                                                            DialogueActorType.Player);

                    latestAddedDialogue.AddResponse(newDialogue);

                    nodeOffsetY += 250;

                    // Set Dialogue params
                    var actorIndex = dialogueHandler.GetActor("Player", DialogueActorType.NPC);
                    var newNode = dialogueHandler.data.AddNode(++lastID,
                        new Vector2(newPosition.x, nodeOffsetY),
                        newDialogue, actorIndex);

                    // Set Dialogue link 
                    dialogueHandler.data.AddConnection(newNode.Position, baseNode.Position);
                    var linkNode = dialogueHandler.data.FindNodeByDialogueName(newDialogues.playerText[i].LinkText);
                    dialogueHandler.data.AddConnection(linkNode.Position, newNode.Position);
                    newNode.CurrentDialogue.AddResponse(linkNode.CurrentDialogue);
                }
            }

        }

        private Dialogue CreateDialogueObject(string Name, string Text, Transform parent = null, DialogueActorType actorType = 0)
        {
            // Create new Dialogue object with correct parent
            var dialogueObject = new GameObject();
            var dialogue = dialogueObject.AddComponent<Dialogue>();
            dialogueObject.transform.parent = parent;
            dialogueObject.name = Name;
            dialogue.ActiveSentence.Sentence = Text;
            dialogue.ActiveSentence.Character = actorType;

            return dialogue;
        }
    }
}