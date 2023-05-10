using UnityEngine;

namespace Character
{
    public class LifeControl : MonoBehaviour
    {
        public int health;

        public void DoDamage(int amount)
        {
            health -= amount;
            if(health <= 0)
                Dead();
        }

        private void Dead()
        {
            Destroy(gameObject);
        }
    }
}
