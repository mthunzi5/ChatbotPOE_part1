using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CybersecurityAwarenessBot.Models;

namespace CybersecurityAwarenessBot.Services;

public class ResponseService
{
    private readonly TaskService _taskService;
    private readonly ActivityLogger _activityLogger;
    private readonly Random _random = new();

    private readonly Dictionary<string, List<string>> _randomResponses = new()
    {
        ["phishing"] = new List<string>
        {
            "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations.",
            "Check the sender's address carefully and never share passwords or OTPs through email.",
            "Phishing messages create urgency. If you feel pressured, pause and double-check before you act."
        }
    };

    private readonly Dictionary<string, string> _topicGuidance = new(StringComparer.OrdinalIgnoreCase)
    {
        ["password"] = "Use long, unique passwords for each account, avoid personal details, and enable multi-factor authentication.",
        ["privacy"] = "Review your privacy settings regularly and only share the minimum data required by apps or websites.",
        ["scam"] = "If a message asks for money, personal details, or OTPs, treat it as suspicious and verify the request independently.",
        ["phishing"] = "Phishing scams often look real. Pause, inspect the link, and do not click attachments from unknown senders.",
        ["social engineering"] = "Always verify identities and never act on pressure-based requests for money or account information."
    };

    private readonly Dictionary<string, string> _sentimentAdvice = new(StringComparer.OrdinalIgnoreCase)
    {
        ["worried"] = "It's completely understandable to feel that way. Let's go through a few clear tips so you can feel more confident.",
        ["curious"] = "Curiosity is a great advantage online. I can explain the safest actions to take and why they matter.",
        ["frustrated"] = "That can be frustrating. Take a deep breath and let's simplify the next step together."
    };

    public ResponseService(TaskService taskService, ActivityLogger activityLogger)
    {
        _taskService = taskService;
        _activityLogger = activityLogger;
    }

    public string GetResponse(string input, UserContext context)
    {
        var normalized = input.Trim().ToLowerInvariant();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            return "Please enter a question or instruction so I can help.";
        }

        if (TryStoreMemory(normalized, input, context, out var memoryResponse))
        {
            return memoryResponse;
        }

        if (TryCreateTaskFromInput(normalized, input, out var taskResponse))
        {
            return taskResponse;
        }

        if (TryHandleSentiment(normalized, context, out var sentimentResponse))
        {
            var topicTip = GetRelevantTopicTip(normalized);
            return string.IsNullOrWhiteSpace(topicTip) ? sentimentResponse : $"{sentimentResponse} {topicTip}";
        }

        if (TryHandleTopic(normalized, out var topicResponse))
        {
            return topicResponse;
        }

        if (normalized.Contains("help") || normalized.Contains("what can i ask") || normalized.Contains("topics"))
        {
            return "I can help with password safety, phishing tips, privacy advice, task reminders, and a cybersecurity quiz. Ask in your own words!";
        }

        if (normalized.Contains("how are you") || normalized.Contains("how r you"))
        {
            return "I'm ready and listening. Tell me what cybersecurity topic you want to explore next.";
        }

        if (normalized.Contains("purpose") || normalized.Contains("what is your"))
        {
            return "My purpose is to help you learn how to stay safe online with tips, tasks, and quizzes.";
        }

