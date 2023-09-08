using Assets.Scripts.FSM_Enemy;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public EnemyState currentState;
    public FieldOfView fov;

    [SerializeField] EnemyPatrol enemyPatrolState;

    void Start(){
        currentState = enemyPatrolState;
    }

    void Update(){
        // currentState.RunState();
        RunCurrentState();
    }

    private void RunCurrentState()
    {
        // The following code seems to be pointless
        // but i kept it just in case


        EnemyState nextState = currentState?.RunState();

        if(nextState != null)
        {
            SwitchToNextState(nextState);
        }

    }

    private void SwitchToNextState(EnemyState nextState)
    {
        currentState = nextState;
    }
}
