using UnityEngine;

namespace VendingMachines
{
    public class DamagesVendingMachine : VendingMachine
    {
        public override float CurrentStat { get => player.DamageMultiplier; }

        protected override void Sell()
        {
            player.IncreaseDamageMultiplier();
        }
    }
}
