namespace Sugarmaple.TheSeed.Api;
using System;

/// <summary>
/// 제공받은 편집 토큰과 클라이언트의 편집 기능을 관리하는 객체입니다.
/// </summary>
public class EditView
{
    private readonly SeedJsonClient _client;
    private readonly string _document;
    private string? _token;

    internal EditView(SeedJsonClient client,
        string document, string text, bool exists, string token)
    {
        _client = client;
        _document = document;
        Text = text;
        Exist = exists;
        _token = token;
    }

    /// <summary>
    /// 열람한 문서의 내용입니다.
    /// </summary>
    public string Text { get; private set; }
    /// <summary>
    /// 해당 문서의 존재 여부입니다.
    /// </summary>
    public bool Exist { get; private set; }

    /// <summary>
    /// 기존 문서를 재열람합니다. text와 token이 갱신됩니다.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="InvalidApiTokenException"></exception>
    internal async Task GetReviewAsync()
    {
        (string text, bool exists, string token, string status) = await _client.GetEditAsync(_document);
        if (status == "문서 이름이 올바르지 않습니다.")
            throw new InvalidApiTokenException();

        Text = text;
        Exist = exists;
        _token = token;
    }

    /// <summary>
    /// 편집을 제출하고 토큰을 파기합니다. 같은 문서를 추가로 편집을 위해서는 Review를 호출해야 합니다.
    /// </summary>
    /// <param name="text"></param>
    /// <param name="log"></param>
    /// <returns>편집 정보를 반환합니다.</returns>
    /// <exception cref="ArgumentNullException">매개변수에 null값이 제공될 경우.</exception>
    public async Task<EditResult> PostEditAsync(string text, string log)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(log);
        if (_token == null) throw new InvalidOperationException("The edit token was expired. You need to get another edit token. Call \"Review()\" first.");

        //{ Status = 편집 도중에 다른 사용자가 먼저 편집을 했습니다., Rev = 0 }
        (var status, var rev) = await _client.PostEditAsync(_document, text, log, _token);
        if (status == "success")
        {
            _token = null;
            return new(rev);
        }

        if (status == "편집 도중에 다른 사용자가 먼저 편집을 했습니다.")
            throw new EditConflictException(_token);

        throw new NotImplementedException($"This version didn't implemented in case that {nameof(status)} is \"{status}\". Please report on issue.");
    }
}

public class EditResult
{
    internal EditResult(int rev)
    {
        Rev = rev;
    }

    public int Rev { get; private set; }
}