    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
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
                Console.Clear();
                AnsiConsole.MarkupLine("[bold yellow]Main menu[/]");
                AnsiConsole.MarkupLine("[green]1.[/] Schedule a new meeting");
                AnsiConsole.MarkupLine("[green]2.[/] List all meetings");
                AnsiConsole.MarkupLine("[green]3.[/] Exit");
                string option = AnsiConsole.Ask<string>("Choose an option: ");
                

                switch (option)
                {
                    case "1":
                    ScheduleNewMeeting(filePath);
                    break;
                    case "2":
                    ListAllMeetings(filePath);
                    break;
                    case "3":
                    Console.Clear();
                    exit = true;
                    break;
                }
            }
        }
        catch (IOException exception)
        {
            AnsiConsole.MarkupLine("[red]An error occured while attempting to write to the file meetings.json: {0}[/]", exception.Message);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine("[red]An error occured: {0}[/]", exception.Message);
        }
    }

    static void ScheduleNewMeeting(string filePath)
    {
        Console.Clear();

        List<Meeting> meetings = new List<Meeting>();

        if (File.Exists(filePath))
        {
            string? existingJSON = File.ReadAllText(filePath);
            if (!string.IsNullOrWhiteSpace(existingJSON))
            {
                meetings = JsonSerializer.Deserialize<List<Meeting>>(existingJSON) ?? new List<Meeting>();
            }
        }

        AnsiConsole.MarkupLine("Enter the names of participants, seperated by commas:");
        string? input = AnsiConsole.Ask<string>("Participants: ");
        List<string> participants = new List<string>(input?.Split(',') ?? Array.Empty<string>());

        AnsiConsole.MarkupLine("Enter the meeting time (MM-dd):");
        string? dateTimeInput = AnsiConsole.Ask<string>("Date (MM-dd)");
        DateTime dateTime;
        while (!DateTime.TryParse(dateTimeInput + " " +DateTime.Now.Year, out dateTime))
        {
            AnsiConsole.MarkupLine("[red]Incorrect date. Please try again (MM-dd):[/]");
            dateTimeInput = AnsiConsole.Ask<string>("Date (MM-dd): ");
        }

        dateTime = dateTime.Date.AddHours(12);

        var newMeeting = new Meeting{
            Participants = participants,
            DateTime = dateTime,
        };

        meetings.Add(newMeeting);

        string json = JsonSerializer.Serialize(meetings, new JsonSerializerOptions { WriteIndented = true});
        File.WriteAllText(filePath, json);

        AnsiConsole.MarkupLine("[green]Meeting succesfully scheduled and saved![/]");
        AnsiConsole.MarkupLine("Press any key to return to main menu...");
        Console.Clear();
    }

    static void ListAllMeetings(string filePath)
    {
        Console.Clear();

        if (File.Exists(filePath))
        {
            string? existingJSON = File.ReadAllText(filePath);
            if (!string.IsNullOrWhiteSpace(existingJSON))
            {
                List<Meeting> meetings = JsonSerializer.Deserialize<List<Meeting>>(existingJSON) ?? new List<Meeting>();

                if (meetings.Count == 0)
                {
                    AnsiConsole.MarkupLine("[yellow]No meetings found in file, that's depressing.[/]");
                }
                else
                {
                    AnsiConsole.MarkupLine("[green]List of scheduled meetings:[/]");
                    foreach (var meeting in meetings)
                    {
                        AnsiConsole.MarkupLine($"[cyan]Date:[/] {meeting.DateTime.ToString("MM-dd-yyyy")},  [cyan]Participants:[/] {string.Join(", ", meeting.Participants)}");
                    }
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]No meetings found, yikes.[/]");
            }
        }
        else
        {
            AnsiConsole.MarkupLine("[red]File not found.[/]");
        }

        AnsiConsole.MarkupLine("Press any key to return to the main menu...");
        Console.Clear();
    }
}
}