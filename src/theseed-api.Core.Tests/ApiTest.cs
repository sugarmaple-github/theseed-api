namespace Sugarmaple.Core.Tests;
using System.Diagnostics;

[TestClass]
public class ApiTest
{
    private readonly string _userDoc = $"�����:{Config.GetUserName()}";

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
    [DataRow("���õ���Ű:�빮")]//Need to choose inauthorized doc.
    public async Task LackOfAuthority(string document)
    {
        var client = GetClient();

        var task = async () => await client.GetEditAsync(document);
        await Assert.ThrowsExceptionAsync<AccessLevelLacksException>(task);
        //{"status":"���� ������ �����մϴ�. ACL�׷� �����ڿ� ���� �ִ� �����(��)���� �մϴ�. �ش� ������ <a href=\"/acl/%EB%8D%94%EC%8B%9C%EB%93%9C%EC%9C%84%ED%82%A4:%EB%8C%80%EB%AC%B8\">ACL ��</a>�� Ȯ���Ͻñ� �ٶ��ϴ�."}
    }

    [TestMethod]
    public async Task InvalidDocException()
    {
        var client = GetClient();

        var task = async () => await client.GetEditAsync("Ʋ:");
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
