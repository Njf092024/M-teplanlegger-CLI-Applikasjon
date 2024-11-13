    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
    using static system.Console;

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
                Clear();
                Console.WriteLine("Main menu");
                Console.WriteLine("1. Schedule a new meeting");
                Console.WriteLine("2. List all meetings");
                Console.WriteLine("3. Exit");
                Console.Write("Choose an option: ");

                switch (Console.ReadLine())
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
            Console.WriteLine($"An error occured while attempting to write to the file meetings.json: {exception.Message}");
        }
        catch (Exception exception)
        {
            Console.WriteLine($" An error occured: {exception.Message}\n");
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

        Console.WriteLine("Enter the names of participants, seperated by commas:");
        string? input = Console.ReadLine();
        List<string> participants = new List<string>(input?.Split(',') ?? Array.Empty<string>());

        Console.WriteLine("Enter the meeting time (MM-dd):");
        string? dateTimeInput = Console.ReadLine();
        DateTime dateTime;
        while (!DateTime.TryParse(dateTimeInput + " " +DateTime.Now.Year, out dateTime))
        {
            Console.WriteLine("Incorrect date. Please try again (MM-dd):");
            dateTimeInput = Console.ReadLine();
        }

        dateTime = dateTime.Date.AddHours(12);

        var newMeeting = new Meeting{
            Participants = participants,
            DateTime = dateTime,
        };

        meetings.Add(newMeeting);

        string json = JsonSerializer.Serialize(meetings, new JsonSerializerOptions { WriteIndented = true});
        File.WriteAllText(filePath, json);

        Console.WriteLine("Meeting succesfully scheduled and saved!");
        Console.WriteLine("Press any key to return to main menu...");
        Console.ReadKey();
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
                    Console.WriteLine("No meetings found in file, that's depressing.");
                }
                else
                {
                    Console.WriteLine("List of scheduled meetings:");
                    foreach (var meeting in meetings)
                    {
                        Console.WriteLine($"Date: {meeting.DateTime.ToString("MM-dd-yyyy")}, Participants: {string.Join(", ", meeting.Participants)}");
                    }
                }
            }
            else
            {
                Console.WriteLine("No meetings found, yikes.");
            }
        }
        else
        {
            Console.WriteLine("File not found.");
        }

        Console.WriteLine("Press any key to return to the main menu...");
        Console.ReadKey();
    }
}
}