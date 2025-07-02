using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AliasesCommand : ICommand
{
    public string Name => "aliases";
    public string Description => "Shows all available aliases for a specific command";
    public List<string> Aliases => new List<string> { "alias", "al" };

    private readonly CommandConsole console;

    public AliasesCommand(CommandConsole console)
    {
        this.console = console;
    }

    public void Execute(string[] parameters)
    {
        if (parameters.Length == 0)
        {
            console.LogToConsole("Use case: aliases <command>");
            console.LogToConsole("Example: aliases help");
            return;
        }

        string commandName = parameters[0].ToLower();
        var command = console.GetCommand(commandName);
        if (command != null)
        {
            if (command.Aliases.Count > 0)
            {
                console.LogToConsole($"Aliases for '{command.Name}': {string.Join(", ", command.Aliases)}");
            }
            else
            {
                console.LogToConsole($"The command '{command.Name}' doesn't have any aliases.");
            }
        }
        else
        {
            console.LogToConsole($"Command '{commandName}' not found.");
        }
    }
}