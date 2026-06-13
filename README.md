# PersonalAI

A lightweight Windows desktop app for chatting with LLM providers via a floating system-tray UI.

## Getting started

1. Download the latest release from https://github.com/vasantbala/PersonalAI/releases
2. Extract the zip to a folder of your choice
3. Set your API key(s) as environment variables (see below)
4. Edit `appsettings.json` to configure your providers
5. Run `PersonalAI.exe`

## Configuration

`appsettings.json` supports multiple providers. Set `ApiKey` to a literal value or reference an environment variable using `${VAR_NAME}` syntax.

### OpenRouter (OpenAI-compatible)
```json
{
  "AppSettings": {
    "Providers": [
      {
        "LLMType": "OpenRouter",
        "Url": "https://openrouter.ai/api",
        "ApiType": "openai",
        "ApiKey": "${OPENROUTER_API_KEY}",
        "ModelName": "openai/gpt-4o"
      }
    ]
  }
}
```

### Anthropic Claude
```json
{
  "AppSettings": {
    "Providers": [
      {
        "LLMType": "Claude",
        "Url": "https://api.anthropic.com",
        "ApiType": "claude",
        "ApiKey": "${ANTHROPIC_API_KEY}",
        "ModelName": "claude-sonnet-4-6"
      }
    ]
  }
}
```

Multiple providers can be listed — switch between them using the settings dropdown in the app.

## Keyboard shortcuts

| Shortcut | Action |
|----------|--------|
| `Ctrl+Enter` | Send message |
| `Ctrl+\` | Clear conversation |
| `Ctrl+F1` | Bring window to front |

## Building

Requires .NET 8 SDK and Windows.

```powershell
dotnet build PersonalAI.sln
```

## Releasing

Create and push a version tag to trigger a GitHub Release with a self-contained `win-x64` executable:

```powershell
git tag v1.0.0
git push origin v1.0.0
```

The release workflow builds, publishes a single-file exe, and uploads it to GitHub Releases automatically.
