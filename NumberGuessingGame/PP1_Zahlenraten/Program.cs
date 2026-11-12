using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[Serializable]
public class HighScore
{
    // Eigenschaften für Highscore-Eintrag
    public int Attempts { get; set; }
    public string PlayerName { get; set; }
    public string Difficulty { get; set; }
}
class Program
{
    static void Main()
    {
        // Highscores werden geladen
        HighScoreManager.Load();

        // Zufallsgenerator erzeugen
        Random random = new Random();

        // Variable für die obere Grenze (wird je nach Schwierigkeitsgrad gesetzt)
        int upperLimit = 10;

        // Begrüßung und Namenseingabe
        Console.WriteLine("Willkommen zum Zahlenraten-Spiel!");
        Console.Write("Bitte gib deinen Namen oder deine Initialen ein: ");
        string playerName = Console.ReadLine();

        // Schwierigkeitsgrad wählen
        Console.WriteLine("Wähle einen Schwierigkeitsgrad:");
        Console.WriteLine("1 = Leicht (1-10)");
        Console.WriteLine("2 = Mittel (1-50)");
        Console.WriteLine("3 = Schwer (1-100)");
        Console.Write("Deine Wahl: ");

        // Benutzereingabe für Schwierigkeitsgrad
        string difficultyInput = Console.ReadLine();
        string difficultyName;

        // Benutzerwahl auswerten
        switch (difficultyInput)
        {
            case "1":
                upperLimit = 10;
                difficultyName = "Leicht";
                Console.WriteLine("Du spielst im leichten Modus (1–10).");
                break;
            case "2":
                upperLimit = 50;
                difficultyName = "Mittel";
                Console.WriteLine("Du spielst im mittleren Modus (1–50).");
                break;
            case "3":
                upperLimit = 100;
                difficultyName = "Schwer";
                Console.WriteLine("Du spielst im schweren Modus (1–100).");
                break;
            default:
                Console.WriteLine("Ungültige Eingabe – es wird der leichte Modus (1–10) verwendet.");
                upperLimit = 10;
                difficultyName = "Leicht";
                break;
        }

        // Zufallszahl im gewählten Bereich erzeugen
        int numberToGuess = random.Next(1, upperLimit + 1);

        // Initialisierung der Variablen für Benutzereingaben
        int userGuess = 0;
        int attempts = 0;

        Console.WriteLine($"Ich habe eine Zahl zwischen 1 und {upperLimit} ausgewählt. Kannst du sie erraten?");

        // Hauptspiel-Schleife
        while (userGuess != numberToGuess)
        {
            Console.Write("Dein Tipp: ");
            string input = Console.ReadLine();

            // Prüfen, ob Eingabe eine gültige ganze Zahl ist
            if (int.TryParse(input, out userGuess))
            {
                // Prüfen, ob die Zahl im gültigen Bereich liegt
                if (userGuess < 1 || userGuess > upperLimit)
                {
                    Console.WriteLine($"Bitte gib eine Zahl zwischen 1 und {upperLimit} ein.");
                    continue; // Überspringt den Rest der Schleife und fragt erneut
                }

                attempts++; // Versuch zählen

                // Feedback geben
                if (userGuess < numberToGuess)
                {
                    Console.WriteLine("Zu niedrig! Versuch es nochmal.");
                }
                else if (userGuess > numberToGuess)
                {
                    Console.WriteLine("Zu hoch! Versuch es nochmal.");
                }
                else
                {
                    Console.WriteLine($"Glückwunsch {playerName}! Du hast die Zahl {numberToGuess} in {attempts} Versuchen erraten!");

                    // Highscore speichern
                    HighScoreManager.AddScore(new HighScore
                    {
                        PlayerName = playerName,
                        Attempts = attempts,
                        Difficulty = difficultyName
                    });

                    Console.WriteLine("\nAktuelle Highscores:");
                    foreach (var hs in HighScoreManager.HighScores)
                        Console.WriteLine($"{hs.PlayerName,-10} | {hs.Difficulty,-7} | {hs.Attempts} Versuche");

                    break;
                }
            }
            else
            {
                Console.WriteLine("Bitte gib eine gültige Zahl ein.");
            }
        }

        // Nach dem Spiel: Option zum erneuten Spielen
        Console.WriteLine("\nMöchtest du nochmal spielen? (j/n)");
        string playAgain = Console.ReadLine();

        if (playAgain.ToLower() == "j")
        {
            Console.Clear(); // Bildschirm leeren
            Main(); // Spiel neu starten
        }
        else
        {
            Console.WriteLine("Danke fürs Spielen! Bis zum nächsten Mal!");
        }
    }
}

public static class HighScoreManager
{
    // Dateiname für die Highscore-Datei
    private static readonly string FileName = "highscores.xml";
    // Liste der Highscores
    public static List<HighScore> HighScores { get; private set; } = new List<HighScore>();

    public static void Load()
    {
        // Überprüfen, ob die Datei existiert
        if (File.Exists(FileName))
        {
            var serializer = new XmlSerializer(typeof(List<HighScore>), new XmlRootAttribute("HighScores"));
            using (var reader = new StreamReader(FileName))
            {
                HighScores = (List<HighScore>)serializer.Deserialize(reader);
            }
        }
        // Highscores sortieren und begrenzen
        SortAndLimit();
    }

    public static void Save()
    {
        // Highscores sortieren und begrenzen
        var serializer = new XmlSerializer(typeof(List<HighScore>), new XmlRootAttribute("HighScores"));
        using (var writer = new StreamWriter(FileName, false))
        {
            serializer.Serialize(writer, HighScores);
        }
    }

    public static void AddScore(HighScore score)
    {
        // Neuen Highscore hinzufügen
        HighScores.Add(score);
        Save();
    }

    // Hilfsmethode zur Bestimmung der Sortierreihenfolge der Schwierigkeitsgrade
    private static int DifficultyOrder(string diff) =>
        diff switch
        {
            "Leicht" => 1,
            "Mittel" => 2,
            "Schwer" => 3,
            _ => 99
        };

    private static void SortAndLimit()
    {
        // Highscores nach Schwierigkeitsgrad und Versuchen sortieren
        HighScores = HighScores
            .OrderBy(h => DifficultyOrder(h.Difficulty))
            .ThenBy(h => h.Attempts)
            .ToList();

        // Auf die Top 5 pro Schwierigkeitsgrad begrenzen
        HighScores = HighScores
            .GroupBy(h => h.Difficulty)
            .SelectMany(g => g.Take(5)) // nur Top 5 pro Schwierigkeitsgrad
            .ToList();
    }
}
