    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using static System.Console;
    using Spectre.Console;

namespace Møteplanlegger_CLI_Applikasjon
{

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string filePath = "meetings.json";
            bool exit = false;

            while (!exit)
            {
                AnsiConsole.Clear();
                AnsiConsole.MarkUpLine("[bold yellow]Main menu[/]");
                AnsiConsole.MarkUpLine("[green]1.[/] Schedule a new meeting");
                AnsiConsole.MarkUpLine("[green]2.[/] List all meetings");
                AnsiConsole.MarkUpLine("[green]3.[/] Exit");
                string option = AnsiConsole.Ask<string>("Choose an option: ");
                

                switch (ReadLine())
                {
                    case "1":
                    ScheduleNewMeeting(filePath);
                    break;
                    case "2":
                    ListAllMeetings(filePath);
                    break;
                    case "3":
                    Clear();
                    exit = true;
                    break;
                }
            }
        }
        catch (IOException exception)
        {
            WriteLine($"An error occured while attempting to write to the file meetings.json: {exception.Message}");
        }
        catch (Exception exception)
        {
            WriteLine($" An error occured: {exception.Message}\n");
        }
    }

    static void ScheduleNewMeeting(string filePath)
    {
        Clear();

        List<Meeting> meetings = new List<Meeting>();

        if (File.Exists(filePath))
        {
            string? existingJSON = File.ReadAllText(filePath);
            if (!string.IsNullOrWhiteSpace(existingJSON))
            {
                meetings = JsonSerializer.Deserialize<List<Meeting>>(existingJSON) ?? new List<Meeting>();
            }
        }

        WriteLine("Enter the names of participants, seperated by commas:");
        string? input = ReadLine();
        List<string> participants = new List<string>(input?.Split(',') ?? Array.Empty<string>());

        WriteLine("Enter the meeting time (MM-dd):");
        string? dateTimeInput = ReadLine();
        DateTime dateTime;
        while (!DateTime.TryParse(dateTimeInput + " " +DateTime.Now.Year, out dateTime))
        {
            WriteLine("Incorrect date. Please try again (MM-dd):");
            dateTimeInput = ReadLine();
        }

        dateTime = dateTime.Date.AddHours(12);

        var newMeeting = new Meeting{
            Participants = participants,
            DateTime = dateTime,
        };

        meetings.Add(newMeeting);

        string json = JsonSerializer.Serialize(meetings, new JsonSerializerOptions { WriteIndented = true});
        File.WriteAllText(filePath, json);

        WriteLine("Meeting succesfully scheduled and saved!");
        WriteLine("Press any key to return to main menu...");
        ReadKey();
    }

    static void ListAllMeetings(string filePath)
    {
        Clear();

        if (File.Exists(filePath))
        {
            string? existingJSON = File.ReadAllText(filePath);
            if (!string.IsNullOrWhiteSpace(existingJSON))
            {
                List<Meeting> meetings = JsonSerializer.Deserialize<List<Meeting>>(existingJSON) ?? new List<Meeting>();

                if (meetings.Count == 0)
                {
                    WriteLine("No meetings found in file, that's depressing.");
                }
                else
                {
                    WriteLine("List of scheduled meetings:");
                    foreach (var meeting in meetings)
                    {
                        WriteLine($"Date: {meeting.DateTime.ToString("MM-dd-yyyy")}, Participants: {string.Join(", ", meeting.Participants)}");
                    }
                }
            }
            else
            {
                WriteLine("No meetings found, yikes.");
            }
        }
        else
        {
            WriteLine("File not found.");
        }

        WriteLine("Press any key to return to the main menu...");
        ReadKey();
    }
}
}