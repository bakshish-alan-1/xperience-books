using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class Validator
{

    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";


    public static bool validateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }


    public static string GetValidateErrorMessage(List<string> errorString) {

        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < errorString.Count; i++) {
            
            //sb.Append(i+1+ ". <indent=15%>");
            sb.Append(errorString[i]);
            //sb.Append("</indent> \n");
        }

        return sb.ToString();
    }

}
