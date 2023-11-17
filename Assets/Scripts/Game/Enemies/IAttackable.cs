using UnityEngine;

namespace Game.Enemies
{
    public interface IAttackable
    {
        void Attack(int damage);
        Vector3 GetWorldPosition();
        bool isDead();
    }
}