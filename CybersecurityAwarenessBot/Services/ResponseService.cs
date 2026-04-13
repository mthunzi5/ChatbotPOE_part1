namespace CybersecurityAwarenessBot.Services;

public class ResponseService
{
    public string GetResponse(string input, string userName)
    {
        var normalized = input.Trim().ToLowerInvariant();

        if (normalized is "help" or "what can i ask" or "what can i ask you about" or "topics")
        {
            return "You can ask about: password safety, phishing scams, suspicious links, social engineering, and safe browsing habits.";
        }

        if (normalized is "how are you" or "how are you?" or "how r you")
        {
            return $"I'm alert and ready to help, {userName}. Your online safety is my priority.";
        }

        if (normalized is "what's your purpose" or "what is your purpose" or "purpose")
        {
            return "My purpose is to help South African citizens identify cyber threats and practice safer online behavior.";
        }

        if (ContainsAny(normalized, "password", "passcode", "pin"))
        {
            return "Use long, unique passwords (12+ characters), avoid reusing passwords, and enable multi-factor authentication on important accounts.";
        }

        if (ContainsAny(normalized, "phishing", "scam", "fake email", "otp", "one time pin"))
        {
            return "Phishing messages often create urgency and ask for sensitive details. Verify sender addresses, never share OTPs, and confirm requests using official channels.";
        }

        if (ContainsAny(normalized, "link", "url", "website", "browsing", "browser"))
        {
            return "Before clicking links, hover to inspect the URL, look for misspellings, and ensure websites use HTTPS. When in doubt, type the official address manually.";
        }

        if (ContainsAny(normalized, "social engineering", "impersonation", "pretending"))
        {
            return "Social engineering attacks exploit trust. Pause, verify identities independently, and never act on pressure-based requests for money or private data.";
        }

        return "I didn't quite understand that. Could you rephrase? Try asking about phishing, passwords, or safe browsing.";
    }

    private static bool ContainsAny(string input, params string[] keywords)
    {
        return keywords.Any(input.Contains);
    }
}