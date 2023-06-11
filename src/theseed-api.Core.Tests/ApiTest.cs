namespace Sugarmaple.Core.Tests;
using System.Diagnostics;

[TestClass]
public class ApiTest
{
    private readonly string _userDoc = $"사용자:{Config.GetUserName()}";

    [TestMethod]
    public void EditAndCheckComplete()
    {
        var client = GetClient();
        var editor = client.GetEditAsync(_userDoc).Result;
        var originalText = editor.Text;
        var editText = originalText == "A" ? "B" : "A";

        var result = editor.PostEditAsync(editText, "Debug").Result;

        var rechecked = client.GetViewAsync(_userDoc).Result;
        Trace.WriteLine(result);
        Assert.AreEqual(editText, rechecked);
    }

    [TestMethod]
    public void ThrowEditConflictException()
    {
        var editor1 = GetClient().GetEditAsync(_userDoc).Result;
        var editor2 = GetClient().GetEditAsync(_userDoc).Result;
        var originalText = editor1.Text;
        var editText = originalText == "A" ? "B" : "A";

        var result = editor1.PostEditAsync(editText, "editor1").Result;
        Trace.WriteLine(result);
        try
        {
            _ = editor2.PostEditAsync("", "editor2").Result;
        }
        catch (AggregateException ex)
        {
            Assert.IsInstanceOfType(ex.InnerException, typeof(EditConflictException));
        }
    }

    [DataTestMethod]
    [DataRow("더시드위키:대문")]//Need to choose inauthorized doc.
    public async Task LackOfAuthority(string document)
    {
        var client = GetClient();

        var task = async () => await client.GetEditAsync(document);
        await Assert.ThrowsExceptionAsync<AccessLevelLacksException>(task);
        //{"status":"편집 권한이 부족합니다. ACL그룹 관리자에 속해 있는 사용자(이)여야 합니다. 해당 문서의 <a href=\"/acl/%EB%8D%94%EC%8B%9C%EB%93%9C%EC%9C%84%ED%82%A4:%EB%8C%80%EB%AC%B8\">ACL 탭</a>을 확인하시기 바랍니다."}
    }

    [TestMethod]
    public async Task InvalidDocException()
    {
        var client = GetClient();

        var task = async () => await client.GetEditAsync("틀:");
        await Assert.ThrowsExceptionAsync<InvalidDocumentException>(task);
    }

    [DataTestMethod]
    [DataRow("A")]
    [DataRow("")]
    public async Task TestBacklinkFrom(string document)
    {
        var result = await GetClient().GetBacklinkFromAsync(document);
        foreach (var o in result.Namespaces)
            Trace.WriteLine(o);
        foreach (var o in result.Backlinks)
            Trace.WriteLine(o);
    }

    private static SeedApiClient GetClient() => Config.GetConfigClient();
}
