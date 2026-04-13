# Cybersecurity Awareness Chatbot (Part 1)

This repository contains a C# console chatbot built for a South African cybersecurity awareness campaign.

## What is implemented

- Voice greeting support (WAV file loaded at startup)
- ASCII art header display
- Text-based user greeting with name personalization
- Predefined responses:
  - How are you?
  - What's your purpose?
  - What can I ask you about?
  - Password safety
  - Phishing awareness
  - Safe browsing
  - Social engineering
- Input validation for empty/unsupported input
- Enhanced console UI:
  - Colored text
  - Section dividers
  - Typing effect
- Structured code using classes and services (not all in `Program.cs`)
- GitHub Actions CI workflow for restore + build

## Project structure

- `CybersecurityAwarenessBot/Program.cs` - entry point
- `CybersecurityAwarenessBot/ChatbotApp.cs` - app flow and conversation loop
- `CybersecurityAwarenessBot/Services/ConsoleStyler.cs` - colors, borders, typing effect
- `CybersecurityAwarenessBot/Services/AudioService.cs` - WAV playback
- `CybersecurityAwarenessBot/Services/AsciiArtService.cs` - ASCII art loading
- `CybersecurityAwarenessBot/Services/ResponseService.cs` - response logic
- `CybersecurityAwarenessBot/Assets/ascii-logo.txt` - ASCII image
- `CybersecurityAwarenessBot/Assets/welcome.wav` - sample WAV greeting file (replace with your own recorded WAV)
- `.github/workflows/dotnet-ci.yml` - CI pipeline

## Important multimedia note

`Assets/welcome.wav` is a valid sample WAV file so the project can run immediately. Replace it with your own recorded greeting before final submission.

Recommended recording line:

"Hello! Welcome to the Cybersecurity Awareness Bot. I'm here to help you stay safe online."

## Run locally

From repository root:

```bash
dotnet run --project CybersecurityAwarenessBot
```

## Full start-to-finish setup guide (from scratch)

### 1. Install required software

1. Install **Visual Studio Code**:
   - https://code.visualstudio.com/
2. Install **.NET SDK 9** (or latest):
   - https://dotnet.microsoft.com/download
3. Install **Git**:
   - https://git-scm.com/downloads
4. (Optional) Install **GitHub Desktop** if you prefer GUI:
   - https://desktop.github.com/

### 2. Configure Git (first time only)

```bash
git config --global user.name "Your Name"
git config --global user.email "you@example.com"
```

### 3. Create and open project folder

```bash
mkdir sxovachatbot
cd sxovachatbot
code .
```

### 4. Build and run

```bash
dotnet restore CybersecurityAwarenessBot/CybersecurityAwarenessBot.csproj
dotnet build CybersecurityAwarenessBot/CybersecurityAwarenessBot.csproj
dotnet run --project CybersecurityAwarenessBot
```

### 5. Replace the placeholder voice file

1. Record your greeting (phone or voice recorder).
2. Convert/export as `.wav`.
3. Save it as:
   - `CybersecurityAwarenessBot/Assets/welcome.wav`
4. Run the app again and verify sound plays.

### 6. Create GitHub repository and push

1. On GitHub, create a new repo named `sxovachatbot`.
2. In terminal:

```bash
git init
git add .
git commit -m "Initial project scaffold"
git branch -M main
git remote add origin https://github.com/<your-username>/sxovachatbot.git
git push -u origin main
```

### 7. Minimum six meaningful commits (example plan)

Use commits like these:

1. `chore: scaffold dotnet console project`
2. `feat: add chatbot app flow and name personalization`
3. `feat: add cybersecurity response engine`
4. `feat: add console styling and typing effect`
5. `feat: add ascii art and voice greeting asset support`
6. `ci: add github actions build workflow`

### 8. Verify CI workflow

1. Go to GitHub repo -> **Actions**.
2. Confirm latest run has a green check mark.
3. Take screenshot of successful run.
4. Add screenshot to `README.md`.

### 9. ARC submission checklist

Submit **only your GitHub repo link** containing:

- Source code
- README
- WAV greeting file
- ASCII art file
- CI workflow in `.github/workflows`
- Screenshot of successful CI run in README
- At least six meaningful commits

### 10. Presentation requirement

Upload an **unlisted YouTube video** showing:

- Code structure and classes
- Logic for user interaction and responses
- How WAV voice integration works
- Console formatting choices

## Troubleshooting

- If audio does not play: verify `welcome.wav` exists and is valid WAV.
- If CI fails: confirm workflow file path is exactly `.github/workflows/dotnet-ci.yml`.
- If `dotnet` command is missing: reinstall .NET SDK and restart terminal.
