namespace Sugarmaple.TheSeed.Api;

/// <summary>
/// 위키에서 올바르지 않은 문서명을 받을 때 throw되는 예외입니다.
/// </summary>
public class InvalidDocumentException : WikiException
{
    /// <summary>
    /// 작성한 문서명입니다.
    /// </summary>
    public string DocumentName { get; private set; }

    internal InvalidDocumentException(string docName, string status) : base("The name of document can't be null or white space.", status)
    {
        DocumentName = docName;
    }
}

/// <summary>
/// 편집 시작 이후 다른 편집자가 먼저 편집을 완료할 때 throw되는 예외입니다.
/// </summary>
public class EditConflictException : WikiException
{
    internal EditConflictException(string editToken) : base($"During editing, another user edited first. Your edit token was \"{editToken}\".")
    {
    }
}

/// <summary>
/// 접근 권한이 부족한 문서에 작업을 시도할 때 throw되는 예외입니다.
/// </summary>
public class AccessLevelLacksException : WikiException
{
    internal AccessLevelLacksException(string status) : base($"Your access level lacks to edit this document.")
    {
    }
}

public class WikiException : Exception
{
    /// <summary>
    /// 반환된 json 파일의 Status 값입니다.
    /// </summary>
    public string Status { get; private set; }

    internal WikiException(string message) : this(message, "")
    {

    }

    internal WikiException(string message, string status) : base(message)
    {
        Status = status;
    }
}