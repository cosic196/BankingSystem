namespace BankingSystem.Domain.ValueObjects;

public class Money
{
    public decimal Amount { get; }

    public Money(decimal amount)
    {
        if (amount < 0)
        {
            throw new ArgumentException("Money amount must be positive", nameof(amount));
        }

        Amount = amount;
    }

    public override string ToString()
    {
        return Amount.ToString();
    }

    public static implicit operator Money(decimal d)
    {
        return new Money(d);
    }

    public static implicit operator decimal(Money m)
    {
        ArgumentNullException.ThrowIfNull(m);
        return m.Amount;
    }

    public static Money operator +(Money m1, Money m2)
    {
        ArgumentNullException.ThrowIfNull(m1);
        ArgumentNullException.ThrowIfNull(m2);

        return new Money(m1.Amount + m2.Amount);
    }

    public static Money operator -(Money m1, Money m2)
    {
        ArgumentNullException.ThrowIfNull(m1);
        ArgumentNullException.ThrowIfNull(m2);

        return new Money(m1.Amount - m2.Amount);
    }

    public static bool operator ==(Money m1, Money m2)
    {
        return m1.Equals(m2);
    }

    public static bool operator !=(Money m1, Money m2)
    {
        return !m1.Equals(m2);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Money other)
        {
            return false;
        }

        return GetHashCode() == other.GetHashCode();
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Amount);
    }
}
