using UnityEngine;
using UnityEditor;
using System;

public class NodeConnectioPoint 
{
    public enum Type { In, Out }

    public Node node;
    public Type connectionType;
    public Rect rect;
    public Vector2 positionOffset;
    public GUIStyle pointStyle;

    public Action<NodeConnectioPoint> OnClickAction;

    public NodeConnectioPoint(Node node, NodeConnectioPoint.Type type, Vector2 positionOffset, GUIStyle style, Action<NodeConnectioPoint> act)
    {
        rect = new Rect(0, 0, 20, 20);
        this.node = node;
        connectionType = type;
        pointStyle = style;
        OnClickAction += act;

        this.positionOffset = positionOffset;
    }

   public void Draw()
    {
        rect.position = node.WindowRect.position + positionOffset; 

        if (GUI.Button(rect, "", pointStyle))
        {
            OnClickAction.Invoke(this);
        }
    }
}