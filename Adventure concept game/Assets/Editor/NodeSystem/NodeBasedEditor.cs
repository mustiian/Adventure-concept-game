using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NodeBasedEditor : EditorWindow
{

    List<Rect> windows = new List<Rect>();
    List<int> windowsToAttach = new List<int>();
    List<int> attachedWindows = new List<int>();

    Vector2 attachedRectOffset;

    bool mouseDragStart = false;
    bool mouseDragEnd = false;

    Vector2 scrollPos = Vector2.zero;

    [MenuItem("Window/Test Node editor")]
    static void ShowEditor()
    {
        NodeBasedEditor editor = EditorWindow.GetWindow<NodeBasedEditor>();


        if (Selection.activeGameObject)
        {

        }
    }

    GUIStyle nodeStyle
    {
        get
        {
            GUIStyle style = new GUIStyle(GUI.skin.window);
            style.overflow = new RectOffset(-20, -20, 0, 0);
            return style;
        }
    }

    void OnGUI()
    {
        Rect workArea = new Rect(0, 0, this.position.width - 20, this.position.height - 20);
        var viewSize = new Rect(0, 0, 5000, 5000);

        BeginWindows();

        scrollPos = GUI.BeginScrollView(workArea, scrollPos, viewSize, true, true);

        for (int i = 0; i < windows.Count; i++)
        {
            windows[i] = GUI.Window(i, windows[i], DrawNodeWindow, "Dialogue" + i, nodeStyle);
        }

        if (attachedWindows.Count >= 2)
        {
            for (int i = 0; i < attachedWindows.Count; i += 2)
            {
                DrawNodeCurve(windows[attachedWindows[i]], windows[attachedWindows[i + 1]]);
            }
        }

        if (GUI.Button(new Rect(0, 0, 80, 30), "Create Node") && Selection.activeTransform)
        {
            windows.Add(new Rect(10, 10, 140, 140));
            GameObject newWindow = new GameObject("Dialogue" + windows.Count);
            newWindow.transform.parent = Selection.activeTransform;
            newWindow.AddComponent<Dialogue>();
        }

        EndWindows();

        if (windowsToAttach.Count == 2)
        {
            attachedWindows.Add(windowsToAttach[0]);
            attachedWindows.Add(windowsToAttach[1]);
            windowsToAttach = new List<int>();
            mouseDragStart = false;
            mouseDragEnd = false;
        }

        if (windowsToAttach.Count == 1)
        {
            var rectCenter = windows[windowsToAttach[0]].center;
            var rectSize = new Vector2(50, 30);

            if (mouseDragStart)
                DrawNodeCurveToMouse(new Rect(rectCenter + attachedRectOffset, rectSize), Event.current.mousePosition);
            else if (mouseDragEnd)
                DrawNodeCurveToMouse(new Rect(rectCenter + attachedRectOffset, rectSize), Event.current.mousePosition);
        }

        GUI.EndScrollView();
    }


    void DrawNodeWindow(int id)
    {

        if (GUI.Button(new Rect(70, 20, 50, 30), "X") && !mouseDragStart)
        {
            windowsToAttach.Add(id);
            attachedRectOffset = Vector2.right * 20;
            mouseDragStart = true;
        }

        if (GUI.Button(new Rect(20, 60, 50, 30), "O") && !mouseDragEnd)
        {
            windowsToAttach.Add(id);
            attachedRectOffset = Vector2.left * 30;
            mouseDragEnd = true;
        }

        GUI.DragWindow();
    }

    private void Update()
    {
        Repaint();
    }

    void DrawNodeCurveToMouse(Rect start, Vector2 mousePosition)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(mousePosition.x, mousePosition.y, 0);
        Vector3 startTan = startPos + Vector3.right * 25;
        Vector3 endTan = endPos + Vector3.left * 25;

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.green, null, 2);
    }

    void DrawNodeCurve(Rect start, Rect end)
    {
        Vector3 startPos = new Vector3(start.x + start.width, start.y + start.height / 2, 0);
        Vector3 endPos = new Vector3(end.x, end.y + end.height / 2, 0);
        Vector3 startTan = startPos + Vector3.right * 25;
        Vector3 endTan = endPos + Vector3.left * 25;

        Handles.DrawBezier(startPos, endPos, startTan, endTan, Color.black, null, 2);
    }

    private void OnSelectionChange()
    {
        Debug.Log(Selection.activeGameObject.name);
    }
}
