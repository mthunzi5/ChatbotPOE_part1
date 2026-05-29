using System.Drawing;
using System.Windows.Forms;
using CybersecurityAwarenessBot.Services;
using CybersecurityAwarenessBot.Models;

namespace CybersecurityAwarenessBot.Forms;

public class TaskManagerForm : Form
{
    private readonly TaskService _taskService;
    private readonly ActivityLogger _activityLogger;
    private readonly ListBox _taskListBox;
    private readonly Button _completeButton;
    private readonly Button _deleteButton;
    private readonly Label _detailsLabel;

    public TaskManagerForm(TaskService taskService, ActivityLogger activityLogger)
    {
        _taskService = taskService;
        _activityLogger = activityLogger;

        Text = "Manage Cybersecurity Tasks";
        ClientSize = new Size(560, 420);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        BackColor = Color.FromArgb(24, 28, 36);
        ForeColor = Color.White;

        _taskListBox = new ListBox
        {
            Location = new Point(20, 20),
            Size = new Size(520, 240),
            BackColor = Color.FromArgb(15, 18, 28),
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 9)
        };
        _taskListBox.SelectedIndexChanged += (_, _) => UpdateTaskDetails();

        _completeButton = new Button
        {
            Text = "Mark Completed",
            Location = new Point(20, 280),
            Size = new Size(160, 40),
            BackColor = Color.FromArgb(58, 191, 116),
            ForeColor = Color.White
        };
        _completeButton.Click += (_, _) => CompleteSelectedTask();

        _deleteButton = new Button
        {
            Text = "Delete Task",
            Location = new Point(200, 280),
            Size = new Size(160, 40),
            BackColor = Color.FromArgb(220, 60, 70),
            ForeColor = Color.White
        };
        _deleteButton.Click += (_, _) => DeleteSelectedTask();

        var closeButton = new Button
        {
            Text = "Close",
            Location = new Point(380, 280),
            Size = new Size(160, 40),
            BackColor = Color.FromArgb(90, 170, 250),
            ForeColor = Color.White,
            DialogResult = DialogResult.OK
        };

        _detailsLabel = new Label
        {
            Text = "Select a task to see the details.",
            Location = new Point(20, 340),
            Size = new Size(520, 60),
            ForeColor = Color.Gainsboro
        };

        Controls.Add(_taskListBox);
        Controls.Add(_completeButton);
        Controls.Add(_deleteButton);
        Controls.Add(closeButton);
        Controls.Add(_detailsLabel);

        Load += (_, _) => RefreshTaskList();
    }

    private void RefreshTaskList()
    {
        _taskListBox.Items.Clear();
        foreach (var task in _taskService.Tasks)
        {
            _taskListBox.Items.Add(FormatTaskSummary(task));
        }

        UpdateTaskDetails();
    }

    private static string FormatTaskSummary(TaskItem task)
    {
        return $"[{task.Id}] {task.Title} ({task.Status})";
    }

    private void UpdateTaskDetails()
    {
        if (_taskListBox.SelectedIndex < 0)
        {
            _detailsLabel.Text = "Select a task to see the details.";
            return;
        }

        var task = _taskService.Tasks[_taskListBox.SelectedIndex];
        _detailsLabel.Text = $"Description: {task.Description}\nReminder: {task.Reminder}\nCreated: {task.CreatedAt:d}\nStatus: {task.Status}";
    }

    private void CompleteSelectedTask()
    {
        if (_taskListBox.SelectedIndex < 0)
        {
            MessageBox.Show(this, "Please select a task first.", "No task selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var task = _taskService.Tasks[_taskListBox.SelectedIndex];
        if (task.IsCompleted)
        {
            MessageBox.Show(this, "This task is already completed.", "Task complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        _taskService.CompleteTask(task.Id);
        _activityLogger.Log($"Task marked completed: '{task.Title}'.");
        RefreshTaskList();
    }

    private void DeleteSelectedTask()
    {
        if (_taskListBox.SelectedIndex < 0)
        {
            MessageBox.Show(this, "Please select a task first.", "No task selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var task = _taskService.Tasks[_taskListBox.SelectedIndex];
        if (MessageBox.Show(this, $"Delete task '{task.Title}'?", "Confirm delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            _taskService.DeleteTask(task.Id);
            _activityLogger.Log($"Task deleted: '{task.Title}'.");
            RefreshTaskList();
        }
    }
}
