using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

public class NodeEditor<T> : EditorWindow where T: Node
{
    protected List<T> nodes = new List<T>();
    protected List<NodeConnection> connections = new List<NodeConnection>();

    protected GUIStyle nodeStyleNormal = new GUIStyle();
    protected GUIStyle nodeStyleActive = new GUIStyle();
    protected GUIStyle pointStyle = new GUIStyle();
    protected GUIStyle connectionButtonStyle = new GUIStyle();
    protected GUIStyle closeNodeButtonStyle = new GUIStyle();

    protected NodeConnectioPoint selectedPointIn;
    protected NodeConnectioPoint selectedPointOut;

    private void OnEnable()
    {
        nodeStyleNormal.normal.background = EditorGUIUtility.Load("node_graphic.png") as Texture2D;
        nodeStyleActive.normal.background = EditorGUIUtility.Load("node_graphic_active.png") as Texture2D;

        pointStyle.normal.background = EditorGUIUtility.Load("node_point_graphic.png") as Texture2D;
        pointStyle.active.background = EditorGUIUtility.Load("node_point_graphic_active.png") as Texture2D;

        closeNodeButtonStyle.normal.background = EditorGUIUtility.Load("node_point_graphic_close.png") as Texture2D;

        connectionButtonStyle.normal.background = EditorGUIUtility.Load("node_point_button_graphic.png") as Texture2D;
    }

    [MenuItem("Window/Node Editor")]
    private static void OpenWindow()
    {
        NodeEditor<Node> window = EditorWindow.GetWindow<NodeEditor<Node>>();
        window.titleContent = new GUIContent("Node Editor");
    }

    private void OnGUI()
    {
        OnGUIDisplay();
    }

    protected virtual void OnGUIDisplay()
    {
        HandleEvent(Event.current);
        DrawConnectionLine(Event.current.mousePosition);

        DrawNodes();
        DrawConnections();
    }

    protected void DrawNodes()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            nodes[i].Draw();
        }
    }

    protected void DrawConnections()
    {
        for (int i = 0; i < connections.Count; i++)
        {
            connections[i].Draw();
        }
    }

    protected virtual void HandleEvent(Event e)
    {
        HandleNodesEvent(e);

        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 1)
                {
                    if (GetNodeFromPosition(e.mousePosition) == null)
                    {
                        GenericMenu menu = MouseContextMenu(e.mousePosition);
                        menu.ShowAsContext();
                        Repaint();
                    }
                    else
                    {
                        GenericMenu menu = NodeContextMenu(e.mousePosition);
                        menu.ShowAsContext();
                        Repaint();
                    }
                    
                } 
                break;
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    MoveCanvas(e.delta);
                    Repaint();
                }
                break;
        }
    }

    protected void AddComponentMenu(GenericMenu menu, string title, GenericMenu.MenuFunction2 func, object obj)
    {
        menu.AddItem(new GUIContent(title), false, func, obj);
    }

    protected virtual GenericMenu MouseContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();
        AddComponentMenu(menu, "Add node", (object obj) => OnClickAddNode(mousePos), mousePos);
        return menu;
    }

    protected virtual GenericMenu NodeContextMenu(Vector2 mousePos)
    {
        GenericMenu menu = new GenericMenu();
        return menu;
    }

    protected virtual void OnClickAddNode(Vector2 mousePos)
    {
        Action<NodeConnectioPoint> actionPoint = delegate (NodeConnectioPoint p)
        {
            OnClickPoint(p);
        };

        Action<T> actionNode = delegate (T p)
        {
            OnClickRemoveNode(p);
        };

        T node = System.Activator.CreateInstance(typeof(T), mousePos, new Vector2(140, 200),
            nodeStyleNormal, nodeStyleActive, pointStyle, closeNodeButtonStyle, 
            actionPoint, actionNode) as T;

        nodes.Add(node);
    }

    protected void MoveCanvas(Vector2 value)
    {
        foreach (Node node in nodes)
        {
            node.Move(value);
        }
    }

    protected void DrawConnectionLine(Vector2 position)
    {
        if (selectedPointIn != null && selectedPointOut == null)
        {
            Vector3 startTan = selectedPointIn.rect.center + 25 * Vector2.left;
            Vector3 endTan = position + 25 * Vector2.right;
            DrawLineBezir(selectedPointIn.rect.center, position, startTan, endTan);
            Repaint();
        }
        else if (selectedPointOut != null && selectedPointIn == null)
        {
            Vector3 startTan = selectedPointOut.rect.center + 25 * Vector2.right;
            Vector3 endTan = position + 25 * Vector2.left;
            DrawLineBezir(selectedPointOut.rect.center, position, startTan, endTan);
            Repaint();
        }
    }

    protected void DrawLineBezir(Vector3 startPos, Vector3 endPos, Vector3 startTan, Vector3 endTan)
    {
        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 2f);
    }

    protected void HandleNodesEvent(Event e)
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (nodes[i].HoldEvent(e))
            {
                Repaint();
            }
        }
    }

    protected T GetNodeFromPosition(Vector2 position)
    {
        foreach (var node in nodes)
        {
            if (node.WindowRect.Contains(position))
                return node;
        }

        return null;
    }

    protected void OnClickPoint(NodeConnectioPoint point)
    {
        if (point.connectionType == NodeConnectioPoint.Type.In)
        {
            ConnectPoints(point, ref selectedPointIn, ref selectedPointOut);
        }
        else if (point.connectionType == NodeConnectioPoint.Type.Out)
        {
            ConnectPoints(point, ref selectedPointOut, ref selectedPointIn);
        }
    }

    private void ConnectPoints(NodeConnectioPoint NewPoint, ref NodeConnectioPoint connectPoint, ref NodeConnectioPoint checkPoint)
    {
        connectPoint = NewPoint;

        if (checkPoint != null)
        {
            if (checkPoint.node != connectPoint.node)
            {
                NodeConnection conn = CreateConnection();
                connections.Add(conn);
            }
            RemoveSelectedPoints();
        }
    }

    protected virtual NodeConnection CreateConnection()
    {
        NodeConnection conn = new NodeConnection(selectedPointIn, selectedPointOut, connectionButtonStyle, OnClickRemoveConnection);
        return conn;
    }

    protected void RemoveSelectedPoints()
    {
        selectedPointIn = null;
        selectedPointOut = null;
    }

    protected virtual void OnClickRemoveConnection(NodeConnection connection)
    {
        connections.Remove(connection);
    }

    protected virtual void OnClickRemoveNode(Node node)
    {
        T newNode = (T)node;
        List<NodeConnection> connectionsToRemove = new List<NodeConnection>();

        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i].pointIn == node.PointIn ||
                connections[i].pointOut == node.PointOut)
                connectionsToRemove.Add(connections[i]);
        }

        for (int i = 0; i < connectionsToRemove.Count; i++)
        {
            connections.Remove(connectionsToRemove[i]);
        }

        nodes.Remove(newNode);
    }
}
