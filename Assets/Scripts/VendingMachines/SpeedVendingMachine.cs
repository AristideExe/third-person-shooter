namespace VendingMachines
{
    public class SpeedVendingMachine : VendingMachine
    {
        public override float CurrentStat { get => player.Speed; }

        protected override void Sell()
        {
            player.IncreaseSpeed();
        }
    }
}