namespace CybersecurityAwarenessBot.Services;

public class AsciiArtService
{
    public string LoadHeaderArt()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "Assets", "ascii-logo.txt");

        if (File.Exists(path))
        {
            return File.ReadAllText(path);
        }

        return @"
   ____      _                ____        _
  / ___|   _| |__   ___ _ __ | __ )  ___ | |_
 | |  | | | | '_ \ / _ \ '__||  _ \ / _ \| __|
 | |__| |_| | |_) |  __/ |   | |_) | (_) | |_
  \____\__, |_.__/ \___|_|   |____/ \___/ \__|
       |___/
";
    }
}