# Fluent Command

[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

A fluent .NET library for building command-line commands with ease. Designed with a clean, intuitive API that makes generating complex CLI commands simple and maintainable.

## Quick Start

### Basic Usage

```csharp
// Create a simple command
var command = new CommandBuilder()
    .SetCommandName("git")
    .AddVerb("clone")
    .AddValueOption("--branch", "main")
    .AddOption("--quiet")
    .AddValue("https://github.com/user/repo.git");

// Get the command string
string commandLine = command.ToString();
// git clone --branch main --quiet https://github.com/user/repo.git
```

### Complex Example

Here's how to generate a more complex Git command:

```bash
git clone \
  --branch release/v1.2 \
  --depth 10 \
  --single-branch \
  --recurse-submodules \
  --shallow-submodules \
  --config core.eol=lf \
  --config fetch.prune=true \
  https://github.com/user/repo.git \
  ./repo
```

With FluentCommand:

```csharp
string command = new CommandBuilder()
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
    .AddValue("./repo")
    .ToString();
```

## API Reference

### Core Methods

- `SetCommandName(string commandName)` - Sets the name of the command to execute
- `AddVerb(string verb)` - Adds a command verb (e.g., `clone` in `git clone`)
- `AddOption(string option)` - Adds a flag/switch option (e.g., `--force`)
- `AddValueOption(string option, string value, string delimiter = " ")` - Adds an option with a value (e.g., `--branch main`)
- `AddValue(string value)` - Adds a positional argument
- `Build()` - Builds and returns the command string
- `ToString()` - Alias for `Build()`

### Advanced Features

#### Nested Commands
FluentCommand automatically escapes special characters needed for more complex use cases:
```csharp
var nestedCommand = new CommandBuilder()
    .SetCommandName("echo")
    .AddValue("hello world");

var command = new CommandBuilder()
    .SetCommandName("bash")
    .AddValueOption("-c", nestedCommand.Build())
    .ToString();
// bash -c "echo \"hello world\""
```

## License

Distributed under the MIT License. See `LICENSE` for more information.