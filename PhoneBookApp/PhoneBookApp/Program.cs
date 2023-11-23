using PhoneBookApp.Classes;
using PhoneBookApp.Enums;
using System;
using System.Collections.Generic;

namespace PhoneDirectory
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<Contact, List<Call>> phoneDirectory = new Dictionary<Contact, List<Call>>();

            Call call1 = new Call(DateTime.Now.AddHours(-2), CallStatus.Completed);
            Call call2 = new Call(DateTime.Now.AddHours(-9), CallStatus.Completed);
            Call call3 = new Call(DateTime.Now.AddHours(-4), CallStatus.Missed);

            Contact contact1 = new Contact("Iva Ivic", 09878777, ContactPreference.Favorite);
            Contact contact2 = new Contact("Pera Peric", 0981654321, ContactPreference.Normal);
            Contact contact3 = new Contact("Mara Maric", 0981234567, ContactPreference.Blocked);

            phoneDirectory[contact1] = new List<Call> { call1, call2 };
            phoneDirectory[contact2] = new List<Call> { call2 };
            phoneDirectory[contact3] = new List<Call> { call3, call2};

            var exit = true;
            while (exit)
            {

                Console.WriteLine("\tTelefonski imenik\n\nUnesi broj za odabir:\n1 - Ispis svih kontakata\n" +
                    "2 - Dodavanje novih kontakata u imenik\n3 - Brisanje korisnika iz imenika\n" +
                    "4 - Editiranje preference kontakta\n5 - Upravljanje kontaktom i poddomena\n" +
                    "6 - Ispis svih poziva\n7 - Izlaz iz aplikacije ");

                var input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 7)
                {
                    switch (choice)
                    {
                        case 1:
                            DisplayContacts(phoneDirectory);
                            break;
                        case 2:
                            AddContact(phoneDirectory);
                            break;
                        case 3:
                            //DeleteContact();
                            break;
                        case 4:
                            //EditContact()
                            break;
                        case 5:
                            //MenageContact()
                            break;
                        case 6:
                            //DisplayCalls()
                            break;
                        case 7:
                            Console.WriteLine("Lijep pozdrav");
                            exit = false;
                            break;
                        default:
                            Console.WriteLine("Pogresan unos. Molim pokusajte ponovno");
                            break;
                    }
                } else {
                    Console.WriteLine("Pogresan unos. Molim pokusajte ponovno");
                }
            }

            static void DisplayContacts(Dictionary<Contact, List<Call>> phoneBook)
            {
                Console.WriteLine("Popis svih kontakata:");
                foreach (var contact in phoneBook.Keys)
                {
                    Console.WriteLine($"\nIme i prezime: {contact.FirstAndLastName}, Broj mobitela: {contact.PhoneNumber}, Preferenca: {contact.Preference}");
                    Console.WriteLine("\nPozivi:");

                    foreach (var call in phoneBook[contact])
                    {
                        Console.WriteLine($"Vrijeme poziva: {call.CallTime}, Status: {call.Status}");
                    }
                }
            }

            static void AddContact(Dictionary<Contact, List<Call>> phoneDirectory)
            {
                Console.WriteLine("Unesite ime i prezime novog kontakta:");
                var firstAndLastName = Console.ReadLine();

                if (IsValidName(firstAndLastName))
                {
                    Console.WriteLine("Unesite broj mobitela:");

                    if (long.TryParse(Console.ReadLine(), out long phoneNumber) && phoneNumber.ToString().Length > 6)
                    {
                        Console.WriteLine("Unesite preferencu za kontakt (0 - Favorite, 1 - Normal, 2 - Blocked):");
                        if (Enum.TryParse(Console.ReadLine(), out ContactPreference preferenca) && preferenca < ContactPreference.Blocked)
                        {
                            Contact newContact = new Contact(firstAndLastName, phoneNumber, preferenca);

                            if (!phoneDirectory.ContainsKey(newContact))
                            {
                                phoneDirectory[newContact] = new List<Call>();
                                Console.WriteLine("Novi kontakt dodan.");
                            }
                            else
                            {
                                Console.WriteLine("Kontakt vec postoji u imeniku.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Neispravan unos preference.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Neispravan unos broja mobitela.");
                    }
                }
                else
                {
                    Console.WriteLine("Neispravan unos imena.");
                }
            }

            static bool IsValidName(string name) 
            {
                return !string.IsNullOrWhiteSpace(name) && name.Length >= 3 && name.All(char.IsLetter);
            }


        }
    }
}

