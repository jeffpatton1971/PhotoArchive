var command = args.FirstOrDefault();
var path = args.Skip(1).FirstOrDefault();

if (string.IsNullOrEmpty(command) || string.IsNullOrEmpty(path))
{
    Console.WriteLine("Usage:");
    Console.WriteLine("  discover <path>");
    Console.WriteLine("  import-jekyll-gallery <path> [--dry-run]");
    return;
}

switch (command)
{
    case "discover":
        await DiscoverCommand.Run(path);
        break;

    case "import-jekyll-gallery":
        var dryRun = args.Contains("--dry-run");
        await ImportJekyllGalleryCommand.Run(path, dryRun);
        break;
}