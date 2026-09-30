using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;
using Markdig.Renderers.Html;
using System.Net;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.IO.Compression;
var root = Path.GetFullPath(args.ElementAtOrDefault(0) ?? ".");
var output = Path.GetFullPath(args.ElementAtOrDefault(1) ?? Path.Combine(root, ".build"));
if (root == output) throw new ArgumentException("Output must not be the source root.");
Directory.CreateDirectory(output);
var pipeline = new MarkdownPipelineBuilder().UseAutoLinks().UseAdvancedExtensions().Build();
string Rel(string p) => Path.GetRelativePath(root,p).Replace('\\','/');
string HtmlName(string p) => Path.ChangeExtension(p,"html").Replace("README.html","index.html");
string Esc(string p) => WebUtility.HtmlEncode(p);
var files = Directory.GetFiles(root,"*.md",SearchOption.AllDirectories).Where(p => !Rel(p).Split('/').Any(s => s is ".git" or "tools" or ".build" or ".github" or "bin" or "obj")).OrderBy(Rel).ToArray();
var docs = files.ToDictionary(p=>p, p=>Markdown.Parse(File.ReadAllText(p),pipeline));
var errors = new List<string>(); var external = new SortedSet<string>(); var checkedLinks = 0; var cliExamples = 0;
var commands = JsonSerializer.Deserialize<string[]>(File.ReadAllText(Path.Combine(root,"tools","cli-command-paths.json")))!;
var headings = docs.ToDictionary(pair=>pair.Key,pair=>pair.Value.Descendants<HeadingBlock>().Select(h=>h.GetAttributes().Id).Where(id=>id is not null).ToHashSet());
foreach(var (file,doc) in docs) {
 var hs=doc.Descendants<HeadingBlock>().ToArray();
 if(hs.Count(h=>h.Level==1)!=1) errors.Add($"{Rel(file)}: require exactly one H1");
 foreach(var image in doc.Descendants<LinkInline>().Where(l=>l.IsImage)) if(image.FirstChild is not LiteralInline alt || string.IsNullOrWhiteSpace(alt.Content.ToString())) errors.Add($"{Rel(file)}: missing image alt text");
 foreach(var code in doc.Descendants<FencedCodeBlock>()) {
  if(string.IsNullOrWhiteSpace(code.Info)) errors.Add($"{Rel(file)}: code fence needs language");
  foreach(var line in code.Lines.ToString().Split('\n').Select(s=>s.Trim())) {
   if(!line.StartsWith("coremq ")) continue; cliExamples++;
   var body=line[7..];
   if(!commands.Any(c=>body==c || body.StartsWith(c+" ") || (body.EndsWith(" --help") && c.StartsWith(body[..^7]+" "))) && body!="--help") errors.Add($"{Rel(file)}: CLI command absent from source-bound inventory: {line}");
  }
 }
 foreach(var link in doc.Descendants<LinkInline>()) {
  var url=link.Url; if(string.IsNullOrWhiteSpace(url)) continue;
  if(Uri.TryCreate(url,UriKind.Absolute,out var abs) && abs.Scheme is "https" or "http" or "mailto") {external.Add(url);continue;}
  if(url.Contains(":") || url.StartsWith("//")) {errors.Add($"{Rel(file)}: unsupported URL {url}");continue;}
  var fragment=url.Contains('#')?Uri.UnescapeDataString(url[(url.IndexOf('#')+1)..]):null;
  var path=url.Split('#','?')[0];
  var dest=path.Length==0?file:Path.GetFullPath(Path.Combine(path.StartsWith('/')?root:Path.GetDirectoryName(file)!,Uri.UnescapeDataString(path.TrimStart('/'))));
  if(!dest.StartsWith(root+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase) || !File.Exists(dest)) errors.Add($"{Rel(file)}: missing/unsafe local link {url}");
  else if(fragment is {Length:>0} && headings.TryGetValue(dest,out var ids) && !ids.Contains(fragment)) errors.Add($"{Rel(file)}: missing anchor {url}");
  checkedLinks++;
  if(!link.IsImage && path.EndsWith(".md",StringComparison.OrdinalIgnoreCase)) link.Url=HtmlName(path)+(fragment is null?"":"#"+fragment);
 }
}
if(errors.Count>0) {foreach(var e in errors) Console.Error.WriteLine(e);return 1;}
var nav=string.Join("",files.Select(p=>$"<a href='/{HtmlName(Rel(p))}'>{Esc(File.ReadLines(p).First(l=>l.StartsWith("# "))[2..])}</a>"));
foreach(var (file,doc) in docs) {
 var title=File.ReadLines(file).First(l=>l.StartsWith("# "))[2..];
 var html=Markdown.ToHtml(doc,pipeline);
 var content=$"<!doctype html><html lang='en'><meta charset='utf-8'><meta name='viewport' content='width=device-width,initial-scale=1'><title>{Esc(title)} · CoreMQ</title><link rel='stylesheet' href='/assets/preview.css'><a class='skip' href='#main'>Skip to content</a><header><a href='/index.html'>CoreMQ</a><span>Documentation preview · reviewed development implementation</span></header><div class='layout'><nav aria-label='Documentation'><label for='nav-search'>Find a guide</label><input id='nav-search' type='search' placeholder='Filter guide titles'>{nav}</nav><main id='main'>{html}<footer>Source-bound guidance. Check release status for qualification and edition limits.</footer></main></div><script src='/assets/preview.js'></script></html>";
 var target=Path.Combine(output,HtmlName(Rel(file)));Directory.CreateDirectory(Path.GetDirectoryName(target)!);File.WriteAllText(target,content);
}
foreach(var p in Directory.GetFiles(Path.Combine(root,"assets"),"*",SearchOption.AllDirectories)) {var target=Path.Combine(output,Rel(p));Directory.CreateDirectory(Path.GetDirectoryName(target)!);File.Copy(p,target,true);}
var zipPath=Path.Combine(output,"coremq-documentation.zip");if(File.Exists(zipPath))File.Delete(zipPath);
using(var zip=ZipFile.Open(zipPath,ZipArchiveMode.Create)) foreach(var p in files.Concat(Directory.GetFiles(Path.Combine(root,"assets"),"*",SearchOption.AllDirectories))) zip.CreateEntryFromFile(p,Rel(p));
var report=new {pages=files.Length,localLinks=checkedLinks,cliExamples,errors=errors.Count,externalLinks=external,renderer="Markdig 0.40.0; preview styles differ from hosted portal",sourceCommit=Environment.GetEnvironmentVariable("GITHUB_SHA")};
File.WriteAllText(Path.Combine(output,"validation.json"),JsonSerializer.Serialize(report,new JsonSerializerOptions{WriteIndented=true}));
Console.WriteLine(JsonSerializer.Serialize(report));return 0;
