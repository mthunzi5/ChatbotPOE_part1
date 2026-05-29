using System.Drawing;
using System.Windows.Forms;
using CybersecurityAwarenessBot.Models;
using CybersecurityAwarenessBot.Services;

namespace CybersecurityAwarenessBot.Forms;

public class MainForm : Form
{
    private readonly ResponseService _responseService;
    private readonly TaskService _taskService;
    private readonly ActivityLogger _activityLogger;
    private readonly QuizManager _quizManager;
    private readonly AudioService _audioService;
    private readonly AsciiArtService _asciiArtService;
    private readonly UserContext _context = new();

    private readonly RichTextBox _conversationBox;
    private readonly TextBox _inputBox;
    private readonly Button _sendButton;
    private readonly Button _taskButton;
    private readonly Button _manageTasksButton;
    private readonly Button _quizButton;
    private readonly Button _activityLogButton;
    private readonly Button _clearButton;
    private readonly TextBox _asciiArtTextBox;
    private readonly ListBox _suggestionList;

    public MainForm()
    {
        Text = "Cybersecurity Awareness Chatbot";
        ClientSize = new Size(980, 700);
        BackColor = Color.FromArgb(30, 34, 45);
        Font = new Font("Segoe UI", 10);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;

        _taskService = new TaskService();
        _activityLogger = new ActivityLogger();
        _quizManager = new QuizManager();
        _audioService = new AudioService();
        _asciiArtService = new AsciiArtService();
        _responseService = new ResponseService(_taskService, _activityLogger);

        var titleLabel = new Label
        {
            Text = "Cybersecurity Awareness Assistant",
            Font = new Font(Font.FontFamily, 18, FontStyle.Bold),
            ForeColor = Color.White,
            AutoSize = true,
            Left = 20,
            Top = 16
        };

        _conversationBox = new RichTextBox
        {
            ReadOnly = true,
            BackColor = Color.FromArgb(20, 24, 34),
            ForeColor = Color.Gainsboro,
            BorderStyle = BorderStyle.None,
            Font = new Font("Consolas", 10),
            Location = new Point(20, 60),
            Size = new Size(620, 520),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left
        };

        _inputBox = new TextBox
        {
            Location = new Point(20, 590),
            Size = new Size(520, 30),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            BackColor = Color.FromArgb(40, 44, 54),
            ForeColor = Color.White
        };

        _sendButton = new Button
        {
            Text = "Send",
            Location = new Point(550, 590),
            Size = new Size(90, 30),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
            BackColor = Color.FromArgb(56, 133, 255),
            ForeColor = Color.White
        };
        _sendButton.Click += (_, _) => ProcessUserInput();

        _clearButton = new Button
        {
            Text = "Clear Chat",
            Location = new Point(650, 590),
            Size = new Size(100, 30),
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left,
            BackColor = Color.FromArgb(220, 60, 70),
            ForeColor = Color.White
        };
        _clearButton.Click += (_, _) => _conversationBox.Clear();

        _asciiArtTextBox = new TextBox
        {
            Multiline = true,
            ReadOnly = true,
            Font = new Font("Consolas", 9, FontStyle.Regular),
            ScrollBars = ScrollBars.Vertical,
            BackColor = Color.FromArgb(15, 18, 28),
            ForeColor = Color.LightGreen,
            Location = new Point(660, 60),
            Size = new Size(300, 260),
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };

        _taskButton = new Button
        {
            Text = "Add Task",
            Location = new Point(660, 340),
            Size = new Size(140, 40),
            BackColor = Color.FromArgb(58, 191, 116),
            ForeColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _taskButton.Click += (_, _) => AddTaskViaGui();

        _manageTasksButton = new Button
        {
            Text = "View / Manage Tasks",
            Location = new Point(820, 340),
            Size = new Size(140, 40),
            BackColor = Color.FromArgb(90, 170, 250),
            ForeColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _manageTasksButton.Click += (_, _) => ShowTaskManager();

        _quizButton = new Button
        {
            Text = "Start Quiz",
            Location = new Point(660, 400),
            Size = new Size(140, 40),
            BackColor = Color.FromArgb(255, 175, 60),
            ForeColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _quizButton.Click += (_, _) => BeginQuiz();

        _activityLogButton = new Button
        {
            Text = "Activity Log",
            Location = new Point(820, 400),
            Size = new Size(140, 40),
            BackColor = Color.FromArgb(136, 84, 208),
            ForeColor = Color.White,
            Anchor = AnchorStyles.Top | AnchorStyles.Right
        };
        _activityLogButton.Click += (_, _) => ShowActivityLog();

        var suggestionLabel = new Label
        {
            Text = "Try asking:",
            ForeColor = Color.White,
            Location = new Point(660, 460),
            AutoSize = true
        };

        _suggestionList = new ListBox
        {
            Location = new Point(660, 490),
            Size = new Size(300, 90),
            BackColor = Color.FromArgb(20, 24, 34),
            ForeColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle,
            Font = new Font("Segoe UI", 9)
        };
        _suggestionList.Items.AddRange(new[]
        {
            "Tell me about password safety",
            "Give me a phishing tip",
            "I'm worried about online scams",
            "Add a task to enable 2FA",
            "Show activity log"
        });
        _suggestionList.DoubleClick += (_, _) => {
            if (_suggestionList.SelectedItem is string item)
            {
                _inputBox.Text = item;
                ProcessUserInput();
            }
        };

        Controls.Add(titleLabel);
        Controls.Add(_conversationBox);
        Controls.Add(_inputBox);
        Controls.Add(_sendButton);
        Controls.Add(_clearButton);
        Controls.Add(_asciiArtTextBox);
        Controls.Add(_taskButton);
        Controls.Add(_manageTasksButton);
        Controls.Add(_quizButton);
        Controls.Add(_activityLogButton);
        Controls.Add(suggestionLabel);
        Controls.Add(_suggestionList);

        Load += MainForm_Load;
        _inputBox.KeyDown += (sender, args) =>
        {
            if (args.KeyCode == Keys.Enter)
            {
                args.Handled = true;
                args.SuppressKeyPress = true;
                ProcessUserInput();
            }
        };
    }

    private void MainForm_Load(object? sender, EventArgs e)
    {
        try
        {
            _audioService.PlayGreeting();
        }
        catch
        {
            // Ignore audio failures in the GUI.
        }

        _asciiArtTextBox.Text = _asciiArtService.LoadHeaderArt();

        _context.UserName = PromptHelper.ShowSingleLine("Welcome", "Please enter your name:")?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(_context.UserName))
        {
            _context.UserName = "Guest";
        }

        AddBotMessage($"Welcome, {_context.UserName}! I am your Cybersecurity Awareness Assistant.");
        AddBotMessage("Ask me about phishing, passwords, privacy, or task reminders.");
        AddBotMessage("Use the buttons to add tasks, play the quiz, or view your activity log.");
        AddBotMessage("If you are not sure, ask me for a tip or say 'help'.");
    }

    private void ProcessUserInput()
    {
        var input = _inputBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(input))
        {
            AddBotMessage("I didn't quite understand that. Please enter a question or command.");
            return;
        }

        AddUserMessage(input);
        _inputBox.Clear();

        var lower = input.ToLowerInvariant();
        if (lower.Contains("start quiz") || lower.Contains("quiz") || lower.Contains("question game") || lower.Contains("test my knowledge"))
        {
            AddBotMessage("Sure! Let's begin the cybersecurity quiz.");
            _activityLogger.Log("Quiz started from chat input.");
            BeginQuiz();
            return;
        }

        if (lower.Contains("activity log") || lower.Contains("what have you done") || lower.Contains("show log"))
        {
            ShowActivityLog();
            return;
        }

        if (lower.Contains("task") || lower.Contains("remind me") || lower.Contains("reminder"))
        {
            var response = _responseService.GetResponse(input, _context);
            AddBotMessage(response);
            return;
        }

        var answer = _responseService.GetResponse(input, _context);
        AddBotMessage(answer);
    }

    private void AddUserMessage(string text)
    {
        AppendText("You: ", Color.LightSkyBlue, true);
        AppendText(text + Environment.NewLine, Color.White, false);
    }

    private void AddBotMessage(string text)
    {
        AppendText("Bot: ", Color.LightCoral, true);
        AppendText(text + Environment.NewLine, Color.WhiteSmoke, false);
    }

    private void AppendText(string text, Color color, bool bold)
    {
        _conversationBox.SelectionStart = _conversationBox.TextLength;
        _conversationBox.SelectionLength = 0;
        _conversationBox.SelectionColor = color;
        _conversationBox.SelectionFont = new Font(_conversationBox.Font, bold ? FontStyle.Bold : FontStyle.Regular);
        _conversationBox.AppendText(text);
        _conversationBox.SelectionColor = _conversationBox.ForeColor;
        _conversationBox.ScrollToCaret();
    }

    private void AddTaskViaGui()
    {
        var title = PromptHelper.ShowSingleLine("Add Cybersecurity Task", "Task title:");
        if (string.IsNullOrWhiteSpace(title))
        {
            return;
        }

        var description = PromptHelper.ShowSingleLine("Task Description", "Description for the task:");
        var reminder = PromptHelper.ShowSingleLine("Task Reminder", "Optional reminder (e.g. in 3 days, tomorrow):");

        var task = _taskService.AddTask(title, description, reminder);
        _activityLogger.Log($"Task added: '{task.Title}'{(string.IsNullOrWhiteSpace(task.Reminder) ? string.Empty : $" (Reminder: {task.Reminder})")}.");

        AddBotMessage($"Task added: '{task.Title}'.{(string.IsNullOrWhiteSpace(task.Reminder) ? string.Empty : $" Reminder set: {task.Reminder}.")}");
    }

    private void ShowTaskManager()
    {
        using var manager = new TaskManagerForm(_taskService, _activityLogger);
        manager.ShowDialog(this);
    }

    private void BeginQuiz()
    {
        _quizManager.Reset();
        using var quizForm = new QuizForm(_quizManager);
        if (quizForm.ShowDialog(this) == DialogResult.OK)
        {
            var finalMessage = quizForm.FinalMessage;
            _activityLogger.Log($"Quiz completed: { _quizManager.Score}/{_quizManager.TotalQuestions }.");
            AddBotMessage(finalMessage);
        }
    }

    private void ShowActivityLog()
    {
        var log = _activityLogger.GetRecentLog();
        MessageBox.Show(this, log, "Recent Activity", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
}
