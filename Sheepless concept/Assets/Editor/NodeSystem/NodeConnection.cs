using UnityEngine;
using UnityEditor;
using System;

public class NodeConnection 
{
    public NodeConnectioPoint pointIn;
    public NodeConnectioPoint pointOut;

    public GUIStyle buttonStyle;

    public Action<NodeConnection> OnClickAction;

    private Rect buttonRect = new Rect(0, 0, 20, 20);

    public NodeConnection(NodeConnectioPoint In, NodeConnectioPoint Out, GUIStyle style, Action<NodeConnection> act)
    {
        pointIn = In;
        pointOut = Out;
        buttonStyle = style;
        OnClickAction += act;
    }

    public void Draw()
    {
        Vector3 startPos = pointIn.rect.center;
        Vector3 endPos = pointOut.rect.center;
        Vector3 startTan = startPos + 25 * Vector3.left;
        Vector3 endTan = endPos + 25 * Vector3.right;

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 2f);

        buttonRect.center = (startPos + endPos) / 2;

        if (GUI.Button(buttonRect, "", buttonStyle))
        {
            OnClickAction.Invoke(this);
        }
    }
}
