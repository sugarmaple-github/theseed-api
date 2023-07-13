namespace Sugarmaple.TheSeed.Api;
using Sugarmaple.Text;

internal class SeedJsonClient
{
    private readonly JsonClient _client;

    public SeedJsonClient(string wikiUri)
    {
        _client = new(wikiUri); ;
    }

    public Task<string> GetAsync(string uri) => _client.GetLiteralAsync(uri);

    public ValueTask<ViewResponse> GetEditAsync(string document) => _client.GetAsync<ViewResponse>(SeedUri.GetEditUri(document));

    public ValueTask<EditResponse> PostEditAsync(string document, string text, string log, string editToken) => _client.PostAsync<EditResponse, EditParameter>(
            CreateUri("edit", document).Build(), new(text, log, editToken));

    public ValueTask<BacklinkResponse> GetBacklinkFromAsync(string document, string @namespace = "", string @from = "", int flag = 0) =>
        _client.GetAsync<BacklinkResponse>(SeedUri.GetBacklinkFrom(document, @namespace, from, flag));

    public ValueTask<BacklinkResponse> GetBacklinkUntilAsync(string document, string @namespace, string until, int flag) =>
         _client.GetAsync<BacklinkResponse>(SeedUri.GetBacklinkUntil(document, @namespace, until, flag));

    public void UpdateAuthHeader(string value) => _client.UpdateAuthHeader(value);

    private static RelativeUri CreateUri() => RelativeUri.Create("api");
    private static RelativeUri CreateUri(string path, string document) => CreateUri().AddPath(path).AddPath(document);
}

internal record struct EditParameter(string Text, string Log, string Token);
public record struct BacklinkItem(string Document, BacklinkFlags Flag);

internal record struct BacklinkResponse(
    NamespaceCountPair[] Namespaces,
    BacklinkPair[] Backlinks,
    string From,
    string Until,
    string? Status);

public record struct NamespaceCountPair(string Namespace, int Count);
public record struct BacklinkPair(string Document, string Flags);
internal record struct EditResponse(string Status, int Rev);
internal record struct ViewResponse(string Text, bool Exists, string Token, string Status);