using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueEditorData 
{
    public List<Node> nodes;
    public List<Connection> connections;

    [System.Serializable]
    public class Node
    {
        public Node() {}

        public Node(int id, Vector2 position, Dialogue dialogue, int actorIndex)
        {
            ID = id;
            Position = position;
            CurrentDialogue = dialogue;
            ActorIndex = actorIndex;
        }

        public int ID;
        public int ActorIndex;
        public Vector2 Position;
        public Dialogue CurrentDialogue;
    }

    [System.Serializable]
    public class Connection
    {
        public Connection(Vector2 pointIn, Vector2 pointOut)
        {
            PointIn = pointIn;
            PointOut = pointOut;
        }

        public Vector2 PointIn;
        public Vector2 PointOut;
    }

    public Node AddNode(int id, Vector2 position, Dialogue dialogue, int actorIndex)
    {
        if (nodes == null)
            nodes = new List<Node>();
        Node node = new Node(id, position, dialogue, actorIndex);
        nodes.Add(node);

        return node;
    }

    public void AddConnection(Vector2 noteIn, Vector2 noteOut)
    {
        if (connections == null)
            connections = new List<Connection>();

        connections.Add(new Connection(noteIn, noteOut));
    }

    public Node FindNodeByDialogueName(string GameObjectDialogueName)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            int result = string.Compare(nodes[i].CurrentDialogue.name, GameObjectDialogueName);

            if (result == 0)
                return nodes[i];
        }

        return null;
    }
}
