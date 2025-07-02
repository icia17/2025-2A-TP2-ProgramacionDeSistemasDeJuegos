using System.Collections.Generic;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
    List<string> Aliases { get; }
    void Execute(string[] parameters);
}