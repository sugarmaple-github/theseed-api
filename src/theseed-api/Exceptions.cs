namespace Sugarmaple.TheSeed.Api;
public class InvalidDocumentException : WikiException
{
    public string DocumentName { get; private set; }

    public InvalidDocumentException(string docName, string status) : base("The name of document can't be null or white space.", status)
    {
        DocumentName = docName;
    }
}

public class EditConflictException : WikiException
{
    public EditConflictException(string editToken) : base($"During editing, another user edited first. Your edit token was \"{editToken}\".")
    {
    }
}

public class AccessLevelLacksException : WikiException
{
    public AccessLevelLacksException(string status) : base($"Your access level lacks to edit this document.")
    {
    }
}

public class WikiException : Exception
{
    public string Status { get; private set; }

    internal WikiException(string message) : this(message, "")
    {

    }

    internal WikiException(string message, string status) : base(message)
    {
        Status = status;
    }
}