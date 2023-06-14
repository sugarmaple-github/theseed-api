# theseed-api

[![NuGet Count](https://img.shields.io/nuget/dt/theseed-api.svg?style=flat-square)](https://www.nuget.org/packages/theseed-api/)
[![Issues Open](https://img.shields.io/github/issues/sugarmaple-github/theseed-api.svg?style=flat-square)](https://github.com/sugarmaple-github/theseed-api/issues)

theseed-api provides classes and methods for using API for The Seed wiki engine.


theseed-api는 The Seed 엔진의 API를 C#으로 사용할 수 있게끔 클래스와 메서드를 제공합니다.

## Simple Demo

```cs
using Sugarmaple.TheSeed.Api;
public async Task<int> DoEdit()
{
    var client = new SeedApiClient("https://theseed.io", "API KEY");
    var editor = await client.GetEditAsync("DocName");
    var result = await editor.PostEditAsync("DocContents", "Summary");
    var rev = result.Rev;
    return rev;
}
```

## Roadmap
- 공식 문서 마련 (Official Document)
- 하위 버전 지원 (.net 5 / .net Standard 2.0)
- 센카와, 리버티 스킨 크롤러를 위한 새 프로젝트 (New Projects for Senkawa and Liberty wiki skin crawler)

## Upcoming Features
- 1.0.1: XML 주석 패키지에 포함. (contain Xml comments on packages.)

## Supported Platforms

- .net 6.0