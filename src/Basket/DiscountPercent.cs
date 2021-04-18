namespace FishAndChipShop.Basket
{
    using System;

    public struct DiscountPercent
    {
        private readonly int value;

        public DiscountPercent(int value)
        {
            if (value < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be greater than 0.");
            }

            if (value > 100)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be less than 100.");
            }

            this.value = value;
        }

        public static implicit operator int(DiscountPercent discountPercent) => discountPercent.value;

        public static bool operator ==(DiscountPercent left, DiscountPercent right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DiscountPercent left, DiscountPercent right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is DiscountPercent discountPercent)
            {
                return this.Equals(discountPercent);
            }

            return this.value.Equals(obj);
        }

        public bool Equals(DiscountPercent other)
        {
            return this.value.Equals(other.value);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        public override string ToString()
        {
            return this.value.ToString();
        }
    }
}
