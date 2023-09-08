// CREDITS: Arthur Scheibe
//Beyioku Daniel

using UnityEngine;

namespace Assets.Scripts.FSM_Enemy
{
    public class WeaponDamage : MonoBehaviour
    {
        public int damage;

        public void OnTriggerEnter(Collider col)
        {
            if(col.CompareTag("Player") && this.gameObject)
            {
                col.GetComponent<Assets.Scripts.Health.PlayerHealth>().TakeDamage(damage);
            }
        }
    }
}
