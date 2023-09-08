using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyPatrol))]
public class EnemyPatrolEditor : Editor
{
    SerializedObject so;
    SerializedProperty propPatrolPoints;

    private void OnEnable()
    {
        so = serializedObject;
        propPatrolPoints = so.FindProperty("wayPoints");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        /*so.Update();
        EditorGUILayout.PropertyField(propPatrolPoints);
        so.ApplyModifiedProperties();*/
    }

    void OnSceneGUI()
    {
        /*so.Update();
        for (int i = 0; i < propPatrolPoints.arraySize; i++)
        {
            SerializedProperty prop = propPatrolPoints.GetArrayElementAtIndex(i);
            prop.vector3Value = Handles.PositionHandle(prop.vector3Value, Quaternion.identity);
        }
        so.ApplyModifiedProperties();*/

        EnemyPatrol enemyPatrol = (EnemyPatrol)target;

        for (int i = 0; i < enemyPatrol.patrolPoints.Length; i++)
        {
            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(enemyPatrol.patrolPoints[i].position, Quaternion.identity);
            Handles.Label(newPos, "Patrol Point " + (i + 1).ToString());
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(enemyPatrol, "Change Patrol Point Position");
                enemyPatrol.patrolPoints[i].position = newPos;
                so.Update();
            }
        }
        
        Handles.DrawDottedLine(enemyPatrol.transform.position, enemyPatrol.patrolPoints[0].position, 3f);
        for (int j = 0; j < enemyPatrol.patrolPoints.Length; j++)
        {
            if (j < enemyPatrol.patrolPoints.Length - 1)
            {
                Handles.DrawDottedLine(enemyPatrol.patrolPoints[j].position, enemyPatrol.patrolPoints[j + 1].position, 3f);
            }
            else
            {
                Handles.DrawDottedLine(enemyPatrol.patrolPoints[j].position, enemyPatrol.patrolPoints[0].position, 3f);
            }
        }

        EditorGUI.BeginChangeCheck();
        Vector3 newEnemyPos = Handles.PositionHandle(enemyPatrol.transform.position, Quaternion.identity);
        Handles.Label(newEnemyPos, "Enemy");
        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(enemyPatrol, "Change Enemy Position");
            enemyPatrol.transform.position = newEnemyPos;
            so.Update();
        }
    }
}
