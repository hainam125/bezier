using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BezierExample))]
public class DrawBezierExample : Editor
{
    private void OnSceneViewGUI(SceneView sv)
    {
        BezierExample be = target as BezierExample;

        be.startPoint.position = Handles.PositionHandle(be.startPoint.position, Quaternion.identity);
        be.endPoint.position = Handles.PositionHandle(be.endPoint.position, Quaternion.identity);
        be.startTangent.position = Handles.PositionHandle(be.startTangent.position, Quaternion.identity);
        be.endTangent.position = Handles.PositionHandle(be.endTangent.position, Quaternion.identity);

        Handles.DrawBezier(be.startPoint.position, be.endPoint.position, be.startTangent.position, be.endTangent.position, Color.red, null, 2f);
    }

    void OnEnable()
    {
        Debug.Log("OnEnable");
        SceneView.onSceneGUIDelegate += OnSceneViewGUI;
    }

    void OnDisable()
    {
        Debug.Log("OnDisable");
        SceneView.onSceneGUIDelegate -= OnSceneViewGUI;
    }
}