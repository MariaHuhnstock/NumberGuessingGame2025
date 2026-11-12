# Number Guessing Game

A console-based number guessing game developed in C# using Visual Studio.

## Project Overview

Players try to guess a randomly generated number. The game offers three difficulty levels and provides feedback after each valid guess. The number of attempts is counted, and completed games are recorded in an XML-based high score list.

## Project Background

This project was developed independently as part of my 20-week internship at Buss-IT-Labs. It was created as a practical exercise in C# application development using Visual Studio.

## Features

- Three difficulty levels:
  - Easy: numbers from 1 to 10
  - Medium: numbers from 1 to 50
  - Hard: numbers from 1 to 100
- Random number generation
- Input validation for numeric guesses
- Feedback when a guess is too high or too low
- Attempt counter
- Player name or initials
- High score storage using XML
- High score sorting and a top-five limit per difficulty when scores are loaded
- Option to play another round

## Technologies

- **Language:** C#
- **Framework:** .NET 8
- **IDE:** Visual Studio
- **Data format:** XML
- **Concepts:** Classes and objects, collections, control flow, file I/O, XML serialization, LINQ

## How to Run

1. Install the .NET 8 SDK.
2. Clone or download this repository.
3. Open `PP1_Zahlenraten.sln` in Visual Studio.
4. Build and run the project.

You can also run it from a terminal:

```bash
dotnet run --project PP1_Zahlenraten/PP1_Zahlenraten.csproj
```

## Learning Objectives

This project provided practical experience with:

- C# fundamentals and control structures
- Working with classes and objects
- Validating console input
- Managing data with `List<T>`
- Reading and writing XML files
- Sorting and grouping data with LINQ
- Building a small console application

## Future Improvements

Potential improvements for a future version:

- Replace the recursive restart call with a dedicated game loop
- Sort and limit high scores immediately after a new score is added
- Improve null and invalid-input handling
- Separate game logic, user interaction, and high score management into dedicated classes

---

*This repository preserves an early version of the project. A future revision may document refactoring and additional features as part of my programming progress.*
