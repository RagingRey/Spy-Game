using Assets.Scripts.Health;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider sl;

    void Update()
    {
        SetHealth();
    }

    public void SetHealth()
    {
        sl.value = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>().currentHealth;
    }
}
