using System;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;

public class NodeEditorDialogue: NodeEditor<NodeDialogue>
{
    public DialogueEditorData Data;

    private DialoguesHandler dialogueHandler;

    private int nodeCount;

    public void Init(DialoguesHandler dialogueHandler)
    {
        Data = dialogueHandler.data;

        this.dialogueHandler = dialogueHandler;

        if (Data.nodes != null)
        {
            LoadData();
        }
        else
        {
            nodeCount = 0;
        }
    }

    private void LoadData()
    {
        for (int i = 0; i < Data.nodes.Count; i++)
        {
            Action<NodeConnectioPoint> actionPoint = OnClickPoint;
            Action<Node> actionNode = OnClickRemoveNode;
            Action<NodeDialogue> actionChangeActor = OnChangeActor;

            var node = new NodeDialogue(Data.nodes[i].ID, Data.nodes[i].Position, new Vector2(140, 200),
                Data.nodes[i].CurrentDialogue,
                nodeStyleNormal, nodeStyleActive, pointStyle, closeNodeButtonStyle,
                actionPoint, actionNode, actionChangeActor);

            node.SelectedActor = Data.nodes[i].ActorIndex;

            if (Data.nodes[i].ID > nodeCount)
                nodeCount = Data.nodes[i].ID;

            node.UpdateActors(dialogueHandler.dialogueActors);
            nodes.Add(node);
        }
        if (Data.connections != null)
            for (int i = 0; i < Data.connections.Count; i++)
            {

                NodeConnection conn = new NodeConnection(GetNodeFromPosition(Data.connections[i].PointIn).PointIn,
                                                        GetNodeFromPosition(Data.connections[i].PointOut).PointOut, 
                                                        connectionButtonStyle, OnClickRemoveConnection);

                connections.Add(conn);
            }
    }

    public void UpdateActors(List<DialoguesHandler.Actor> actors)
    {
        dialogueHandler.dialogueActors = actors;

        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].UpdateActors(actors);
        }
    }

    protected override void OnGUIDisplay()
    {
        base.OnGUIDisplay();
    }

    protected override GenericMenu MouseContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();

        AddComponentMenu(menu, "Add Dialogue", (object obj) => OnClickAddNode(mousePos), mousePos);

        return menu;
    }

    protected override void OnClickAddNode(Vector2 mousePos)
    {
        Action<NodeConnectioPoint> actionPoint = OnClickPoint;
        Action<Node> actionNode = OnClickRemoveNode;
        Action <NodeDialogue> actionChangeActor = OnChangeActor;

        Dialogue dialogue = CreateDialogueObject();

        var node = new NodeDialogue(++nodeCount, mousePos, new Vector2(140, 200), dialogue,
            nodeStyleNormal, nodeStyleActive, pointStyle, closeNodeButtonStyle,
            actionPoint, actionNode, actionChangeActor);

        node.UpdateActors(dialogueHandler.dialogueActors);
        nodes.Add(node);
    }

    private Dialogue CreateDialogueObject()
    {
        GameObject newWindow = new GameObject("Dialogue " + nodes.Count);
        newWindow.transform.parent = Selection.activeTransform;
        Dialogue dialogue = newWindow.AddComponent<Dialogue>();

        EditorUtility.SetDirty(dialogue);
        EditorSceneManager.MarkSceneDirty(dialogue.gameObject.scene);

        return dialogue;
    }

    protected override void OnClickRemoveNode(Node node)
    {
        List<NodeConnection> connectionsToRemove = new List<NodeConnection>();

        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i].pointIn == node.PointIn ||
                connections[i].pointOut == node.PointOut)
            {
                connectionsToRemove.Add(connections[i]);
            } 
        }

        for (int i = 0; i < connectionsToRemove.Count; i++)
        {
            connections.Remove(connectionsToRemove[i]);
            RemoveDialogueConnection(connectionsToRemove[i]);
        }

        nodes.Remove((NodeDialogue)node);

        NodeDialogue dialogue = (NodeDialogue)node;
        GameObject.DestroyImmediate(dialogue.DialogueObject.gameObject);
    }

    protected override void OnClickRemoveConnection(NodeConnection connection)
    {
        base.OnClickRemoveConnection(connection);

        RemoveDialogueConnection(connection);
    }

    private void RemoveDialogueConnection(NodeConnection connection)
    {
        NodeDialogue dialogueStart = (NodeDialogue)connection.pointOut.node;
        NodeDialogue dialogueNext = (NodeDialogue)connection.pointIn.node;

        dialogueStart.DialogueObject.DeleteResponse(dialogueNext.DialogueObject);
    }

    protected override void HandleEvent(Event e)
    {
        base.HandleEvent(e);
    }

    protected override GenericMenu NodeContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = base.NodeContextMenu(mousePos);
        AddComponentMenu(menu, "Set start dialogue", (object obj) => OnClickSetStartDialogue(mousePos), mousePos);
        return menu;
    }

    private void OnClickSetStartDialogue(Vector2 mousePos)
    {
        NodeDialogue node = GetNodeFromPosition(mousePos);

        dialogueHandler.StartDialogue = node.DialogueObject;

        Debug.Log("Set start " + dialogueHandler.name);

        EditorUtility.SetDirty(dialogueHandler);
        EditorSceneManager.MarkSceneDirty(dialogueHandler.gameObject.scene);
    }

    private void OnChangeActor(NodeDialogue node)
    {
        node.DialogueObject.ActiveSentence.Character = dialogueHandler.dialogueActors[node.SelectedActor].ActorType;
        node.DialogueObject.ActiveSentence.Position = dialogueHandler.dialogueActors[node.SelectedActor].ObjectTransform;

        EditorUtility.SetDirty(node.DialogueObject);
        EditorSceneManager.MarkSceneDirty(node.DialogueObject.gameObject.scene);
    }

    protected override NodeConnection CreateConnection()
    {
        NodeConnection nc = base.CreateConnection();
        NodeDialogue dialogueStart = (NodeDialogue)nc.pointOut.node;
        NodeDialogue dialogueNext = (NodeDialogue)nc.pointIn.node;

        dialogueStart.DialogueObject.AddResponse(dialogueNext.DialogueObject);

        EditorUtility.SetDirty(dialogueStart.DialogueObject);
        EditorSceneManager.MarkSceneDirty(dialogueStart.DialogueObject.gameObject.scene);

        return nc;
    }

    private void OnDestroy()
    {
        if (Data.nodes != null)
            Data.nodes.Clear();

        if (Data.connections != null)
            Data.connections.Clear();

        for (int i = 0; i < nodes.Count; i++)
        {
            Data.AddNode(nodes[i].ID, nodes[i].WindowRect.position, nodes[i].DialogueObject, nodes[i].SelectedActor);
        }

        Data.connections = new List<DialogueEditorData.Connection>(connections.Count);

        for (int i = 0; i < connections.Count; i++)
        {
            Data.AddConnection(connections[i].pointIn.node.WindowRect.center,
                                connections[i].pointOut.node.WindowRect.center);
        }
    }
}
