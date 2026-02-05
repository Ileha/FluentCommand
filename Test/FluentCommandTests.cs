using FluentCommand;
using FluentCommand.Arguments;

namespace Test;

[TestFixture]
public class FluentCommandTests
{
    [Test]
    public void ComplicatedCommand_ShouldCreateCommand()
    {
        // Arrange
        var command = new CommandBuilder()
            .SetCommandName("git")
            .AddVerb("clone")
            .AddValueOption("--branch", "release/v1.2")
            .AddValueOption("--depth", "10")
            .AddOption("--single-branch")
            .AddOption("--recurse-submodules")
            .AddOption("--shallow-submodules")
            .AddValueOption("--config", "core.eol=lf")
            .AddValueOption("--config", "fetch.prune=true")
            .AddValue("https://github.com/user/repo.git")
            .AddValue("./repo");

        // Act
        var result = command.ToString();

        // Assert
        Assert.That(result,
            Is.EqualTo(
                "git clone --branch release/v1.2 --depth 10 --single-branch --recurse-submodules --shallow-submodules --config core.eol=lf --config fetch.prune=true https://github.com/user/repo.git ./repo"));
    }

    [Test]
    public void SetVerb_Should_CreateCommand()
    {
        // Arrange
        var command = new CommandBuilder()
            .SetCommandName("git")
            .AddVerb("clone")
            .AddValueOption("--test", "test")
            .AddOption("--no-auth")
            .AddValue("path");

        // Act
        var result = command.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("git clone --test test --no-auth path"));
    }

    [Test]
    public void ValueArgument_ShouldEscapeSpecialCharacters()
    {
        // Arrange
        var value = "test value with spaces and \"quotes\" and 'single quotes'";
        var arg = new ValueArgument(0, value);

        // Act
        var result = arg.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("\"test value with spaces and \\\"quotes\\\" and \\'single quotes\\'\""));
    }

    [Test]
    public void ValueArgument_ShouldHandleSimpleValue()
    {
        // Arrange
        var value = "simplevalue";
        var arg = new ValueArgument(0, value);

        // Act
        var result = arg.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("simplevalue"));
    }

    [Test]
    public void OptionValueArgument_ShouldEscapeValuePart()
    {
        // Arrange
        var key = "-e";
        var value = "key=value with spaces";
        var join = " ";
        var arg = new OptionValueArgument(0, key, value, join);

        // Act
        var result = arg.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("-e \"key=value with spaces\""));
    }

    [Test]
    public void OptionValueArgument_ShouldHandleSimpleKeyAndValue()
    {
        // Arrange
        var key = "-v";
        var value = "simplevalue";
        var join = " ";
        var arg = new OptionValueArgument(0, key, value, join);

        // Act
        var result = arg.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("-v simplevalue"));
    }

    [Test]
    public void OptionArgument_ShouldReturnKeyAsIs()
    {
        // Arrange
        var key = "-f";
        var arg = new OptionArgument(0, key);

        // Act
        var result = arg.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("-f"));
    }

    [Test]
    public void OptionArgument_ShouldHandleLongOption()
    {
        // Arrange
        var key = "--verbose";
        var arg = new OptionArgument(0, key);

        // Act
        var result = arg.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("--verbose"));
    }

    [Test]
    public void Escape_ShouldHandleEmptyString()
    {
        // Assert
        Assert.Throws<ArgumentException>(() =>
        {
            //Arrange & Act
            var valueArgument = new ValueArgument(0, "");
        });
    }

    [Test]
    public void Escape_ShouldHandleAllSpecialCharacters()
    {
        // Arrange
        var value = "\"'\n\r\t\\";
        var arg = new ValueArgument(0, value);

        // Act
        var result = arg.ToString();

        // Assert
        Assert.That(result, Is.EqualTo("\"\\\"\\'\n\r\t\\\\\""));
    }

    [Test]
    public void Test_NestedCommandEscaping()
    {
        // Arrange
        var innerCommand = new CommandBuilder()
            .SetCommandName("echo")
            .AddValue("hello world");

        var middleCommand = new CommandBuilder()
            .SetCommandName("bash")
            .AddValueOption("-c", innerCommand.Build());

        var outerCommand = new CommandBuilder()
            .SetCommandName("bash")
            .AddValueOption("-c", $"udocker run container {middleCommand.Build()}");

        // Act
        var result = outerCommand.Build();

        // Assert
        Assert.That(result,
            Is.EqualTo("bash -c \"udocker run container bash -c \\\"echo \\\\\\\"hello world\\\\\\\"\\\"\""));
    }

    [Test]
    public void Test_BuildFluentCommand()
    {
        var environmentVariables = new Dictionary<string, string>
        {
            {"data", "Development"},
            {"product", "Production"}
        };

        var bashBuilder = new CommandBuilder()
            .SetCommandName("bash")
            .AddValueOption("-c", "apt-get update && apt-get install -y curl sudo");

        var commandBuilder = new CommandBuilder()
            .SetCommandName("run")
            .AddValueOptions(environmentVariables?.Select(kv =>
                new KeyValuePair<string, string?>("-e", $"{kv.Key}={kv.Value}")))
            .AddValueOption("--user", "user", "=")
            .AddValueOption("--workdir", null)
            .AddValue("container");

        var result = $"{commandBuilder} {bashBuilder}";

        Assert.That(result,
            Is.EqualTo(
                "run -e data=Development -e product=Production --user=user container bash -c \"apt-get update && apt-get install -y curl sudo\""));
    }
}