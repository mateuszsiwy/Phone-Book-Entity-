using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class UserInterface
{

    

    public UserInterface() 
    {
        while (true)
        {
            var options = new Dictionary<string, Action>
        {
            {"Add", () => Add() },
            {"Delete", () => Delete() },
            {"Update", () => Update() },
            {"Read", () => Read() }
        };

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Welcome to the Phone Book, please select one of the options!")
                    .PageSize(10)
                    .AddChoices(options.Keys));

            options[choice].Invoke();
        }
        
    }

    public void Add()
    {
        using var db = new ContactContext();
        var name = AnsiConsole.Prompt(new TextPrompt<string>("Enter a name"));
        var email = AnsiConsole.Prompt(new TextPrompt<string>("Enter email"));
        while (!Validation.ValidateEmail(email))
        {
            email = AnsiConsole.Prompt(new TextPrompt<string>("Entered email is not correct, enter a new one"));
        }
        var phone = AnsiConsole.Prompt(new TextPrompt<string>("Enter phone number"));
        while (!Validation.ValidatePhoneNumber(phone))
        {
            phone = AnsiConsole.Prompt(new TextPrompt<string>("Entered phone number is not correct, enter a new one"));
        }
        db.Add(new  Contact { Name = name, Email = email, PhoneNumber = phone });
        db.SaveChanges();
    }

    public void Delete() 
    {
        using var db = new ContactContext();
        var name = AnsiConsole.Prompt(new TextPrompt<string>("Enter a name that you want to delete"));
        var contact = db.Contacts.Where(x => x.Name == name).FirstOrDefault();
        
        if (contact != null)
        {
            db.Contacts.Remove(contact);
            db.SaveChanges();
        }
        else
        {
            Console.WriteLine("Name not in database");
        }
    }

    public void Update() 
    {
        using var db = new ContactContext();
        var id = AnsiConsole.Prompt(new TextPrompt<string>("Enter a Id that you want to update"));
        var contact = db.Contacts.Where(x=> x.Id.ToString() == id).FirstOrDefault();
        if (contact != null)
        {
            var name = AnsiConsole.Prompt(new TextPrompt<string>("Enter name"));
            var email = AnsiConsole.Prompt(new TextPrompt<string>("Enter email"));
            while (!Validation.ValidateEmail(email))
            {
                email = AnsiConsole.Prompt(new TextPrompt<string>("Entered email is not correct, enter a new one"));
            }
            var phone = AnsiConsole.Prompt(new TextPrompt<string>("Enter phone number"));
            while (!Validation.ValidatePhoneNumber(phone))
            {
                phone = AnsiConsole.Prompt(new TextPrompt<string>("Entered phone number is not correct, enter a new one"));
            }
            contact.Name = name;
            contact.Email = email;
            contact.PhoneNumber = phone;
            db.SaveChanges();
        }
        else
        {
            Console.WriteLine("Name not in database");
        }
        
    }

    public void Read() 
    {
        using var db = new ContactContext();
        List<Contact> contacts = db.Contacts.ToList();
        var table = new Table();
        table.AddColumn("Id");
        table.AddColumn("Name");
        table.AddColumn("Email");
        table.AddColumn("Phone number");
        foreach (var item in contacts)
        {
            table.AddRow(item.Id.ToString(), item.Name, item.Email, item.PhoneNumber);
        }

        AnsiConsole.Write(table);
    }
}

