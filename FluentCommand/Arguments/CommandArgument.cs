namespace FluentCommand.Arguments;

public abstract class CommandArgument
{
    public abstract int GroupNumber { get; }
    public int AddedIndex { get; }

    public CommandArgument(int addedIndex)
    {
        AddedIndex = addedIndex;
    }

    public abstract override string ToString();

    protected static string Escape(string input)
    {
        if (string.IsNullOrEmpty(input))
            return string.Empty;

        // Characters that need escaping in command line
        var specialChars = new[] {' ', '"', '\\', '\'', '\n', '\r', '\t'};
        var needsEscaping = input.Any(c => specialChars.Contains(c));

        if (!needsEscaping)
            return input;

        // Escape backslashes first
        var escaped = input.Replace("\\", "\\\\");

        // Escape double quotes
        escaped = escaped.Replace("\"", "\\\"");

        // Escape single quotes
        escaped = escaped.Replace("'", "\\'");

        // If the string contains spaces or special characters, wrap in double quotes
        if (input.Contains(' ') || input.Contains('\t') || input.Contains('\n') || input.Contains('\r'))
            escaped = $"\"{escaped}\"";

        return escaped;
    }
}