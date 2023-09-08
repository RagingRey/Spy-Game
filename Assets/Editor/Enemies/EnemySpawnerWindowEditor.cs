using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemySpawnerWindowEditor : EditorWindow
{
    GameObject defaultMobPrefab;
    GameObject swordMobPrefab;
    GameObject gunMobPrefab;
    GameObject boxingMobPrefab;
    string[] enemyArray = new string[] { "Default Mob", "Sword Mob", "Gun Mob", "Boxing Mob"};
    int selectedEnemyIndex;
    
    int defaultMobIndex;
    int swordMobIndex;
    int gunMobIndex;
    int boxingMobIndex;

    const string DEFAULTMOBNAME = "DefaultMob";
    const string SWORDMOBNAME = "SwordMob";
    const string GUNMOBNAME = "GunMob";
    const string BOXINGMOBNAME = "BoxingMobName";

    Transform objectContainer;
    float spawnRadius;
    


    private void OnEnable()
    {
        defaultMobPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Mob/Default Mob.prefab");
        swordMobPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Mob/Sword Mob.prefab");
        gunMobPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Mob/Gun Mob.prefab");
        boxingMobPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Enemies/Mob/Boxing Mob.prefab");

        selectedEnemyIndex = -1;

        defaultMobIndex = 1;
        swordMobIndex = 1;
        gunMobIndex = 1;
        boxingMobIndex = 1;
        spawnRadius = 5f;
    }

    [MenuItem("Tools/Enemy Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(EnemySpawnerWindowEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Enemy Type", EditorStyles.boldLabel);
        selectedEnemyIndex = EditorGUILayout.Popup("Select an enemy type:", selectedEnemyIndex, enemyArray);
        using (new EditorGUI.DisabledScope(selectedEnemyIndex < 0))
        {
            if (selectedEnemyIndex < 0) return;
            switch (enemyArray[selectedEnemyIndex])
            {
                case "Default Mob":
                    defaultMobIndex = EditorGUILayout.IntField("Default Mob ID", defaultMobIndex);
                    objectContainer = EditorGUILayout.ObjectField("Object Parent", objectContainer, typeof(Transform), true) as Transform;
                    EditorGUILayout.HelpBox("Object parent not required", MessageType.None, false);
                    spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
                    if (GUILayout.Button("Spawn Default Mob"))
                    {
                        Collider collider = defaultMobPrefab.GetComponentInChildren<Collider>();
                        float halfHeight = collider.bounds.size.y;
                        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
                        Vector3 spawnPos = new Vector3(spawnCircle.x, halfHeight + 1.61f, spawnCircle.y);
                        string objName = "DefaultMob" + defaultMobIndex.ToString();
                        defaultMobIndex++;
                        GameObject newMob = PrefabUtility.InstantiatePrefab(defaultMobPrefab, objectContainer) as GameObject;
                        newMob.name = objName;
                        newMob.transform.position = spawnPos;
                    }
                    break;
                case "Sword Mob":
                    swordMobIndex = EditorGUILayout.IntField("Sword Mob ID", swordMobIndex);
                    objectContainer = EditorGUILayout.ObjectField("Object Parent", objectContainer, typeof(Transform), true) as Transform;
                    EditorGUILayout.HelpBox("Object parent not required", MessageType.None, false);
                    spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
                    if (GUILayout.Button("Spawn Sword Mob"))
                    {
                        Collider collider = swordMobPrefab.GetComponentInChildren<Collider>();
                        float halfHeight = collider.bounds.size.y;
                        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
                        Vector3 spawnPos = new Vector3(spawnCircle.x, halfHeight + 1.61f, spawnCircle.y);
                        string objName = "SwordMob" + swordMobIndex.ToString();
                        swordMobIndex++;
                        GameObject newMob = PrefabUtility.InstantiatePrefab(swordMobPrefab, objectContainer) as GameObject;
                        newMob.name = objName;
                        newMob.transform.position = spawnPos;
                    }
                    break;
                case "Gun Mob":
                    gunMobIndex = EditorGUILayout.IntField("Gun Mob ID", gunMobIndex);
                    objectContainer = EditorGUILayout.ObjectField("Object Parent", objectContainer, typeof(Transform), true) as Transform;
                    EditorGUILayout.HelpBox("Object parent not required", MessageType.None, false);
                    spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
                    if (GUILayout.Button("Spawn Gun Mob"))
                    {
                        Collider collider = gunMobPrefab.GetComponentInChildren<Collider>();
                        float halfHeight = collider.bounds.size.y;
                        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
                        Vector3 spawnPos = new Vector3(spawnCircle.x, halfHeight + 1.61f, spawnCircle.y);
                        string objName = "GunMob" + gunMobIndex.ToString();
                        gunMobIndex++;
                        GameObject newMob = PrefabUtility.InstantiatePrefab(gunMobPrefab, objectContainer) as GameObject;
                        newMob.name = objName;
                        newMob.transform.position = spawnPos;
                    }
                    break;
                case "Boxing Mob":
                    boxingMobIndex = EditorGUILayout.IntField("Boxing Mob ID", boxingMobIndex);
                    objectContainer = EditorGUILayout.ObjectField("Object Parent", objectContainer, typeof(Transform), true) as Transform;
                    EditorGUILayout.HelpBox("Object parent not required", MessageType.None, false);
                    spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
                    if (GUILayout.Button("Spawn Boxing Mob"))
                    {
                        Collider collider = boxingMobPrefab.GetComponentInChildren<Collider>();
                        float halfHeight = collider.bounds.size.y;
                        Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
                        Vector3 spawnPos = new Vector3(spawnCircle.x, halfHeight + 1.61f, spawnCircle.y);
                        string objName = "BoxingMob" + boxingMobIndex.ToString();
                        boxingMobIndex++;
                        GameObject newMob = PrefabUtility.InstantiatePrefab(boxingMobPrefab, objectContainer) as GameObject;
                        newMob.name = objName;
                        newMob.transform.position = spawnPos;
                    }
                    break;
            }
            
        }
        
    }
}
