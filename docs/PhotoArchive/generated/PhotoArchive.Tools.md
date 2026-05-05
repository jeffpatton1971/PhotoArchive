# API Reference
- [DiscoverCommand](#discovercommand)
- [FrontMatterParser](#frontmatterparser)
- [ImportJekyllGalleryCommand](#importjekyllgallerycommand)
- [PhotoMapper](#photomapper)

<a id="discovercommand"></a>
# DiscoverCommand

Scans a directory of Jekyll gallery Markdown files and reports the front-matter fields found.

<a id="discovercommand.run(string)"></a>
## Method: Run(string)
Scans all `*.md` files under `path`, parses their YAML front matter, and prints a frequency report of every field key found.

**Parameters**
- `path` — The root directory to scan recursively.


---

<a id="frontmatterparser"></a>
# FrontMatterParser

Parses YAML front matter from Jekyll-style Markdown files.

<a id="frontmatterparser.parse(string)"></a>
## Method: Parse(string)
Parses the YAML front matter block from the supplied Markdown `content`. The front matter is expected to be the first section delimited by `---`.

**Parameters**
- `content` — The full text of a Markdown file.

**Returns**

A dictionary of key/value pairs parsed from the YAML front matter, or an empty dictionary if no front matter is present.


---

<a id="importjekyllgallerycommand"></a>
# ImportJekyllGalleryCommand

Imports photos from a Jekyll gallery directory into the PhotoArchive database.

<a id="importjekyllgallerycommand.run(string,bool)"></a>
## Method: Run(string, bool)
Reads all `*.md` files under `path`, validates their front matter, and imports new photos into the database. When `dryRun` is the database is not modified and a preview of valid records is printed instead.

**Parameters**
- `path` — The root directory containing Jekyll gallery Markdown files.
- `dryRun` — When, performs validation and preview only without writing to the database.


---

<a id="photomapper"></a>
# PhotoMapper

Maps a parsed YAML front-matter dictionary from a Jekyll gallery Markdown file to a [Photo](#photo) entity.

<a id="photomapper.map(system.collections.generic.dictionary[string,object])"></a>
## Method: Map(Dictionary<string, object>)
Creates a [Photo](#photo) entity from the supplied front-matter dictionary.

**Parameters**
- `data` — A dictionary of key/value pairs parsed from a Jekyll gallery Markdown front-matter block. Must contain at least an `id` key.

**Returns**

A new [Photo](#photo) populated from the front-matter values.

**Exceptions**
- [InvalidOperationException](#invalidoperationexception) — Thrown when the required `id` field is missing or empty.

