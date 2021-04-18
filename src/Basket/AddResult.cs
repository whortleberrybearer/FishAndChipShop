namespace FishAndChipShop.Basket
{
    public readonly struct AddResult
    {
        public AddResult(bool success)
        {
            this.Success = success;
        }

        public static AddResult Succeeded { get; } = new AddResult(true);

        public bool Success { get; }
    }
}
