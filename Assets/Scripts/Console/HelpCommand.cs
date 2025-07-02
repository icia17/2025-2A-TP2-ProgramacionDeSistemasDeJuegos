using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HelpCommand : ICommand
{
    public string Name => "help";
    public string Description => "Shows info about a specific command or shows all available commands";
    public List<string> Aliases => new List<string> { "h", "?" };

    private readonly CommandConsole console;

    public HelpCommand(CommandConsole console)
    {
        this.console = console;
    }

    public void Execute(string[] parameters)
    {
        if (parameters.Length == 0)
        {
            console.LogToConsole("=== AVAILABLE COMMANDS ===");
            foreach (var command in console.GetAllCommands())
            {
                console.LogToConsole($"• {command.Name}: {command.Description}");
                if (command.Aliases.Count > 0)
                    console.LogToConsole($"  Aliases: {string.Join(", ", command.Aliases)}");
            }
        }
        else
        {
            string commandName = parameters[0].ToLower();
            var command = console.GetCommand(commandName);
            if (command != null)
            {
                console.LogToConsole($"=== {command.Name.ToUpper()} ===");
                console.LogToConsole($"Description: {command.Description}");
                if (command.Aliases.Count > 0)
                    console.LogToConsole($"Aliases: {string.Join(", ", command.Aliases)}");
            }
            else
            {
                console.LogToConsole($"Command '{commandName}' not found. Use 'help' to see all available commands.");
            }
        }
    }
}