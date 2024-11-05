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

    
}
