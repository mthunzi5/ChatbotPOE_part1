namespace CybersecurityAwarenessBot.Models;

public class UserContext
{
    public string UserName { get; set; } = string.Empty;
    public string Mood { get; set; } = string.Empty;
    public string LastTopic { get; set; } = string.Empty;
    public Dictionary<string, string> Memory { get; } = new(StringComparer.OrdinalIgnoreCase);
}
