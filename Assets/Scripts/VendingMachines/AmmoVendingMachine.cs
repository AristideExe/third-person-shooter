using UnityEngine;

namespace VendingMachines
{
    public class AmmoVendingMachine : VendingMachine
    {
        protected override void Sell()
        {
            player.RefillAmmo();
        }
    }
}
