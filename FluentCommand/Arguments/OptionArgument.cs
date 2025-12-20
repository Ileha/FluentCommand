using EnsureThat;

namespace FluentCommand.Arguments;

public class OptionArgument : CommandArgument
{
    private readonly string _key;
    public override int GroupNumber => 0;

    public OptionArgument(int addedIndex, string key)
        : base(addedIndex)
    {
        Ensure.That(key, nameof(key)).IsNotNullOrWhiteSpace();
        _key = key;
    }

    public override string ToString()
    {
        // Options without values don't need escaping
        return _key;
    }
}