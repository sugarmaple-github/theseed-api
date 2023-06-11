namespace Sugarmaple.TheSeed.Api;
using Sugarmaple.Text;

internal static class SeedUri
{
    public static string GetEditUri(string document) => CreateUri("edit", document).Build();
    public static string GetBacklinkFrom(string document, string @namespace, string from, int flag) =>
        CreateUri("backlink", document)
            .AddQuery(nameof(@namespace), @namespace)
            .AddQuery(nameof(@from), @from)
            .AddQuery(nameof(flag), flag)
            .Build();

    public static string GetBacklinkUntil(string document, string @namespace, string until, int flag) =>
        CreateUri("backlink", document)

                        .AddQuery(nameof(@namespace), @namespace)
        .AddQuery(nameof(until), until)
                        .AddQuery(nameof(flag), flag)
                        .Build();

    private static RelativeUri CreateUri() => RelativeUri.Create("api");
    private static RelativeUri CreateUri(string path, string document) => CreateUri().AddPath(path).AddPath(document);
}
