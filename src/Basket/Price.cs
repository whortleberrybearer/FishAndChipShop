namespace FishAndChipShop.Basket
{
    using System;

    public readonly struct Price
    {
        private readonly decimal value;

        public Price(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value), value, "Must be greater or equal to 0.");
            }

            this.value = value;
        }

        public static implicit operator decimal(Price price) => price.value;

        public static bool operator ==(Price left, Price right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Price left, Price right)
        {
            return !(left == right);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Price price)
            {
                return this.Equals(price);
            }

            return this.value.Equals(obj);
        }

        public bool Equals(Price other)
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
