using Game.Enemies;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Towers
{
    public class TowerHealth : MonoBehaviour, IAttackable
    {
        public int health;

        public UnityEvent onDeath = new UnityEvent();
        
        public void Attack(int damage)
        {
            health -= damage;

            if (isDead())
            {
                onDeath.Invoke();
            }
        }

        public Vector3 GetWorldPosition()
        {
            return this.transform.position;
        }

        public bool isDead()
        {
            return health <= 0;
        }
    }
}