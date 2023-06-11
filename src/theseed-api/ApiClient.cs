namespace Sugarmaple.TheSeed.Api;
using System;

public class SeedApiClient
{
    private readonly SeedJsonClient _client;

    internal SeedApiClient(string wikiUri)
    {
        _client = new(wikiUri);
        _client.UpdateAuthHeader($"Bearer abc");
    }

    public SeedApiClient(string wikiUri, string apiToken) : this(wikiUri)
    {
        UpdateApiToken(apiToken);
    }

    #region Public Method
    /// <summary>
    /// 문서를 열람합니다.
    /// </summary>
    /// <param name="document">열람할 문서명입니다.</param>
    /// <returns>문서 내용을 반환합니다. 문서가 개설되지 않았다면 null을 반환합니다.</returns>
    /// <inheritdoc cref="GuardDocument(string?)"/>
    public async Task<string?> GetViewAsync(string document)
    {
        GuardDocument(document);
        (string text, bool exists, _, string status) = await _client.GetEditAsync(document);
        GuardStatus(status, nameof(document));
        return exists ? text : null;
    }

    /// <summary>
    /// 문서를 열람하고 편집 뷰를 반환합니다.
    /// </summary>
    /// <param name="document">편집할 문서명입니다.</param>
    /// <returns>편집을 시행할 수 있는 뷰를 반환합니다.</returns>
    /// <inheritdoc cref="GuardDocument(string?)"/>
    /// <inheritdoc cref="GuardStatus"/>
    public async Task<EditView> GetEditAsync(string document)
    {
        GuardDocument(document);
        (string text, bool exists, string token, string status) = await _client.GetEditAsync(document);
        GuardStatus(status, nameof(document));
        return new(_client, document, text, exists, token);
    }

    /// <summary>
    /// <paramref name="document"/>의 역링크 목록을 확인하고 이름 <paramref name="namespace" />
    /// </summary>
    /// <param name="document">역링크 목록을 확인할 문서명입니다.</param>
    /// <param name="namespace">역링크 목록을 확인할 이름 공간입니다. 만약 해당 이름 공간의 역링크가 없을 경우, 문서 이름공간의 역링크를 출력합니다.</param>
    /// <param name="from">어떤 문자열부터의 역링크 목록을 확인할 것인지 반환합니다.</param>
    /// <param name="flags">역링크의 타입을 정합니다.</param>
    /// <returns>역링크의 결과 객체를 반환합니다.</returns>
    /// <inheritdoc cref="GuardStatus"/>
    public async Task<BacklinkResult> GetBacklinkFromAsync(string document, string @namespace = "", string @from = "", BacklinkFlags flags = BacklinkFlags.All)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(@namespace);
        ArgumentNullException.ThrowIfNull(@from);

        var result = await _client.GetBacklinkFromAsync(document, @namespace, from, (int)flags);
        GuardStatus(result.Status, nameof(document));
        var ret = new BacklinkResult(_client, document, flags, result);
        return ret;
    }

    /// <inheritdoc cref="GetBacklinkFromAsync(string, string, string, BacklinkFlags)"/>
    internal Task<BacklinkResult> GetBacklinkFromAsync(string document, SeedNamespace @namespace, string @from = "", BacklinkFlags flags = BacklinkFlags.All) =>
        GetBacklinkFromAsync(document, @namespace.Name, from, flags);

    public async Task<BacklinkResult> GetBacklinkUntilAsync(string document, string @namespace, string until = "", BacklinkFlags flags = BacklinkFlags.All)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(@namespace);
        ArgumentNullException.ThrowIfNull(until);

        var result = await _client.GetBacklinkUntilAsync(document, @namespace, until, (int)flags);
        GuardStatus(result.Status, nameof(document));
        var ret = new BacklinkResult(_client, document, flags, result);
        return ret;
    }
    internal Task<BacklinkResult> GetBacklinkUntilAsync(string document, SeedNamespace @namespace, string until = "", BacklinkFlags flags = BacklinkFlags.All) =>
        GetBacklinkUntilAsync(document, @namespace.Name, until, flags);


    public void UpdateApiToken(string apiToken)
    {
        foreach (var c in apiToken)
        {
            if (!char.IsAscii(c))
                throw new ArgumentException($"{nameof(apiToken)} must contain only ASCII characters.", nameof(apiToken));
        }
        _client.UpdateAuthHeader($"Bearer {apiToken}");
    }
    #endregion

    /// <exception cref="ArgumentException"><paramref name="document"/>가 빈 문자열이거나 길이가 255를 넘습니다.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="document"/>가 null입니다.</exception>
    private static void GuardDocument(string? document)
    {
        ArgumentNullException.ThrowIfNull(document);

        if (document.Length == 0)
            throw new ArgumentException("The name of document can't be null or white space.", nameof(document));

        if (document.Length > 255)
            throw new ArgumentException("The name of document can't be over 255", nameof(document));
    }

    /// <exception cref="InvalidApiTokenException">Api Token이 유효하지 않습니다.</exception>
    /// <exception cref="InvalidDocumentException">이 위키에서 유효하지 않는 문서명입니다.</exception>
    /// <exception cref="AccessLevelLacksException">접근 권한이 부족합니다.</exception>
    private static void GuardStatus(string? status, string paramNameForDoc)
    {
        if (status == null) return;
        if (status == "권한이 부족합니다.")
            throw new InvalidApiTokenException($"Api token is not valid.", status);
        if (status == "문서 이름이 올바르지 않습니다.")
            throw new InvalidDocumentException(paramNameForDoc, status);
        if (status.StartsWith("편집"))
            throw new AccessLevelLacksException(status);
    }
}

[Flags]
public enum BacklinkFlags : byte
{
    All = 0,
    Link = 1,
    File = 2,
    Include = 4,
    Redirect = 8,
}