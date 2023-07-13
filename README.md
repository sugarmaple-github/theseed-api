# theseed-api

[![NuGet Count](https://img.shields.io/nuget/dt/theseed-api.svg?style=flat-square)](https://www.nuget.org/packages/theseed-api/)
[![Issues Open](https://img.shields.io/github/issues/sugarmaple-github/theseed-api.svg?style=flat-square)](https://github.com/sugarmaple-github/theseed-api/issues)

theseed-api provides classes and methods for using API for The Seed wiki engine.


theseed-api�� The Seed ������ API�� C#���� ����� �� �ְԲ� Ŭ������ �޼��带 �����մϴ�.

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
- ���� ���� ���� (Official Document)
- ���� ���� ���� (.net 5 / .net Standard 2.0)
- ��ī��, ����Ƽ ��Ų ũ�ѷ��� ���� �� ������Ʈ (New Projects for Senkawa and Liberty wiki skin crawler)

## Upcoming Features
- 1.0.2: �ѿ� XML �ּ� �ϼ�. (complete Xml comments on packages for both Korean and English.)

## Update Log
- 1.0.1: Solved the problem that some members of 'BacklinkResult' class had wrong protection levels. 
- 1.0.0: Released.

## Supported Platforms

- .net 6.0