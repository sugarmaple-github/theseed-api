namespace Sugarmaple.TheSeed.Api;
using System;

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

    public string Text { get; private set; }
    public bool Exist { get; private set; }

    internal async Task ReviewAsync()
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
    /// <returns>편집된 판 번호을 반환합니다.</returns>
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