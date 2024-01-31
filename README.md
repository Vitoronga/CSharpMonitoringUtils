# CSharpMonitoringUtils

An easy-to-use C# system monitoring library based on the community project [*Vanara*](https://github.com/dahall/vanara). 

The goal of this library is to extract useful functionality from project Vanara regarding system monitoring (Input, Processes, Windows) for the *Microsoft Windows* operating system and make it more C# friendly (and facilitate its use).

***WARNING: This project is highly experimental and early in progress. Use it at your own risk.***

***Legal Disclaimer: I do not endorse nor support any software or creation that uses this library. As for the user, use it responsibly.***



## Before proceding:

**It is recommended to run the code with Administrator privileges, otherwise it may experience instabilities or simply not run.**

**Also, some minor functionalities may not work for 32-bit systems, so beware of that.**



## Getting Started:

### 1. Get the [*nuget package*](https://www.nuget.org/packages/MonitoringUtils/)

```
dotnet add package MonitoringUtils --version 1.0.0
```

### 2. Add namespace reference (according to the functionality you need)

```
using MonitoringUtils.Window;
using MonitoringUtils.Keyboard;
using MonitoringUtils.Mouse;
```

### 3. Use the main static classes to access functionalities:

```
// Getting focused window:
WindowInfo currentWindowInfo = WindowFuncs.GetForegroundWindow();
Console.WriteLine($"Window process name: {currentWindowInfo.ProcessName}");

// Getting mouse1 button usage
bool isM1Active = MouseFuncs.IsButtonPressed(MouseButtonsEnum.LeftButton);
Console.WriteLine($"Mouse 1 is currently {(isM1Active ? "Active" : "Inactive")}");

// Getting keyboard overall usage
bool hasKeyboardBeenUsed = KeyboardFuncs.HasAnyKeyChanged();
Console.WriteLine($"Since last check, the keyboard has {(hasKeyboardBeenUsed ? "" : "NOT ")}been used");
```


## General Tips

As the project isn't properly documented yet, here are some guidelines to work with it:

### When gathering input, the same received data may not be obtainable anymore

For example, when you call MouseFuncs.HasAnyButtonChanged(), the information about the buttons state is cleared out, so any new call to this method will report no changes until new data is gathered. This leads to the next tip:

### Store obtained data for multiple subsequent manipulations

If you need to check several input states at once, you should store that general data first, and only then perform the necessary checks. If you use the method calls directly in your conditions, you will start to face inconsistent data all around.

### Favour the library implementations over manual/custom ones

Calls to Keyboard.IsKeyPressed() and Keyboard.IsKeyToggled(), when done together, should be dropped in favour of Keyboard.IsKeyPressedOrToggled(), as it uses a single system call to retrieve both informations. This improves performance and consistency in the gathered data.


## Should I use the .NET Core or .NET Framework version?

The main version is the .NET Core. The .NET Framework version is a very raw conversion from the main one, and exists solely because it may be more adequate, compatible or useful considering the context of this library. However, some functionalities may not work properly in the .NET Framework version, and alternative ways of running them may become necessary. Major problems regarding that may be reported by creating an issue in this repo.
