using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetupReminder.Core.Services;
using System.IO;

namespace MeetupReminder
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the name of the group you want to see events for");
            string groupname = Console.ReadLine();

            var meetups = MeetupServices.GetMeetupsFor(groupname).Result;

            var message = "";

            foreach (var meetup in meetups)
            {
                message += meetup.name;
                message += meetup.time;
            }

            SmsService.SendSMS("+17038640171", message);
            Console.ReadLine();
        }
    }
}
