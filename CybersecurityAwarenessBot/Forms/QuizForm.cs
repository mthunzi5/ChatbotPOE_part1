using System.Drawing;
using System.Windows.Forms;
using CybersecurityAwarenessBot.Services;
using CybersecurityAwarenessBot.Models;

namespace CybersecurityAwarenessBot.Forms;

public class QuizForm : Form
{
    private readonly QuizManager _quizManager;
    private readonly Label _questionLabel;
    private readonly List<RadioButton> _choiceButtons;
    private readonly Button _submitButton;
    private readonly Label _feedbackLabel;

    public string FinalMessage { get; private set; } = string.Empty;

    public QuizForm(QuizManager quizManager)
    {
        _quizManager = quizManager;

        Text = "Cybersecurity Quiz";
        ClientSize = new Size(620, 420);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        StartPosition = FormStartPosition.CenterParent;
        MaximizeBox = false;
        BackColor = Color.FromArgb(20, 24, 34);
        ForeColor = Color.White;

        _questionLabel = new Label
        {
            Location = new Point(20, 20),
            Size = new Size(580, 80),
            Font = new Font("Segoe UI", 11, FontStyle.Bold),
            ForeColor = Color.White
        };

        _choiceButtons = new List<RadioButton>();
        for (var i = 0; i < 4; i++)
        {
            var choiceButton = new RadioButton
            {
                Location = new Point(40, 110 + i * 50),
                Size = new Size(540, 40),
                ForeColor = Color.Gainsboro,
                Font = new Font("Segoe UI", 10)
            };
            _choiceButtons.Add(choiceButton);
            Controls.Add(choiceButton);
        }

        _submitButton = new Button
        {
            Text = "Submit Answer",
            Location = new Point(420, 320),
            Size = new Size(180, 40),
            BackColor = Color.FromArgb(58, 191, 116),
            ForeColor = Color.White
        };
        _submitButton.Click += (_, _) => SubmitAnswer();

        _feedbackLabel = new Label
        {
            Location = new Point(20, 320),
            Size = new Size(380, 80),
            ForeColor = Color.LightGreen,
            Font = new Font("Segoe UI", 10, FontStyle.Regular)
        };

        Controls.Add(_questionLabel);
        Controls.Add(_submitButton);
        Controls.Add(_feedbackLabel);

        Load += (_, _) => ShowCurrentQuestion();
    }

    private void ShowCurrentQuestion()
    {
        var question = _quizManager.GetCurrentQuestion();
        if (question is null)
        {
            CloseQuiz();
            return;
        }

        _questionLabel.Text = $"Question {_quizManager.CurrentQuestionNumber}/{_quizManager.TotalQuestions}: {question.Question}";

        for (var i = 0; i < _choiceButtons.Count; i++)
        {
            _choiceButtons[i].Visible = i < question.Choices.Length;
            if (i < question.Choices.Length)
            {
                _choiceButtons[i].Text = question.Choices[i];
                _choiceButtons[i].Checked = false;
            }
        }

        _feedbackLabel.Text = string.Empty;
        _submitButton.Enabled = true;
    }

    private void SubmitAnswer()
    {
        var question = _quizManager.GetCurrentQuestion();
        if (question is null)
        {
            CloseQuiz();
            return;
        }

        var selectedIndex = _choiceButtons.FindIndex(x => x.Checked);
        if (selectedIndex < 0)
        {
            MessageBox.Show(this, "Please select an answer before submitting.", "Answer Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var feedback = _quizManager.SubmitAnswer(selectedIndex);
        _feedbackLabel.Text = feedback;
        _submitButton.Enabled = false;

        Task.Delay(900).ContinueWith(_ =>
        {
            Invoke(() =>
            {
                if (_quizManager.GetCurrentQuestion() is null)
                {
                    CloseQuiz();
                }
                else
                {
                    ShowCurrentQuestion();
                }
            });
        });
    }

    private void CloseQuiz()
    {
        FinalMessage = _quizManager.Score switch
        {
            var s when s == _quizManager.TotalQuestions => $"Excellent! You scored {s}/{_quizManager.TotalQuestions}. You're a cybersecurity pro!",
            var s when s >= _quizManager.TotalQuestions * 0.7 => $"Great job! You scored {s}/{_quizManager.TotalQuestions}. Keep building your cybersecurity skills.",
            _ => $"You scored {_quizManager.Score}/{_quizManager.TotalQuestions}. Keep learning to stay safe online!"
        };

        DialogResult = DialogResult.OK;
        Close();
    }
}
