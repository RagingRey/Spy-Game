using System;
using UnityEngine;
using UnityEngine.UI;

public class LockOnUIIndicator : MonoBehaviour{
    [SerializeField] Image indicator;

    private void Update() {
        if(EnemyLockOn.LOCKED_ENEMY == null){
            indicator.enabled = false;
            return;
        }
        else{
            indicator.enabled = true;
        }

        SetPosition();
    }

    void SetPosition(){
        float minX = indicator.GetPixelAdjustedRect().width/2f;
        float maxX = Screen.width - minX;

        float minY = indicator.GetPixelAdjustedRect().height/2f;
        float maxY = Screen.height - minY;

        Vector2 position = Camera.main.WorldToScreenPoint(EnemyLockOn.LOCKED_ENEMY.transform.position);

        if(TargetIsBehindCamera()){
            position.x = position.x < Screen.width/2f ? maxX : minX;
        }

        position = ClampPosition(position, minX, maxX, minY, maxY);

        indicator.transform.position = position;
    }

    bool TargetIsBehindCamera(){
        return Vector3.Dot((EnemyLockOn.LOCKED_ENEMY.transform.position - Camera.main.transform.position), Camera.main.transform.forward) < 0;
    }

    Vector2 ClampPosition(Vector2 position, float minX, float maxX, float minY, float maxY){
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        return position;
    }
}