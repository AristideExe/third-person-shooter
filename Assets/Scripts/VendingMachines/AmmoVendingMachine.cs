using UnityEngine;

namespace VendingMachines
{
    public class AmmoVendingMachine : VendingMachine
    {
        public override float CurrentStat { get => 240; }

        protected override void Sell()
        {
            player.RefillAmmo();
        }
    }
}
