namespace VendingMachines
{
    public class MaxHealthVendingMachine : VendingMachine
    {
        public override float CurrentStat { get => player.MaxHealth; }

        protected override void Sell()
        {
            player.IncreaseMaxHealth();
        }
    }
}
