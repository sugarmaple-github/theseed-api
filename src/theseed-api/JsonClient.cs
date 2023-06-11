namespace Sugarmaple.TheSeed.Api;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

public class JsonClient
{
    private readonly HttpClient _client = new();

    public JsonClient(string baseAddress)
    {
        _client.BaseAddress = new Uri(baseAddress);
    }

    private static readonly JsonSerializerOptions _serializerOptions = new() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    private static readonly JsonSerializerOptions _deserializerOptions = new() { PropertyNameCaseInsensitive = true };
    private static readonly MediaTypeHeaderValue _contentType = MediaTypeHeaderValue.Parse("application/json");

    internal HttpClient InternalClient => _client;

    public ValueTask<TOut?> GetAsync<TOut>(string uri)
    {
        try
        {
            var result = _client.GetAsync(uri).Result;
            var stream = result.Content.ReadAsStream();
            return JsonSerializer.DeserializeAsync<TOut>(stream, _deserializerOptions);
        }
        catch (AggregateException ex)
        {
            if (ex.InnerException is HttpRequestException httpEx)
                throw httpEx;
            throw ex;
        }
    }

    internal Task<string> GetLiteralAsync(string uri) => _client.GetAsync(uri).Result.Content.ReadAsStringAsync();

    public ValueTask<TOut?> PostAsync<TOut, TPost>(string uri, TPost data) => JsonSerializer.DeserializeAsync<TOut>(
        _client.PostAsync(uri,
            JsonContent.Create(data, _contentType, _serializerOptions)).Result.Content.ReadAsStream(),
        _deserializerOptions);

    public bool TryUpdateAuthHeader(string headerValue, Func<bool> validChecker)
    {
        var pastAuth = _client.DefaultRequestHeaders.Authorization;
        _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(headerValue);
        if (!validChecker())
        {
            _client.DefaultRequestHeaders.Authorization = pastAuth;
            return false;
        }
        return true;
    }

    public void UpdateAuthHeader(string value) => _client.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(value);
}
