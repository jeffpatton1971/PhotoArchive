using YamlDotNet.Serialization;

/// <summary>
/// Parses YAML front matter from Jekyll-style Markdown files.
/// </summary>
public static class FrontMatterParser
{
    /// <summary>
    /// Parses the YAML front matter block from the supplied Markdown <paramref name="content"/>.
    /// The front matter is expected to be the first section delimited by <c>---</c>.
    /// </summary>
    /// <param name="content">The full text of a Markdown file.</param>
    /// <returns>
    /// A dictionary of key/value pairs parsed from the YAML front matter,
    /// or an empty dictionary if no front matter is present.
    /// </returns>
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