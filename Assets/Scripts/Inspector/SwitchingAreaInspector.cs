using UnityEditor;

[CustomEditor(typeof(SwitchingArea))]
public class SwitchingAreaInspector : Editor{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("Supported debugging colliders are Sphere and Box colliders. They're only required in code for debugging.", MessageType.Info);
    }
}
