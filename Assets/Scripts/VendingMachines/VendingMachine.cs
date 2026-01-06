using System;
using UnityEngine;

namespace VendingMachines
{
    public abstract class VendingMachine : MonoBehaviour, IInteractable
    {
        [SerializeField] protected PlayerController player;
        [SerializeField] private int price;
        [SerializeField] private float sellDelay;
        [SerializeField] private float princeIncreaseMultiplier;
        
        public int Price => price;
        public float SellDelayPercentage => Math.Clamp(_sellTimer / sellDelay, 0f, 1f);
        
        public abstract float CurrentStat
        {
            get;
        }
        
        private float _sellTimer;
        
        
        private void Update()
        {
            _sellTimer -= Time.deltaTime;
        }

        public void Interact()
        {
            if (player.Money >= price && _sellTimer <= 0f)
            {
                player.RemoveMoney(price);
                _sellTimer = sellDelay;
                price = Mathf.RoundToInt(price * princeIncreaseMultiplier);
                Sell();
            }
        }

        protected abstract void Sell();
    }
}
