/*namespace Sugarmaple.Core.Tests;

using System.Diagnostics;
using System.Text.Json;
using System.Web;

[TestClass]
public class ApiResultTest
{
    [Obsolete]
    private readonly SeedJsonClient _client = Config.GetJsonClient();
    [Obsolete]
    private readonly SeedJsonClient _jsonClient = Config.GetJsonClient();
    [Obsolete]
    private readonly SeedJsonClient _wrongClient = new(Config.GetValue("WikiUri"));

    [DataTestMethod]
    [DataRow("더시드위키:대문")]
    public void LackOfAuthority(string document)
    {
        var uri = SeedUri.GetEditUri(document);
        var client = Config.GetJsonClient();

        var result = client.GetAsync(uri).Result;

        var expected = HttpUtility.UrlEncode(document).ToUpper();
        Assert.AreEqual(expected, result);
        //{"status":"권한이 부족합니다."}
        //{"status":"편집 권한이 부족합니다. ACL그룹 관리자에 속해 있는 사용자(이)여야 합니다. 해당 문서의 <a href=\"/acl/%EB%8D%94%EC%8B%9C%EB%93%9C%EC%9C%84%ED%82%A4:%EB%8C%80%EB%AC%B8\">ACL 탭</a>을 확인하시기 바랍니다."}
    }

    [TestMethod]
    public void EmptyStringDoc()
    {
        var uri = SeedUri.GetEditUri("");
        var client = Config.GetJsonClient();

        var result = client.GetAsync(uri).Result;

        Assert.AreEqual("{\"status\":\"문서 이름이 올바르지 않습니다.\"}", result);

        var o = JsonSerializer.Deserialize<ViewResponse>(
            result,
            _deserializerOptions);
    }

    [TestMethod]
    public void UnmadeDoc()
    {
        var uri = SeedUri.GetEditUri($"사용자:{Config.GetUserName()}/Unmade");
        var client = Config.GetJsonClient();

        var result = client.GetAsync(uri).Result;
        var json = JsonDocument.Parse(result);
        var exists = json.RootElement.GetProperty("exists");
        //{"text":"","exists":false,"token":}

        Assert.AreEqual(false, exists.GetBoolean());
    }

    [DataTestMethod]
    [DataRow("A")]
    public void TestRightGetApi(string document)
    {
        var uri = SeedUri.GetEditUri(document);
        var client = Config.GetJsonClient();
        var result = client.GetAsync(uri).Result;
        Trace.WriteLine(result);

        var o = JsonSerializer.Deserialize<ViewResponse>(
            result,
            _deserializerOptions);
    }

    [TestMethod]
    public void TestEdit()
    {
        var uri = SeedUri.GetEditUri($"사용자:{Config.GetUserName()}");
    }

    [DataTestMethod]
    [DataRow("더시드위키:대문", "더시드위키")]
    [DataRow("나무위키", "문서")]
    public void BacklinkFrom(string document, string @namespace)
    {
        var uri = SeedUri.GetBacklinkFrom(document, @namespace, "", 1);
        var client = Config.GetJsonClient();
        var task = client.GetBacklinkFromAsync(document, @namespace);
        while (!task.IsCompleted) ;
        var result = task.Result;

        Trace.WriteLine(result);
    }
    //{"namespaces":[{"namespace":"문서","count":7},{"namespace":"사용자","count":3},{"namespace":"더시드위키","count":2}],"backlinks":[{"document":"더시드위키:도움말","flags":"link"},{"document":"더시드위키:분류","flags":"link"}],"from":null,"until":null}

    [DataTestMethod]
    [DataRow("더시드위키:대문", "더시드위키")]
    public void TestBacklinkUntilFlag(string document, string @namespace)
    {
        var uri = SeedUri.GetBacklinkUntil(document, @namespace, "", 1);
        var result = _client.GetAsync(uri).Result;

        Trace.WriteLine(result);
    }

    private static readonly JsonSerializerOptions _deserializerOptions = new() { PropertyNameCaseInsensitive = true };
}
*/