using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof (RescaleBox))]
public class InspectorButton : Editor
{

public override void OnInspectorGUI()

{
base.OnInspectorGUI();
if(GUILayout.Button("ResizeBox"))
{
            ((RescaleBox)target).ResizeBox();
}
}
}
