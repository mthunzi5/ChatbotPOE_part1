using CybersecurityAwarenessBot.Services;
using CybersecurityAwarenessBot.Models;

namespace CybersecurityAwarenessBot;

public class ChatbotApp
{
    private readonly AudioService _audioService = new();
    private readonly AsciiArtService _asciiArtService = new();
    private readonly TaskService _taskService = new();
    private readonly ActivityLogger _activityLogger = new();
    private readonly ResponseService _responseService;
    private readonly UserContext _context = new();

    public ChatbotApp()
    {
        _responseService = new ResponseService(_taskService, _activityLogger);
    }

    public void Run()
    {
        Console.Title = "Cybersecurity Awareness Assistant";
        ConsoleStyler.ShowHeader(_asciiArtService.LoadHeaderArt());

        _audioService.PlayGreeting();

        _context.UserName = PromptForName();

        ConsoleStyler.WriteBotLine($"Welcome, {_context.UserName}! I am your Cybersecurity Awareness Assistant.");
        ConsoleStyler.WriteBotLine("Ask me about phishing, passwords, and safe browsing.");
        ConsoleStyler.WriteBotLine("Type 'help' for examples, or 'exit' to quit.");
        ConsoleStyler.DrawDivider();

        while (true)
        {
            ConsoleStyler.WritePrompt($"{_context.UserName}> ");
            var input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input))
            {
                ConsoleStyler.WriteWarning("I didn't quite understand that. Please enter a question.");
                continue;
            }

            if (IsExitCommand(input))
            {
                ConsoleStyler.WriteBotLine($"Goodbye, {_context.UserName}. Stay cyber safe.");
                break;
            }

            var response = _responseService.GetResponse(input, _context);
            ConsoleStyler.WriteBotLine(response);
        }
    }

    private static string PromptForName()
    {
        while (true)
        {
            ConsoleStyler.WritePrompt("Please enter your name: ");
            var name = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(name))
            {
                return name;
            }

            ConsoleStyler.WriteWarning("Name cannot be empty. Try again.");
        }
    }

    private static bool IsExitCommand(string input)
    {
        var normalized = input.Trim().ToLowerInvariant();
        return normalized is "exit" or "quit" or "bye";
    }
}