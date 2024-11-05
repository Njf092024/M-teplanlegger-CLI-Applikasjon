using System.Text.Json;
using Møteplanlegger_CLI_Applikasjon;

namespace Møteplanlegger_CLI_Applikasjon;
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text.Json;
}

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
                    exit = true;
                    Console.WriteLine("Nope. Press any key to try again.");
                    Console.ReadKey();
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
            Console.WriteLine($"exception.Message\n");
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

        Console.WriteLine("Enter the names of participants, seperated by commas:");
        string? input = Console.ReadLine();
        List<string> participants = new List<string>(input?.Split(',') ?? Array.Empty<string>());

        Console.WriteLine("Enter the meeting time (yyyy-MM-dd HH:mm):");
        string? dateTimeInput = Console.ReadLine();
        DateTime dateTime;
        while (!DateTime.TryParse(dateTimeInput, out dateTime))
        {
            Console.WriteLine("Incorrect date. Please try again (yyyy-MM-dd HH:mm):");
            dateTimeInput = Console.ReadLine();
        }

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
}
