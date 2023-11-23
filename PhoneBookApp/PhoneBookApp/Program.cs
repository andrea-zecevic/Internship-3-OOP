using PhoneBookApp.Classes;
using PhoneBookApp.Enums;
using System;
using System.Collections.Generic;

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
            Call call2 = new Call(DateTime.Now.AddHours(-9), CallStatus.Completed);
            Call call3 = new Call(DateTime.Now.AddHours(-4), CallStatus.Missed);

            Contact contact1 = new Contact("Iva Ivic", 09878777, ContactPreference.Favorite);
            Contact contact2 = new Contact("Pera Peric", 0981654321, ContactPreference.Normal);
            Contact contact3 = new Contact("Mara Maric", 0981234567, ContactPreference.Blocked);

            PhoneDirectoryManager.PhoneDirectory[contact1] = new List<Call> { call1, call2 };
            PhoneDirectoryManager.PhoneDirectory[contact2] = new List<Call> { call2 };
            PhoneDirectoryManager.PhoneDirectory[contact3] = new List<Call> { call3, call2};

            var exit = true;
            while (exit)
            {

                Console.WriteLine("\n\tTelefonski imenik\n\nUnesi broj za odabir:\n1 - Ispis svih kontakata\n" +
                    "2 - Dodavanje novih kontakata u imenik\n3 - Brisanje korisnika iz imenika\n" +
                    "4 - Editiranje preference kontakta\n5 - Upravljanje kontaktom i poddomena\n" +
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
                        if (Enum.TryParse(Console.ReadLine(), out ContactPreference preferenca) && preferenca < ContactPreference.Blocked)
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
                else
                {
                    Console.WriteLine("Uneseno ime nije ispravno.");
                }
            }

            static void EditContactPreference()
            {
                Console.WriteLine("Kojem kontaktu zelis izmjeniti preferencu:");
                var contactToEditPreference = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(contactToEditPreference))
                {

                    checkIfContactExistsAndChangeItsPreference(contactToEditPreference);
                   
                }
                else
                {
                    Console.WriteLine("Uneseno ime nije ispravno.");
                }
            }

            static void checkIfContactExistsAndChangeItsPreference(string userInput)
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





        }
    }
}

