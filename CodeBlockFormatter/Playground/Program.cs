using System.Diagnostics;
using System.IO;

using Markdig;
using Markdig.CodeBlockFormatter;
using Markdig.CodeBlockFormatter.PrismHighlighter;

void md()
{
    var markdown =
@"
```csharp:foo.cs
{
+    Console.WriteLine(""Diff Add"");
-    Console.WriteLine(""Diff Remove"");
    Console.WriteLine(""No Diff"");
}
``` 
";

    var pipeline = new MarkdownPipelineBuilder().UseCodeBlockFormatter( /* new PrismHighlighter()  */ ).Build();

    // Environment.CurrentDirectory = @"C:\Users\McNerdius\Repos\MarkDigger\HighlighterExtension\PrismHighlighter";

    var html = Markdown.ToHtml( markdown, pipeline );

    Console.WriteLine( html );

}


md();
