/*namespace Sugarmaple.Core.Tests;

[TestClass]
public class SeedUriTest
{
    [DataTestMethod]
    [DataRow("더시드위키:대문")]
    public void Edit(string document)
    {
        var uri = SeedUri.GetEditUri(document);

        Assert.AreEqual($"api/edit/{document}", uri);
    }

    [DataTestMethod]
    [DataRow("더시드위키:대문", "더시드위키")]
    public void TestBacklinkFromFlag(string document, string @namespace)
    {
        var uri = SeedUri.GetBacklinkFrom(document, @namespace, "", 1);

        Assert.AreEqual($"api/backlink/{document}?namespace={@namespace}&from=&flag=1", uri);
    }

    [DataTestMethod]
    [DataRow("더시드위키:대문", "더시드위키")]
    public void TestBacklinkUntilFlag(string document, string @namespace)
    {
        var uri = SeedUri.GetBacklinkUntil(document, @namespace, "", 1);

        Assert.AreEqual($"api/backlink/{document}?namespace={@namespace}&until=&flag=1", uri);
    }
}
*/