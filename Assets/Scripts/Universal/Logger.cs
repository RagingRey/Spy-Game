// CREDITS: Medyan Mehiddine

using UnityEngine;

public class Logger : MonoBehaviour
{
    [Tooltip("Enabled: Allows debugging for all scripts \nDisabled: Disables debugging for all scripts \n(This cannot be changed during runtime)")]
    public bool globalDebugging = true;
    public static bool GlobalDebugging;

    private void Start() {
        GlobalDebugging = globalDebugging;        
    }    
}
