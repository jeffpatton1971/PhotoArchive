/// <summary>
/// Scans a directory of Jekyll gallery Markdown files and reports the front-matter fields found.
/// </summary>
public static class DiscoverCommand
{
    /// <summary>
    /// Scans all <c>*.md</c> files under <paramref name="path"/>, parses their YAML front matter,
    /// and prints a frequency report of every field key found.
    /// </summary>
    /// <param name="path">The root directory to scan recursively.</param>
    public static async Task Run(string path)
    {
        var files = Directory.GetFiles(path, "*.md", SearchOption.AllDirectories);

        var fieldCounts = new Dictionary<string, int>();

        foreach (var file in files)
        {
            var content = await File.ReadAllTextAsync(file);
            var data = FrontMatterParser.Parse(content);

            foreach (var key in data.Keys)
            {
                fieldCounts[key] = fieldCounts.ContainsKey(key)
                    ? fieldCounts[key] + 1
                    : 1;
            }
        }

        Console.WriteLine($"Files scanned: {files.Length}");

        foreach (var kv in fieldCounts.OrderByDescending(x => x.Value))
        {
            Console.WriteLine($"{kv.Key}: {kv.Value}");
        }
    }
}