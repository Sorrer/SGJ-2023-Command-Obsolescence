using System;
using System.Collections;
using UnityEngine;

namespace Game.Economy
{
    public class IncomeBehaviour : MonoBehaviour
    {
        public int amount;
        public int interval;
        
        private void Start()
        {
            StartCoroutine(SpawnEnemyCoroutine());
        }

        private IEnumerator SpawnEnemyCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(interval);
                Bank.Instance.AddToBalance(amount);
            }
        }
    }
}