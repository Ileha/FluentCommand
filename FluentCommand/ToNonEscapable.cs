using EnsureThat;

namespace FluentCommand;

public class ToNonEscapable
{
    private readonly string _command;

    public ToNonEscapable(string command)
    {
        Ensure.That(command, nameof(command)).IsNotNullOrWhiteSpace();

        _command = command;
    }

    public static implicit operator string(ToNonEscapable builder)
    {
        Ensure.That(builder, nameof(builder)).IsNotNull();

        return builder.ToString();
    }

    public override string ToString()
    {
        var base64Command = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(_command));
        return $"$(echo {base64Command} | base64 -d)";
    }
}