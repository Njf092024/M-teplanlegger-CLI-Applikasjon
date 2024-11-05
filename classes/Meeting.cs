using System;
using System.Collections.Generic;

namespace Møteplanlegger_CLI_Applikasjon
{
    public class Meeting
    {
        public List<string> Participants { get; set; } = new List<string>();
        public DateTime DateTime { get; set; }

        public Meeting(List<string> participants, DateTime dateTime)
        {
            Participants = participants;
            DateTime = dateTime;
        }

        public Meeting() { }
    }
}