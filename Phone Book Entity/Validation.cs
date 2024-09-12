using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


public class Validation
{
    public static bool ValidateEmail(string email)
    {
        try
        {
            var isValid = new MailAddress(email);
            return true;
        }
        catch { return false; }
    }
    
    public static bool ValidatePhoneNumber(string number)
    {
        string pattern = @"^\+?\d{0,3}?[-. ]?\(?\d{1,4}?\)?[-. ]?\d{1,4}[-. ]?\d{1,9}$";

        bool isValid = Regex.IsMatch(number, pattern);
        return isValid;
    }
}