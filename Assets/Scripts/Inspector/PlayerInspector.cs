// CREDITS: Medyan Mehiddine

using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerMovement))]
public class PlayerInspector : Editor{
    public override void OnInspectorGUI(){
        PlayerMovement player = (PlayerMovement)target;

        if(GUILayout.Button("Update jump values (In Play Mode)")){
            player.UpdateJumpVelocity();
        }

        DrawDefaultInspector();
    }
}
