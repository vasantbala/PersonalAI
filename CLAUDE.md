# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

PersonalAI is a Windows desktop application (WPF) that provides a lightweight UI for interacting with LLM services. It allows users to send prompts to multiple configured LLM backends and receive responses through a minimal, modern interface with a system tray integration.

**Tech Stack:**
- Language: C# (.NET 8.0)
- UI Framework: WPF (Windows Presentation Foundation) with MahApps.Metro theming
- Architecture: Modular solution with dependency injection (Microsoft.Extensions.DependencyInjection)
- Build System: Visual Studio/.NET CLI

## Build and Run Commands

### Building the Project
```bash
# Build using .NET CLI
dotnet build PersonalAI.sln

# Build in Release mode
dotnet build PersonalAI.sln -c Release

# Build just the main application
dotnet build PersonalAI/PersonalAI.csproj
```

### Running the Application
```bash
# Run the main WPF application
dotnet run --project PersonalAI/PersonalAI.csproj

# Run in Release mode
dotnet run --project PersonalAI/PersonalAI.csproj -c Release
```

### Publishing
```bash
# Publish as single executable file (configured in .csproj)
dotnet publish PersonalAI/PersonalAI.csproj -c Release -o ./publish
```

## Project Structure and Architecture

### Solution Layout
- **PersonalAI** (Main WPF Application) - Desktop UI and application entry point
- **PersonalAI.Common** - Shared configuration classes (AppSettings, GradleSettingsItem)
- **PersonalAI.Core** - LLM client abstraction and HTTP communication
- **PersonalAI.Sandbox** - UWP/sandbox application variant
- **PersonalAI.Sandbox.WPF** - Alternative WPF implementation for testing

### Core Components

#### 1. Configuration Layer (PersonalAI.Common)
- **AppSettings.cs** - Root configuration class containing array of LLM service definitions
- **GradleSettingsItem.cs** - Configuration for individual LLM services (LLMType, URL)
- Configuration is read from `appsettings.json` and bound via `IOptions<AppSettings>` pattern

#### 2. Service Layer (PersonalAI.Core)
- **IGradioClient** - Interface defining the contract for LLM prediction requests
- **GradioClient** - HTTP client implementation that:
  - Uses `IHttpClientFactory` for creating named HTTP clients (one per LLM type)
  - Sends POST requests to `/predict` endpoint with form-encoded prompt data
  - Returns response as string

#### 3. UI Layer (PersonalAI - Main Application)
- **App.xaml.cs** - Application bootstrap and dependency injection setup:
  - Configures all services using Microsoft.Extensions.DependencyInjection
  - Loads configuration from `appsettings.json`
  - Creates named HTTP clients for each configured LLM service
  - Manages system tray icon and context menu
- **MainWindow.xaml.cs** - Main UI window with:
  - Hotkey registration (Ctrl+F1) for bringing window to front
  - LLM service dropdown populated from configuration
  - Search box with Ctrl+Enter to send prompts
  - Real-time response display with copy-to-clipboard and clear buttons
  - Settings toggle to show/hide LLM selection dropdown
  - Single-instance enforcement (second launch attempt fails)
- **ViewModels** - MVVM pattern classes:
  - **LLMServiceViewModel** - Manages bound collection of LLM options and selection state
  - **LLMServiceItemViewModel** - Represents individual LLM service option (Name, BaseUrl)

#### 4. UI Design
- Built with MahApps.Metro theming for modern Windows 11 appearance
- Borderless window with rounded corners and gradient background
- Draggable header implementation using transparent Thumb control
- Responsive layout with auto-sizing TextBox for multi-line input/output
- System tray integration with minimize-to-tray functionality

### Configuration (appsettings.json)
The application requires `appsettings.json` in the output directory with structure:
```json
{
  "AppSettings": {
    "GradleSettings": [
      {
        "LLMType": "OpenAI",
        "Url": "http://127.0.0.1:5000"
      }
    ]
  }
}
```

Multiple LLM services can be configured and selected from the dropdown. The URL must point to a Gradio-compatible endpoint accepting POST requests with a "prompt" parameter.

## Data Flow

1. **Initialization** (App.xaml.cs)
   - Load appsettings.json → Parse into AppSettings object
   - Create named HttpClient for each configured LLM service
   - Inject dependencies into MainWindow (IGradioClient, IOptions<AppSettings>)

2. **UI Interaction** (MainWindow.xaml.cs)
   - User selects LLM from dropdown (bound to SelectedLLMService in ViewModel)
   - User types prompt and presses Ctrl+Enter
   - ShowMainWindow() displays main window if minimized (accessible via Ctrl+F1 hotkey or tray click)

3. **Prediction Request** (MainWindow.xaml.cs → GradioClient)
   - Ctrl+Enter triggers DoPredict() which calls GradioClient.Predict(llmType, prompt)
   - GradioClient uses HttpClientFactory to get named client for selected LLM type
   - Sends POST to `/predict` with form-encoded prompt
   - Response is displayed in ResponseTB TextBox

## Key Technical Decisions

- **Dependency Injection**: Microsoft.Extensions.DependencyInjection for loose coupling and testability
- **Configuration Binding**: IOptions<T> pattern allows hot-reload of settings (appsettings.json)
- **Named HttpClients**: Each LLM service gets a separate named HttpClient with its own BaseAddress
- **Single Instance**: Window hotkey registration (Ctrl+F1) prevents multiple instances
- **Tray Integration**: System.Windows.Forms.NotifyIcon provides tray presence even when window is minimized

## Important Files

- `PersonalAI/App.xaml.cs` - Startup and DI configuration
- `PersonalAI/MainWindow.xaml.cs` - Main UI logic and event handlers
- `PersonalAI.Core/GradioClient.cs` - LLM HTTP communication
- `PersonalAI/appsettings.json` - LLM service configuration
- `PersonalAI.sln` - Solution file (root)

## Development Notes

- `PersonalAI.sln` is at the repo root; the old `PersonalAI.Sandbox/PersonalAI.Sandbox.sln` is legacy and includes the sandbox projects
- `PersonalAI.Sandbox.WPF` is an alternative implementation; main app is `PersonalAI`
- MahApps.Metro.IconPacks provides Modern icon style used throughout the UI
- XAML uses data binding extensively; changes to ViewModels require PropertyChanged events
- The application must run from a directory containing appsettings.json (set via Directory.GetCurrentDirectory() in App.cs)
