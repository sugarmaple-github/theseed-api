namespace Sugarmaple.TheSeed.Api;
public class InvalidApiTokenException : WikiException
{
    internal InvalidApiTokenException() : this($"api token is not valid.")
    {

    }

    internal InvalidApiTokenException(bool expired = false) : this(expired ? ExpiredMessage() : InvalidEntryMessage())
    {

    }

    internal InvalidApiTokenException(string message) : base(message)
    {

    }

    internal InvalidApiTokenException(string message, string status) : base(message, status)
    {

    }

    private static string InvalidEntryMessage() => $"Api token is not valid.";
    private static string ExpiredMessage() => $"Api token is expired.";
}