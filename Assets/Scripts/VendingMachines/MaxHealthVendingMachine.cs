namespace VendingMachines
{
    public class MaxHealthVendingMachine : VendingMachine
    {
        protected override void Sell()
        {
            player.IncreaseMaxHealth();
        }
    }
}
