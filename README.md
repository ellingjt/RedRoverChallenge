# Red Rover Coding Challenge
Converts a field selection string into an indented list, with optional field name sorting.

## Running
Requires .NET 8 SDK.

From the repo root:
```powershell
dotnet run --project RedRoverChallenge -- "<input>"
```

Or, for sorted output:
```powershell
dotnet run --project RedRoverChallenge -- "<input>" --sorted
```

Example:
```powershell
dotnet run --project RedRoverChallenge -- "(id, name, email, type(id, name, customFields(c1, c2, c3)), externalId)" --sorted
```

## Assumptions
- Field names can contain anything except commas or parentheses
- Outer parentheses are optional
- Mismatched parentheses throw a `FormatException` before parsing
- Nesting is capped at 100 levels to prevent stack overflow

## Tests
Some tests for edge cases are in RedRoverChallenge.Tests
```powershell
dotnet test RedRoverChallenge.Tests
```