        return "I didn't quite understand that. Can you try rephrasing? For example, ask about passwords, phishing, privacy, tasks, or the quiz.";
    }

    private bool TryHandleTopic(string normalized, out string response)
    {
        if (ContainsAny(normalized, "password", "passcode", "pin"))
        {
            response = _topicGuidance["password"];
            return true;
        }

        if (ContainsAny(normalized, "privacy", "private", "data protection"))
        {
            response = _topicGuidance["privacy"];
            return true;
        }

        if (ContainsAny(normalized, "scam", "scams", "phishing", "fake email", "otp", "one time pin"))
        {
            response = GetRandomTopicResponse("phishing");
            return true;
        }

        if (ContainsAny(normalized, "social engineering", "impersonation", "pretending"))
        {
            response = _topicGuidance["social engineering"];
            return true;
        }

        if (ContainsAny(normalized, "link", "url", "website", "browser", "browsing"))
        {
            response = "Before clicking links, hover over them to inspect the destination, look for HTTPS, and type known addresses manually.";
            return true;
        }

        response = string.Empty;
        return false;
    }

    private bool TryHandleSentiment(string normalized, UserContext context, out string response)
    {
        var sentiment = _sentimentAdvice.Keys.FirstOrDefault(normalized.Contains);
        if (sentiment is null)
        {
            response = string.Empty;
            return false;
        }

        context.Mood = sentiment;
        _activityLogger.Log($"Sentiment detected: {sentiment}");
        response = _sentimentAdvice[sentiment];
        return true;
    }

    private bool TryCreateTaskFromInput(string normalized, string originalInput, out string response)
    {
        if (!ContainsAny(normalized, "add a task", "add task", "remind me", "reminder", "set a reminder", "task to", "add reminder"))
        {
            response = string.Empty;
            return false;
        }

        var title = ExtractTitleForTask(originalInput);
        if (string.IsNullOrWhiteSpace(title))
        {
            response = "I can help you set a task. Try saying, 'Add a task to update my password.'";
            return true;
        }

        var reminder = ExtractReminder(originalInput);
        var task = _taskService.AddTask(title, title, reminder);
        var reminderMessage = string.IsNullOrWhiteSpace(reminder) ? string.Empty : $" Reminder set for {reminder}.";
        _activityLogger.Log($"Task added from chat: '{task.Title}'{(string.IsNullOrWhiteSpace(reminder) ? string.Empty : $" with reminder {reminder}")}.");
        response = $"Task added: '{task.Title}'.{reminderMessage}";
        return true;
    }

    private bool TryStoreMemory(string normalized, string originalInput, UserContext context, out string response)
    {
        if (ContainsAny(normalized, "my browser is", "i use", "i am using"))
        {
            var value = ExtractMemoryValue(originalInput, new[] { "my browser is", "i use", "i am using" });
            if (!string.IsNullOrWhiteSpace(value))
            {
                context.Memory["browser"] = value;
                response = $"Thanks for sharing. I will remember that you use {value}.";
                _activityLogger.Log($"Memory stored: browser = {value}.");
                return true;
            }
        }

        response = string.Empty;
        return false;
    }

    private string ExtractTitleForTask(string input)
    {
        var lower = input.ToLowerInvariant();
        var patterns = new[] { "remind me to", "add a task to", "add task to", "task to", "set a reminder to", "add a reminder to" };
        foreach (var pattern in patterns)
        {
            if (lower.Contains(pattern))
            {
                var title = input[(lower.IndexOf(pattern, StringComparison.Ordinal) + pattern.Length)..].Trim();
                return CleanTaskTitle(title);
            }
        }

        return CleanTaskTitle(input);
    }

    private static string CleanTaskTitle(string title)
    {
        title = Regex.Replace(title, "(in \\d+ days|tomorrow|today|next week|this week|on [A-Za-z0-9 ]+)$", string.Empty, RegexOptions.IgnoreCase).Trim();
        return title.Trim('.', ' ', '?', '!');
    }

    private static string ExtractReminder(string input)
    {
        var match = Regex.Match(input.ToLowerInvariant(), "(tomorrow|today|in \\d+ days|next week|in \\d+ hours|on [a-z0-9 ]+)", RegexOptions.IgnoreCase);
        return match.Success ? match.Value : string.Empty;
    }

    private static string ExtractMemoryValue(string input, string[] patterns)
    {
        foreach (var pattern in patterns)
        {
            var index = input.ToLowerInvariant().IndexOf(pattern, StringComparison.Ordinal);
            if (index >= 0)
            {
                return input[(index + pattern.Length)..].Trim();
            }
        }

        return string.Empty;
    }

    private string GetRelevantTopicTip(string normalized)
    {
        foreach (var topic in _topicGuidance.Keys)
        {
            if (normalized.Contains(topic))
            {
                return _topicGuidance[topic];
            }
        }

        return string.Empty;
    }

    private string GetRandomTopicResponse(string topic)
    {
        if (_randomResponses.TryGetValue(topic, out var responses) && responses.Any())
        {
            return responses[_random.Next(responses.Count)];
        }

        return _topicGuidance.GetValueOrDefault(topic, "Here is some cybersecurity guidance.");
    }

    private static bool ContainsAny(string input, params string[] keywords)
    {
        return keywords.Any(input.Contains);
    }
}
