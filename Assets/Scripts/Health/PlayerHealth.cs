//CREDITS: Arthur Scheibe
//Beyioku Daniel

using UnityEngine;

namespace Assets.Scripts.Health
{
    public class PlayerHealth : MonoBehaviour
    {
        public int maxHealth = 10;
        public int currentHealth;

        // Start is called before the first frame update
        void Start()
        {
            currentHealth = maxHealth;

        }

        // Update is called once per frame
        void Update()
        {

            // To be edited out (just for testing purposes for now)
            if (Input.GetKeyDown(KeyCode.P))
            {
                TakeDamage(2);
            }
        }

        public void TakeDamage(int damage)
        {
              currentHealth -= damage;
            if(currentHealth<=0){
                
                KillPlayer();

            }
           

           
        }

        public void KillPlayer()
        {
            
            
                this.gameObject.GetComponent<Player>().Death();
                currentHealth = maxHealth;
            
        }
    }
}