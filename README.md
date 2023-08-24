# CSharpMonitoringUtils

An easy-to-use C# system monitoring library based on the community project *Vanara*. 

The goal of this library is to extract useful functionality from project Vanara regarding system monitoring (Input, Processes, Windows) for the *Microsoft Windows* operating system and make it more C# friendly (and facilitate its use).

***WARNING: This project is highly experimental and early in progress. Use it at your own risk.***






## General Tips

**It is recommended to run the code with Administrator privileges, otherwise it may experience instabilities or simply not run.**

As the project isn't properly documented yet, here are some guidelines to work with it:

### When gathering input, the same received data may not be obtainable anymore

For example, when you call CursorFuncs.HasAnyButtonChanged(), the information about the buttons state is cleared out, so any new call to this method will report no changes until new data is gathered. This leads to the next tip:

### Store obtained data for multiple subsequent manipulations

If you need to check several input states at once, you should store that general data first, and only then perform the necessary checks. If you use the method calls directly in your conditions, you will start to face inconsistent data all around.

### Favour the library implementations over manual/custom ones

Calls to Keyboard.IsKeyPressed() and Keyboard.IsKeyToggled(), when done together, should be dropped in favour of Keyboard.IsKeyPressedOrToggled(), as it uses a single system call to retrieve both informations. This improves performance and consistency in the gathered data.
