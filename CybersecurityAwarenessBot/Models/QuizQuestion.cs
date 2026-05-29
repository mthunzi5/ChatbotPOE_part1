namespace CybersecurityAwarenessBot.Models;

public class QuizQuestion
{
    public string Question { get; init; } = string.Empty;
    public string[] Choices { get; init; } = Array.Empty<string>();
    public int CorrectAnswerIndex { get; init; }
    public string Explanation { get; init; } = string.Empty;
}
