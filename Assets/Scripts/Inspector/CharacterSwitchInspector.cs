using UnityEditor;

[CustomEditor(typeof(CharacterSwitch))]
public class CharacterSwitchInspector : Editor{
    public override void OnInspectorGUI(){
        DrawDefaultInspector();
        EditorGUILayout.HelpBox("NOTE: The order of the characters in the array should be: Scout -> Hacker -> GoBro", MessageType.Info);
    }
}
