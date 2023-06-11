namespace Sugarmaple.Core.Tests;

using Microsoft.Extensions.Configuration;

internal static class Config
{
    static readonly IConfigurationRoot _root;

    static Config()
    {
        var path = Directory.GetParent(AppContext.BaseDirectory)!.Parent!.Parent!.Parent!.FullName;
        var builder = new ConfigurationBuilder()
            .SetBasePath(path)
            .AddJsonFile("appsettings.json", true, true);
        _root = builder.Build();
    }

    public static string GetValue(string key) =>
        _root[key] ?? throw new KeyNotFoundException();

    public static SeedApiClient GetConfigClient()
    {
        var wikiUri = GetValue("WikiUri");
        var apiToken = GetValue("ApiToken");
        return new(wikiUri, apiToken);
    }

    /*public static SeedJsonClient GetJsonClient()
    {
        var wikiUri = GetValue("WikiUri");
        var ret = new SeedJsonClient(wikiUri);
        var apiToken = GetValue("ApiToken");
        ret.UpdateAuthHeader($"Bearer {apiToken}");
        return ret;
    }*/

    public static string GetUserName() => GetValue("User");
}