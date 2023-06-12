# theseed-api

[![NuGet Count](https://img.shields.io/nuget/dt/theseed-api.svg?style=flat-square)](https://www.nuget.org/packages/theseed-api/)

theseed-api는 The Seed 엔진의 API를 C#으로 사용할 수 있게끔 클래스와 메서드를 제공합니다.

## Simple Demo

```cs
using Sugarmaple.TheSeed.Api;
public async Task<int> DoEdit()
{
    var client = new SeedApiClient("https://theseed.io", "API KEY");
    var editor = await client.GetEditAsync(_userDoc);
    var result = await editor.PostEditAsync("DocName", "Summary");
    var rev = result.Rev;
    return rev;
}
```

## Supported Platforms

- .net 6.0