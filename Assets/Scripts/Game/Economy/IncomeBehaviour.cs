using System;
using System.Collections;
using UnityEngine;

namespace Game.Economy
{
    public class IncomeBehaviour : MonoBehaviour
    {
        public int Amount;
        public float Interval;
        public float Duration;

        private float _duration;
        
        private void Start()
        {
            _duration = Duration;
            StartCoroutine(EarnIncome());
        }

        private IEnumerator EarnIncome()
        {
            while (_duration >= Interval)
            {
                yield return new WaitForSeconds(Interval);
                _duration -= Interval;
                Bank.Instance.AddToBalance(Amount);
            }
        }
        
        
    }
}