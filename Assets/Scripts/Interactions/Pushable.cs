// CREDITS:
// Beyioku Daniel

using UnityEngine;

public class Pushable : Interactable
{
    [Header("DEBUGGING")]
    [SerializeField] bool debugging;

    private float _mass;

    public void Push()
    {
        if(CharacterSwitch.ActiveCharacter != CharacterSwitch.Gobro)
            return;
        
        if(Logger.GlobalDebugging && debugging)
            UnityEngine.Debug.Log($"<color=white>Player is pushing:</color> <color=red>" + gameObject.name + "</color>");

        _mass = this.GetComponent<Rigidbody>().mass;
        this.GetComponent<Rigidbody>().mass = 1;

        gameObject.AddComponent<FixedJoint>();
        this.GetComponent<FixedJoint>().connectedBody = GameObject.FindGameObjectWithTag(playerTag).GetComponent<Rigidbody>();
    }

    public void Leave()
    {
        if(CharacterSwitch.ActiveCharacter != CharacterSwitch.Gobro)
            return;

        if(Logger.GlobalDebugging && debugging)
            UnityEngine.Debug.Log($"<color=white>Player stopped pushing:</color> <color=blue>" + gameObject.name + "</color>");

        this.GetComponent<Rigidbody>().mass = _mass;

        this.GetComponent<FixedJoint>().connectedBody = null;
        Destroy(GetComponent<FixedJoint>());
    }
}