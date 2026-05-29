namespace CybersecurityAwarenessBot.Services;

public class ActivityLogger
{
    private readonly List<string> _entries = new();

    public IReadOnlyList<string> Entries => _entries.AsReadOnly();

    public void Log(string action)
    {
        var entry = $"{DateTime.Now:yyyy-MM-dd HH:mm}: {action}";
        _entries.Add(entry);

        if (_entries.Count > 10)
        {
            _entries.RemoveAt(0);
        }
    }

    public string GetRecentLog()
    {
        if (!_entries.Any())
        {
            return "No recent activity yet.";
        }

        return string.Join(Environment.NewLine, _entries.Select((entry, index) => $"{index + 1}. {entry}"));
    }
}
