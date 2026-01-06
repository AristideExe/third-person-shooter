using UnityEngine;

namespace VendingMachines
{
    public class DamagesVendingMachine : VendingMachine
    {
        protected override void Sell()
        {
            player.IncreaseDamageMultiplier();
        }
    }
}
