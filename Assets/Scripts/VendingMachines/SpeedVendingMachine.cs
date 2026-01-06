namespace VendingMachines
{
    public class SpeedVendingMachine : VendingMachine
    {
        protected override void Sell()
        {
            player.IncreaseSpeed();
        }
    }
}