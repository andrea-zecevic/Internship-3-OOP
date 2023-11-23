using PhoneBookApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Classes
{
    public class Call
    {
        public DateTime CallTime { get; set; }
        public CallStatus Status { get; set; }

        public Call(DateTime callTime, CallStatus status)
        {
            CallTime = callTime;
            Status = status;
        }
    }
}
