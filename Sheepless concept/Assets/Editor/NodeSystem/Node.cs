using UnityEngine;
using System;

public class Node 
{
    public Rect WindowRect;

    public NodeConnectioPoint PointIn;
    public NodeConnectioPoint PointOut;

    public GUIStyle NodeStyle;
    public GUIStyle NodeStyleNormal;
    public GUIStyle NodeStyleActive;
    public GUIStyle CloseButtonStyle;

    private Vector2 offset;
    private Rect closeButtonRect;

    public Action<Node> OnNodeRemove;

    public Node(Vector2 position, Vector2 size, 
        GUIStyle styleNormal, GUIStyle styleActive, GUIStyle pointStyle, GUIStyle closeStyle,
        Action<NodeConnectioPoint> nodeConnAct, Action<Node> closeAct)
    {
        WindowRect = new Rect(position, size);
        NodeStyleNormal = styleNormal;
        NodeStyleActive = styleActive;

        CloseButtonStyle = closeStyle;
        OnNodeRemove += closeAct;

        NodeStyle = styleNormal;

        offset = new Vector2(WindowRect.width/2 - 10f, -WindowRect.height/2 + 10f);
        closeButtonRect = new Rect(0, 0, 20, 20);

        PointIn = new NodeConnectioPoint(this, NodeConnectioPoint.Type.In, 
            new Vector2(-20, WindowRect.height / 2), 
            pointStyle, nodeConnAct);
        PointOut = new NodeConnectioPoint(this, NodeConnectioPoint.Type.Out, 
            new Vector2(WindowRect.width, WindowRect.height / 2), 
            pointStyle, nodeConnAct);
    }

    public virtual void Move(Vector2 value)
    {
        WindowRect.position += value;
    }

    public virtual void Draw()
    {
        if (PointIn != null)
            PointIn.Draw();
        if (PointOut != null)
            PointOut.Draw();

        GUI.Box(WindowRect, "", NodeStyle);

         closeButtonRect.center = WindowRect.center + offset;

        if (GUI.Button(closeButtonRect, "", CloseButtonStyle))
        {
            OnNodeRemove.Invoke(this);
        }
    }

    public virtual bool HoldEvent(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (WindowRect.Contains(e.mousePosition))
                    {
                        NodeStyle = NodeStyleActive;
                        return true;
                    }
                    else
                    {
                        NodeStyle = NodeStyleNormal;
                        return true;
                    }
                }
                break;
            case EventType.MouseDrag:
                if (e.button == 0 && WindowRect.Contains(e.mousePosition))
                {
                    Move(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }
}
