using CybersecurityAwarenessBot.Models;

namespace CybersecurityAwarenessBot.Services;

public class TaskService
{
    private readonly List<TaskItem> _tasks = new();
    private int _nextId = 1;

    public IReadOnlyList<TaskItem> Tasks => _tasks.AsReadOnly();

    public TaskItem AddTask(string title, string description, string reminder)
    {
        var task = new TaskItem
        {
            Id = _nextId++,
            Title = title.Trim(),
            Description = description.Trim(),
            Reminder = reminder.Trim()
        };

        _tasks.Add(task);
        return task;
    }

    public bool CompleteTask(int id)
    {
        var task = _tasks.FirstOrDefault(x => x.Id == id);
        if (task is null)
        {
            return false;
        }

        task.IsCompleted = true;
        return true;
    }

    public bool DeleteTask(int id)
    {
        var task = _tasks.FirstOrDefault(x => x.Id == id);
        if (task is null)
        {
            return false;
        }

        return _tasks.Remove(task);
    }
}
