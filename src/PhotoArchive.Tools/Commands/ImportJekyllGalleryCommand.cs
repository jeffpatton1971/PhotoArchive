using PhotoArchive.Core.Entities;

public static class ImportJekyllGalleryCommand
{
    public static async Task Run(string path, bool dryRun)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            Console.WriteLine("ERROR: Path is required.");
            return;
        }

        if (!Directory.Exists(path))
        {
            Console.WriteLine($"ERROR: Directory not found: {path}");
            Console.WriteLine("Usage:");
            Console.WriteLine(@"  import-jekyll-gallery ""C:\code\projects\kansaspattons\_gallery"" --dry-run");
            return;
        }
        
        var files = Directory.GetFiles(path, "*.md", SearchOption.AllDirectories);

        Console.WriteLine($"Files found: {files.Length}");
        Console.WriteLine(dryRun ? "[DRY RUN]" : "[IMPORT]");

        int processed = 0;
        int valid = 0;
        int invalid = 0;

        foreach (var file in files)
        {
            processed++;

            try
            {
                var content = await File.ReadAllTextAsync(file);
                var data = FrontMatterParser.Parse(content);
                var photo = PhotoMapper.Map(data);

                var errors = Validate(photo).ToList();

                if (errors.Any())
                {
                    invalid++;
                    Console.WriteLine();
                    Console.WriteLine($"INVALID: {Path.GetFileName(file)}");
                    foreach (var error in errors)
                    {
                        Console.WriteLine($"  - {error}");
                    }

                    continue;
                }

                valid++;

                if (dryRun && valid <= 25)
                {
                    Console.WriteLine($"VALID: {photo.Slug} | {photo.TakenAt:yyyy-MM-dd} | {photo.Source}");
                }
            }
            catch (Exception ex)
            {
                invalid++;
                Console.WriteLine();
                Console.WriteLine($"ERROR: {Path.GetFileName(file)}");
                Console.WriteLine($"  - {ex.Message}");
            }
        }

        Console.WriteLine();
        Console.WriteLine("Summary");
        Console.WriteLine("=======");
        Console.WriteLine($"Processed: {processed}");
        Console.WriteLine($"Valid:     {valid}");
        Console.WriteLine($"Invalid:   {invalid}");
    }

    private static IEnumerable<string> Validate(Photo photo)
    {
        if (string.IsNullOrWhiteSpace(photo.Slug))
            yield return "Slug is required.";

        if (string.IsNullOrWhiteSpace(photo.Source))
            yield return "Source is required.";

        if (string.IsNullOrWhiteSpace(photo.OriginalUrl))
            yield return "OriginalUrl is required.";

        if (photo.TakenAt is null)
            yield return "TakenAt is required.";

        if (photo.Year is null or < 1800 or > 2200)
            yield return $"Year is invalid: {photo.Year}";

        if (photo.Month is null or < 1 or > 12)
            yield return $"Month is invalid: {photo.Month}";

        if (photo.Day is null or < 1 or > 31)
            yield return $"Day is invalid: {photo.Day}";

        if (photo.TakenAt is not null &&
            photo.Year is not null &&
            photo.Month is not null &&
            photo.Day is not null)
        {
            if (photo.TakenAt.Value.Year != photo.Year ||
                photo.TakenAt.Value.Month != photo.Month ||
                photo.TakenAt.Value.Day != photo.Day)
            {
                yield return $"TakenAt date {photo.TakenAt:yyyy-MM-dd} does not match Year/Month/Day {photo.Year}-{photo.Month}-{photo.Day}.";
            }
        }
    }
}