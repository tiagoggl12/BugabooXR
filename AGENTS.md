# AGENTS.md - Development Guidelines for BugabooXR

This document contains essential information for agentic coding agents working on this Unity AR project.

## Project Overview

BugabooXR is a Unity 6 AR application using AR Foundation for cross-platform mobile AR experiences (ARCore/ARKit). The project includes interactive onboarding, object placement/manipulation, and plane detection visualization.

## Build Commands

### Unity Test Framework
```bash
# Run Edit Mode tests (unit tests)
Unity -batchmode -runTests -projectPath . -testResults editMode-results.xml -testPlatform EditMode

# Run Play Mode tests 
Unity -batchmode -runTests -projectPath . -testResults playMode-results.xml -testPlatform PlayMode

# Run specific test class
Unity -batchmode -runTests -projectPath . -testResults results.xml -testPlatform EditMode -testCategory "ARConfigurationTests"
```

### Build Commands
```bash
# Build for Android (APK)
Unity -quit -batchmode -projectPath . -executeMethod BuildCommand.PerformBuild -buildTarget Android

# Build for iOS (Xcode project)
Unity -quit -batchmode -projectPath . -executeMethod BuildCommand.PerformBuild -buildTarget iOS

# Alternative: Use Unity menu items in editor
Build > Build iOS
Build > Build Android
```

### CI/CD Pipeline
- **GitHub Actions** handles automated Android builds and testing
- **Xcode Cloud** handles iOS builds and TestFlight deployment
- Build artifacts are stored in `build/Android/` and `build/iOS/`

## Code Style Guidelines

### Naming Conventions
- **Private fields**: `_camelCase` with underscore prefix (enforced by EditorConfig)
- **Public properties/fields**: `PascalCase`
- **Methods**: `PascalCase` 
- **Local variables**: `camelCase`
- **Constants**: `PascalCase` or `UPPER_CASE`

Example:
```csharp
[SerializeField] Button m_CreateButton;  // Private serialized field
public Button createButton { get; set; } // Public property
private int _playerScore;                // Private field
const int MAX_HEALTH = 100;              // Constant
```

### Import Organization
- System namespaces first, then Unity namespaces, then third-party
- Group related imports together
- Remove unused imports

```csharp
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
```

### Class Structure
1. File header with namespace and class documentation
2. Public fields and properties with [SerializeField] and [Tooltip]
3. Private fields
4. Unity lifecycle methods (Awake, Start, Update, etc.)
5. Public methods
6. Private helper methods

### Unity-Specific Patterns
- Use `[SerializeField]` for private fields that need inspector access
- Use `[Tooltip("")]` for all serialized fields
- Implement proper event subscription/unsubscription in OnEnable/OnDisable
- Use `TryGetComponent<T>()` instead of `GetComponent<T>()` when component might not exist
- Always null-check Unity objects before accessing

### Error Handling
- Use `Debug.LogWarning()` for recoverable issues
- Use `Debug.LogError()` for critical failures
- Always validate required components in Awake/Start
- Return early from methods when validation fails

### AR-Specific Guidelines
- Always check AR support before initializing AR features
- Handle AR session state changes properly
- Clean up AR trackables when they're removed
- Use appropriate permissions handling for camera access

## Project Structure

```
Assets/
├── MobileARTemplateAssets/     # Core AR template scripts and UI
├── Samples/XR Interaction Toolkit/  # XRI sample scripts
├── Scenes/                     # Main AR scenes
├── XR/                         # XR rig and configuration
├── Editor/                     # Custom editor scripts
└── Tests/                      # Unit and integration tests
    ├── EditMode/               # Editor-only tests
    └── PlayMode/               # Runtime tests
```

## Key Dependencies
- Unity 6000.3.3f1 or later
- AR Foundation 6.3.1
- XR Interaction Toolkit 3.3.0
- Universal Render Pipeline 17.3.0
- Unity Input System 1.17.0

## Testing Strategy
- **EditMode tests**: Validate project configuration, packages, and build settings
- **PlayMode tests**: Test AR functionality, object placement, and user interactions
- Always run tests before committing changes
- Tests are located in `Assets/Tests/` with appropriate subdirectories

## Platform Considerations
- **Android**: Requires ARCore, minimum SDK 30+ (Android 11+)
- **iOS**: Requires ARKit, iOS 11.0+
- Always test on physical devices (AR requires camera access)
- Platform-specific code should use `#if UNITY_ANDROID` or `#if UNITY_IOS`

## Common Pitfalls
1. Don't make serialized fields readonly (Unity needs them writable)
2. Don't remove unused private methods (Unity may call them via reflection)
3. Always handle AR session lifecycle properly
4. Remember to manage event listeners to prevent memory leaks
5. Test cross-platform compatibility when adding platform-specific features

## Git Workflow
- Main branch: `main`
- iOS builds deploy to `xcode-project` branch
- Use descriptive commit messages following the project's established pattern
- Always run tests before pushing to main

## Performance Guidelines
- Use object pooling for frequently spawned/destroyed AR objects
- Optimize mesh visualizers for plane detection
- Consider impact on mobile battery life
- Profile on target devices, not just in editor