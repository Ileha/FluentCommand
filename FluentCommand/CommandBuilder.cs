using EnsureThat;
using FluentCommand.Arguments;

namespace FluentCommand;

public class CommandBuilder
{
    private readonly List<CommandArgument> _arguments = new();
    private int _argumentIndex;
    private string? _command;

    public static implicit operator string(CommandBuilder builder)
    {
        return builder.ToString();
    }

    public CommandBuilder SetCommandName(string? name)
    {
        _command = name;

        return this;
    }

    public CommandBuilder AddVerb(string verb)
    {
        Ensure.That(verb, nameof(verb)).IsNotNullOrWhiteSpace();

        _arguments.Add(new Verb(_argumentIndex, verb));

        _argumentIndex += 1;
        return this;
    }

    public CommandBuilder AddValue(params string?[]? values)
    {
        if (values is null)
            return this;

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
                continue;

            _arguments.Add(new ValueArgument(_argumentIndex, value));

            _argumentIndex += 1;

            return this;
        }

        return this;
    }

    public CommandBuilder AddValueOption(string optionName, string? value, string? divider = " ")
    {
        Ensure.That(optionName, nameof(optionName)).IsNotNullOrWhiteSpace();

        if (string.IsNullOrWhiteSpace(value))
            return this;

        _arguments.Add(new OptionValueArgument(_argumentIndex, optionName, value, divider ?? " "));

        _argumentIndex += 1;
        return this;
    }

    public CommandBuilder AddValueOptions(IEnumerable<KeyValuePair<string, string?>>? options, string? divider = " ")
    {
        if (options is null)
            return this;

        foreach (var option in options)
            AddValueOption(option.Key, option.Value, divider);

        return this;
    }

    public CommandBuilder AddOption(string option)
    {
        if (string.IsNullOrWhiteSpace(option))
            return this;

        _arguments.Add(new OptionArgument(_argumentIndex, option));

        _argumentIndex += 1;
        return this;
    }

    public string Build()
    {
        return ToString();
    }

    public string? GetCommandName()
    {
        return _command;
    }

    public string? GetArguments()
    {
        var arguments = _arguments
            .OrderBy(arg => arg.GroupNumber)
            .ThenBy(arg => arg.AddedIndex)
            .Select(arg => arg.ToString())
            .Where(arg => !string.IsNullOrWhiteSpace(arg));

        var builtCommand = string.Join(" ", arguments);

        return string.IsNullOrWhiteSpace(builtCommand) ? null : builtCommand;
    }

    public override string ToString()
    {
        var command = GetCommandName();
        var arguments = GetArguments();

        var data = Enumerable.Empty<string?>()
            .Append(command)
            .Append(arguments)
            .Where(item => !string.IsNullOrWhiteSpace(item));

        return string.Join(" ", data);
    }
}