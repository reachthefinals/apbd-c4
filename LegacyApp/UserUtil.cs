using System;
using System.Text.RegularExpressions;

namespace LegacyApp;

public static class UserUtil
{
    public static bool IsValidMail(string email)
    {
        var mailPattern = @"^[\w\d-_+]+@[\w\d-+]+.[a-z-]{2,}$";
        if (Regex.IsMatch(email, mailPattern)) return true;

        return false;
    }

    public static int RelativeAge(DateTime dateOfBirth)
    {
        var now = DateTime.Now;
        var age = now.Year - dateOfBirth.Year;
        if (now.Month < dateOfBirth.Month
            || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)
           )
            age--;
        return age;
    }
}