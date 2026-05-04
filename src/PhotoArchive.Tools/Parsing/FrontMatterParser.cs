using YamlDotNet.Serialization;

public static class FrontMatterParser
{
    public static Dictionary<string, object> Parse(string content)
    {
        var parts = content.Split("---", StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 1)
            return new Dictionary<string, object>();

        var yaml = parts[0];

        var deserializer = new DeserializerBuilder().Build();

        return deserializer.Deserialize<Dictionary<string, object>>(yaml)
               ?? new Dictionary<string, object>();
    }
}