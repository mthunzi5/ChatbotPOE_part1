using CybersecurityAwarenessBot.Models;

namespace CybersecurityAwarenessBot.Services;

public class QuizManager
{
    private readonly List<QuizQuestion> _questions;
    private int _currentQuestionIndex;
    private int _score;

    public QuizManager()
    {
        _questions = new List<QuizQuestion>
        {
            new QuizQuestion
            {
                Question = "What should you do if you receive an email asking for your password?",
                Choices = new[] { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                CorrectAnswerIndex = 2,
                Explanation = "Correct! Reporting phishing emails helps prevent scams."
            },
            new QuizQuestion
            {
                Question = "Which of the following is a strong password practice?",
                Choices = new[] { "Using your pet's name", "Reusing the same password", "Using a long passphrase", "Writing it on a sticky note" },
                CorrectAnswerIndex = 2,
                Explanation = "Use long passphrases that are hard to guess and avoid reusing passwords across accounts."
            },
            new QuizQuestion
            {
                Question = "True or false: Public Wi-Fi is always safe to use for online banking.",
                Choices = new[] { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "False. Public Wi-Fi can be unsafe; use a secure network or a VPN for sensitive activities."
            },
            new QuizQuestion
            {
                Question = "What is one sign of a phishing scam?",
                Choices = new[] { "A message from a familiar contact", "A spelling mistake in the email", "A well-formatted invoice", "A private meeting request" },
                CorrectAnswerIndex = 1,
                Explanation = "Spelling mistakes and urgency are common signs of phishing attempts."
            },
            new QuizQuestion
            {
                Question = "What does 2FA stand for?",
                Choices = new[] { "Two-Factor Authentication", "Two-Firewall Application", "Trusted File Access", "Time Frame Authorization" },
                CorrectAnswerIndex = 0,
                Explanation = "Two-Factor Authentication adds an extra security step beyond a password."
            },
            new QuizQuestion
            {
                Question = "If you are unsure whether a link is safe, what should you do?",
                Choices = new[] { "Click it quickly", "Hover over it to inspect the URL", "Forward it to friends", "Ignore your instincts" },
                CorrectAnswerIndex = 1,
                Explanation = "Hover over links to inspect the URL before clicking and verify the destination."
            },
            new QuizQuestion
            {
                Question = "Which option is the safest way to create a password?",
                Choices = new[] { "Use only numbers", "Use a mix of letters, numbers, and symbols", "Use your birthdate", "Use a common word" },
                CorrectAnswerIndex = 1,
                Explanation = "A mixed password is much harder to guess."
            },
            new QuizQuestion
            {
                Question = "True or false: You should share OTP codes with support staff if they ask for them.",
                Choices = new[] { "True", "False" },
                CorrectAnswerIndex = 1,
                Explanation = "False. You should never share one-time passwords with anyone."
            },
            new QuizQuestion
            {
                Question = "Which of these is a good privacy habit?",
                Choices = new[] { "Posting your password online", "Reviewing app permissions regularly", "Using unsecured public computers", "Sharing account details" },
                CorrectAnswerIndex = 1,
                Explanation = "Reviewing permissions keeps your data safer and helps you control what apps can access."
            },
            new QuizQuestion
            {
                Question = "What should you do if a website asks for more information than expected?",
                Choices = new[] { "Provide it without thinking", "Close the site and verify its legitimacy", "Use the same password", "Ignore the warning" },
                CorrectAnswerIndex = 1,
                Explanation = "Always verify the website before sharing personal information."
            }
        };
    }

    public int CurrentQuestionNumber => _currentQuestionIndex + 1;
    public int TotalQuestions => _questions.Count;
    public int Score => _score;

    public QuizQuestion? GetCurrentQuestion()
    {
        return _currentQuestionIndex < _questions.Count ? _questions[_currentQuestionIndex] : null;
    }

    public string SubmitAnswer(int selectedIndex)
    {
        var question = GetCurrentQuestion();
        if (question is null)
        {
            return "No question is available right now.";
        }

        var isCorrect = selectedIndex == question.CorrectAnswerIndex;
        if (isCorrect)
        {
            _score++;
        }

        var feedback = isCorrect ? "Correct!" : "Not quite.";
        var explanation = question.Explanation;

        _currentQuestionIndex++;
        return $"{feedback} {explanation}";
    }

    public void Reset()
    {
        _currentQuestionIndex = 0;
        _score = 0;
    }
}
