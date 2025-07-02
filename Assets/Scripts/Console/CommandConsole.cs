using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CommandConsole : MonoBehaviour
{
    [SerializeField] private GameObject consolePanel;
    [SerializeField] private TextMeshProUGUI logText;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button executeButton;
    [SerializeField] private InputActionReference openCloseInput;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private int maxLogEntries = 100;

    private Dictionary<string, ICommand> commands = new Dictionary<string, ICommand>();
    private Dictionary<string, string> aliases = new Dictionary<string, string>();
    private List<string> logEntries = new List<string>();
    private bool isConsoleOpen = false;
    private ConsoleLogHandler customLogHandler;

    private void Awake()
    {
        InitializeUI();
        RegisterCommands();
        SetupLogHandler();
    }

    private void Start()
    {
        SetConsoleVisibility(false);
        LogToConsole("=== Command Console Initialized ===");
        LogToConsole("Press F1 to Open/Close the Console");
        LogToConsole("Type help to check out all available commands!");
    }
    
    private void OnDestroy()
    {
        if (customLogHandler != null)
        {
            customLogHandler.RestoreOriginalHandler();
        }
    }

    private void InitializeUI()
    {
        if (executeButton != null)
            executeButton.onClick.AddListener(ExecuteCommand);
        
        openCloseInput.action.performed += ToggleConsole;

        if (inputField != null)
        {
            inputField.onSubmit.AddListener(_ => ExecuteCommand());
        }
    }

    private void RegisterCommands()
    {
        RegisterCommand(new HelpCommand(this));
        RegisterCommand(new AliasesCommand(this));
        RegisterCommand(new PlayAnimationCommand(this));
    }

    private void SetupLogHandler()
    {
        customLogHandler = new ConsoleLogHandler(this);
        Debug.unityLogger.logHandler = customLogHandler;
    }

    public void RegisterCommand(ICommand command)
    {
        if (command == null) return;

        string commandName = command.Name.ToLower();
        commands[commandName] = command;

        // Register Aliases
        foreach (string alias in command.Aliases)
        {
            aliases[alias.ToLower()] = commandName;
        }

        Debug.Log($"Command Registered: {command.Name}");
    }

    public ICommand GetCommand(string commandName)
    {
        commandName = commandName.ToLower();
        
        // Search command directly
        if (commands.ContainsKey(commandName))
            return commands[commandName];
        
        // Search by alias
        if (aliases.ContainsKey(commandName))
            return commands[aliases[commandName]];
        
        return null;
    }

    public IEnumerable<ICommand> GetAllCommands()
    {
        return commands.Values;
    }

    private void ExecuteCommand()
    {
        if (inputField == null || string.IsNullOrWhiteSpace(inputField.text))
            return;

        string input = inputField.text.Trim();
        LogToConsole($"> {input}");

        // Parse Command
        string[] parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0) return;

        string commandName = parts[0];
        string[] parameters = parts.Skip(1).ToArray();

        // Exec Command
        var command = GetCommand(commandName);
        if (command != null)
        {
            try
            {
                command.Execute(parameters);
            }
            catch (Exception ex)
            {
                LogToConsole($"Error executing command: {ex.Message}", LogType.Error);
            }
        }
        else
        {
            LogToConsole($"Command '{commandName}' doesnt exist. Use 'help' to see all available commands.", LogType.Warning);
        }

        // Clean Input
        inputField.text = "";
        inputField.ActivateInputField();
    }

    public void ToggleConsole(InputAction.CallbackContext callbackContext)
    {
        SetConsoleVisibility(!isConsoleOpen);
    }

    private void SetConsoleVisibility(bool visible)
    {
        isConsoleOpen = visible;
        if (consolePanel != null)
        {
            consolePanel.SetActive(visible);
        }

        if (visible && inputField != null)
        {
            inputField.ActivateInputField();
        }
    }

    public void LogToConsole(string message, LogType logType = LogType.Log)
    {
        string timestamp = DateTime.Now.ToString("HH:mm:ss");
        string formattedMessage = $"[{timestamp}] {message}";
        
        logEntries.Add(formattedMessage);
        
        // Entry Limit Counter
        if (logEntries.Count > maxLogEntries)
        {
            logEntries.RemoveAt(0);
        }

        UpdateLogDisplay();
    }

    private void UpdateLogDisplay()
    {
        if (logText != null)
        {
            logText.text = string.Join("\n", logEntries);
            
            // Auto-scroll
            if (scrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                scrollRect.verticalNormalizedPosition = 0f;
            }
        }
    }
}