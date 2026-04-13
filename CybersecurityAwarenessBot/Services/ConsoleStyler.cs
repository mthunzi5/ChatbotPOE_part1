namespace CybersecurityAwarenessBot.Services;

public static class ConsoleStyler
{
    public static void ShowHeader(string asciiArt)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(new string('=', 72));
        Console.WriteLine("      CYBERSECURITY AWARENESS ASSISTANT - SOUTH AFRICA EDITION");
        Console.WriteLine(new string('=', 72));
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(asciiArt);
        Console.ResetColor();
        DrawDivider();
    }

    public static void WriteBotLine(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Bot: ");
        Console.ResetColor();
        TypeText(text, 10);
        Console.WriteLine();
    }

    public static void WriteWarning(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Warning: {text}");
        Console.ResetColor();
    }

    public static void WritePrompt(string prompt)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write(prompt);
        Console.ResetColor();
    }

    public static void DrawDivider()
    {
        Console.ForegroundColor = ConsoleColor.DarkCyan;
        Console.WriteLine(new string('-', 72));
        Console.ResetColor();
    }

    private static void TypeText(string text, int delayMs)
    {
        foreach (var character in text)
        {
            Console.Write(character);
            Thread.Sleep(delayMs);
        }
    }
}