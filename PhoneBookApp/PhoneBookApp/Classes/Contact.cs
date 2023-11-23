using PhoneBookApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Classes
{
    public class Contact
    {
        public string FirstAndLastName { get; set; }
        public long PhoneNumber { get; set; }
        public ContactPreference Preference { get; set; }

        public Contact(string firstAndLastName, long phoneNumber, ContactPreference preference)
        {
            FirstAndLastName = firstAndLastName;
            PhoneNumber = phoneNumber;
            Preference = preference;
        }
    }
}
