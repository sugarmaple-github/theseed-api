namespace Sugarmaple.Text;
using System.Text;
internal struct RelativeUri
{
    private readonly StringBuilder _builder;
    private bool isQuery;

    public RelativeUri(string basePath)
    {
        _builder = StringBuilderPool.Obtain().Append(basePath);
        isQuery = false;
    }

    public static RelativeUri Create(string basePath) => new RelativeUri(basePath);

    public RelativeUri AddPath(string path)
    {
        _builder.Append('/').Append(path);
        return this;
    }

    public RelativeUri AddQuery(string name, int value) => AddQuery(name, value.ToString());
    public RelativeUri AddQuery(string name, string value)
    {
        _builder
          .Append(isQuery ? '&' : '?')
          .Append(name)
          .Append('=')
          .Append(value);
        isQuery = true;
        return this;
    }

    public string Build() => _builder.Finish();
}