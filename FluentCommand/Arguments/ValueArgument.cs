using EnsureThat;

namespace FluentCommand.Arguments;

public class ValueArgument : CommandArgument
{
    private readonly string _value;
    public override int GroupNumber => 1;

    public ValueArgument(int addedIndex, string value)
        : base(addedIndex)
    {
        Ensure.That(value, nameof(value)).IsNotNullOrWhiteSpace();
        _value = value;
    }

    public override string ToString()
    {
        return Escape(_value);
    }
}