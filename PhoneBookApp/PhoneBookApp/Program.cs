using PhoneBookApp.Classes;
using PhoneBookApp.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace PhoneDirectory
{
    static class PhoneDirectoryManager
    {
        public static Dictionary<Contact, List<Call>> PhoneDirectory { get; } = new Dictionary<Contact, List<Call>>();
    }
    class Program
    {
        static void Main(string[] args)
        {

            Call call1 = new Call(DateTime.Now.AddHours(-2), CallStatus.Completed);
            Call call2 = new Call(DateTime.Now.AddDays(-9), CallStatus.Completed);
            Call call3 = new Call(DateTime.Now.AddHours(-4), CallStatus.Missed);

            Contact contact1 = new Contact("Iva Ivic", 09878777, ContactPreference.Favorite);
            Contact contact2 = new Contact("Pera Peric", 0981654321, ContactPreference.Normal);
            Contact contact3 = new Contact("Mara Maric", 0981234567, ContactPreference.Blocked);

            PhoneDirectoryManager.PhoneDirectory[contact1] = new List<Call> { call1, call2 };
            PhoneDirectoryManager.PhoneDirectory[contact2] = new List<Call> { call2 };
            PhoneDirectoryManager.PhoneDirectory[contact3] = new List<Call> { call3, call2 };

            var exit = true;
            while (exit)
            {

                Console.WriteLine("\n\tTelefonski imenik\n\nUnesi broj za odabir:\n\n1 - Ispis svih kontakata\n" +
                    "2 - Dodavanje novih kontakata u imenik\n3 - Brisanje korisnika iz imenika\n" +
                    "4 - Editiranje preference kontakta\n5 - Upravljanje kontaktom i podmenu\n" +
                    "6 - Ispis svih poziva\n7 - Izlaz iz aplikacije ");

                var input = Console.ReadLine();

                if (int.TryParse(input, out int choice) && choice >= 1 && choice <= 7)
                {
                    switch (choice)
                    {
                        case 1:
                            DisplayContacts();
                            break;
                        case 2:
                            AddContact();
                            break;
                        case 3:
                            DeleteContact();
                            break;
                        case 4:
                            EditContactPreference();
                            break;
                        case 5:
                            ManageContact();
                            break;
                        case 6:
                            DisplayCalls();
                            break;
                        case 7:
                            Console.WriteLine("Lijep pozdrav");
                            exit = false;
                            break;
                        default:                     
                            Console.WriteLine("Pogresan unos. Molim pokusajte ponovno");                       
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Pogresan unos. Molim pokusajte ponovno");
                }
            }

            static void DisplayContacts()
            {
                Console.WriteLine("Popis svih kontakata:");
                foreach (var contact in PhoneDirectoryManager.PhoneDirectory.Keys)
                {
                    Console.WriteLine($"\nIme i prezime: {contact.FirstAndLastName}, Broj mobitela: {contact.PhoneNumber}, Preferenca: {contact.Preference}");
                    Console.WriteLine("\nPozivi:");

                    foreach (var call in PhoneDirectoryManager.PhoneDirectory[contact])
                    {
                        Console.WriteLine($"Vrijeme poziva: {call.CallTime}, Status: {call.Status}");
                    }
                }
            }

            static void AddContact()
            {
                Console.WriteLine("Unesite ime i prezime novog kontakta:");
                var firstAndLastName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(firstAndLastName) && IsValidName(firstAndLastName))
                {
                    Console.WriteLine("Unesite broj mobitela:");

                    if (long.TryParse(Console.ReadLine(), out long phoneNumber) && phoneNumber.ToString().Length > 6)
                    {
                        Console.WriteLine("Unesite preferencu za kontakt (0 - Favorite, 1 - Normal, 2 - Blocked):");
                        if (Enum.TryParse(Console.ReadLine(), out ContactPreference preferenca) && preferenca >= 0 && preferenca <= ContactPreference.Blocked)
                        {
                            Contact newContact = new Contact(firstAndLastName, phoneNumber, preferenca);


                            if (!PhoneDirectoryManager.PhoneDirectory.ContainsKey(newContact))
                            {
                                PhoneDirectoryManager.PhoneDirectory[newContact] = new List<Call>();
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
                return !string.IsNullOrWhiteSpace(name) && name.Length >= 3 && name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
            }

            static void DeleteContact()
            {
                Console.WriteLine("\nUnesite ime kontakta kojeg zelite izbrisati:");
                var contactNameToDelete = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(contactNameToDelete))
                {
                    RemoveContactFromTheDirectory(contactNameToDelete);                 
                }
                else
                {
                    Console.WriteLine("Uneseno ime nije ispravno.");
                }
            }

            static void RemoveContactFromTheDirectory(string contactNameToDelete)
            {
                var contactToDelete = PhoneDirectoryManager.PhoneDirectory.Keys.FirstOrDefault(contact => contact.FirstAndLastName == contactNameToDelete);

                if (contactToDelete != null)
                {
                    PhoneDirectoryManager.PhoneDirectory.Remove(contactToDelete);
                    Console.WriteLine($"\nKontakt {contactToDelete.FirstAndLastName} ste uspjesno izbrisali iz imenika.");
                }
                else
                {
                    Console.WriteLine("Uneseni kontakt ne postoji u imeniku.");
                }
            }

            static void EditContactPreference()
            {
                Console.WriteLine("Kojem kontaktu zelis izmjeniti preferencu:");
                var contactToEditPreference = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(contactToEditPreference))
                {
                    CheckIfContactExistsAndChangeItsPreference(contactToEditPreference);
                }
                else
                {
                    Console.WriteLine("Uneseno ime nije ispravno.");
                }
            }

            static void CheckIfContactExistsAndChangeItsPreference(string userInput)
            {
                var contactToEdit = PhoneDirectoryManager.PhoneDirectory.Keys.FirstOrDefault(contact => contact.FirstAndLastName == userInput);

                if (contactToEdit != null)
                {
                    Console.WriteLine($"Trenutna preferenca {contactToEdit.FirstAndLastName} je {contactToEdit.Preference}");

                    Console.WriteLine("Unesite novu preferencu za kontakt (0 - Favorite, 1 - Normal, 2 - Blocked):");

                    if (Enum.TryParse(Console.ReadLine(), out ContactPreference newPreference) && newPreference < ContactPreference.Blocked)
                    {
                        contactToEdit.Preference = newPreference;
                        Console.WriteLine($"\nKontaktu {contactToEdit.FirstAndLastName} ste uspjesno promjenili preferencu.");
                    }
                    else
                    {
                        Console.WriteLine("Neispravan unos preference.");
                    }
                }
                else
                {
                    Console.WriteLine("Uneseni kontakt ne postoji u imeniku.");
                }
            }

            static void ManageContact()
            {
                Console.WriteLine("Unesite kontakt cijim pozivima zelite upravljati.");
                var contactToManage = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(contactToManage))
                {
                    Submenu(contactToManage);
                }
                else
                {
                    Console.WriteLine("Uneseno ime nije ispravno.");
                }
            }

            static void Submenu(string contactToManage)
            {
                var exit = true;
                while (exit)
                {
                    Console.WriteLine("\nUnesite zeljenu akciju:\n\n1 - Ispis svih poziva\n2 - Kreiranje novog poziva\n3 - Izlaz");
                    var action = Console.ReadLine();

                    if (int.TryParse(action, out int choice) && choice >= 1 && choice <= 3)
                    {
                        switch (choice)
                        {
                            case 1:
                                PrintCalls(contactToManage);
                                break;
                            case 2:
                                CreateNewCall(contactToManage);
                                break;
                            case 3:
                                exit = false;
                                break;
                            default:
                                Console.WriteLine("Pogresan unos. Molim pokusajte ponovno.");
                                break;
                        }

                    }
                }
            }
        
            static void PrintCalls(string contactToManage)
            {
                var contactCalls = PhoneDirectoryManager.PhoneDirectory.Keys.FirstOrDefault(contact => contact.FirstAndLastName == contactToManage);


                if (contactCalls != null && PhoneDirectoryManager.PhoneDirectory.TryGetValue(contactCalls, out var calls))
                {
                    var sortedCalls = calls.OrderByDescending(contact => contact.CallTime).ToList();

                    foreach (var call in sortedCalls)
                    {
                        Console.WriteLine($"Vrijeme poziva: {call.CallTime}, Status: {call.Status}");
                    }
                }
                else
                {
                    Console.WriteLine("Taj kontakt ne postoji u imeniku.");
                }
            }

            static void CreateNewCall(string contactToManage)
            {
                var contact = PhoneDirectoryManager.PhoneDirectory.Keys.FirstOrDefault
                    (contact => contact.FirstAndLastName == contactToManage);

                if (contact != null && PhoneDirectoryManager.PhoneDirectory.TryGetValue(contact, out var calls))
                {
                    if (contact.Preference == ContactPreference.Blocked)
                    {
                        Console.WriteLine("Kontakt je blokiran i s njim nije moguce uspostaviti kontakt.");
                        return;
                    }

                    var randomDuration = new Random().Next(1, 21);

                    SimulateCall(calls, randomDuration);
                }
                else
                {
                    Console.WriteLine("Taj kontakt ne postoji u imeniku.");
                }
            }

            static void SimulateCall(List<Call> calls, int randomDuration)
            {
                var random = new Random();
                var randomStatus = (CallStatus)random.Next(0, 3);

                var newCall = new Call(DateTime.Now, CallStatus.InProgress);
                calls.Add(newCall);

                Console.WriteLine($"Poziv je uspostavljen. Trajanje poziva: {randomDuration} sekundi, Status odgovora: {randomStatus}");
                System.Threading.Thread.Sleep(randomDuration * 1000);

                newCall.Status = CallStatus.Completed;
                Console.WriteLine("Poziv je zavrsen.");
            }

            static void DisplayCalls()
            {
                Console.WriteLine("Popis svih poziva:");
                foreach (var contact in PhoneDirectoryManager.PhoneDirectory.Keys)
                {
                    var sortedCalls = PhoneDirectoryManager.PhoneDirectory[contact].OrderBy(call => call.CallTime).ToList();

                    foreach (var call in sortedCalls)
                    {
                        Console.WriteLine($"Vrijeme poziva: {call.CallTime}, Status: {call.Status}");
                    }
                }
            }


        }
    }
}

