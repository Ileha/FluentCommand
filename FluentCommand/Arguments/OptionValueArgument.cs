using EnsureThat;

namespace FluentCommand.Arguments;

public class OptionValueArgument : CommandArgument
{
    private readonly string _key;
    private readonly string _value;
    private readonly string _join;
    public override int GroupNumber => 0;

    public OptionValueArgument(int addedIndex, string key, string value, string join)
        : base(addedIndex)
    {
        Ensure.That(key, nameof(key)).IsNotNullOrWhiteSpace();
        Ensure.That(value, nameof(value)).IsNotNullOrWhiteSpace();
        Ensure.That(join, nameof(join)).IsNotNullOrEmpty();

        _key = key;
        _value = value;
        _join = join;
    }

    public override string ToString()
    {
        // Only escape the value part, keep key and join as is
        var escapedValue = Escape(_value);

        Ensure.That(escapedValue, nameof(escapedValue)).IsNotNullOrWhiteSpace();

        return $"{_key}{_join}{escapedValue}";
    }
}