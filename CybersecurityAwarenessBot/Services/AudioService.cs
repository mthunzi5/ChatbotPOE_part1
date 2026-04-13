using System.Media;

namespace CybersecurityAwarenessBot.Services;

public class AudioService
{
    public void PlayGreeting()
    {
        if (!OperatingSystem.IsWindows())
        {
            ConsoleStyler.WriteWarning("Audio greeting is only supported on Windows. Continuing without audio.");
            return;
        }

        var path = Path.Combine(AppContext.BaseDirectory, "Assets", "welcome.wav");

        if (!File.Exists(path))
        {
            ConsoleStyler.WriteWarning("Voice greeting file not found. Continuing without audio.");
            return;
        }

        try
        {
            using var player = new SoundPlayer(path);
            player.Load();
            player.PlaySync();
        }
        catch
        {
            ConsoleStyler.WriteWarning("Unable to play voice greeting. Continuing with text mode.");
        }
    }
}