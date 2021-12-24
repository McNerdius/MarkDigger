using System.Diagnostics;
using System.IO;

using Markdig;
using Markdig.CodeBlockHighlighter;
using Markdig.CodeBlockHighlighter.PrismHighlighter;

void md()
{
    var markdown =
@"
```csharp:foo.cs {#id .class}
{
+    Console.WriteLine(""Diff Add"");
-    Console.WriteLine(""Diff Remove"");
    Console.WriteLine(""No Diff"");
}
``` 
";

    var pipeline = new MarkdownPipelineBuilder().UseCodeBlockHighlighter( new PrismHighlighter() )/* .UseGenericAttributes() */.Build();
    // var pipeline = new MarkdownPipelineBuilder().UseGenericAttributes().Build();

    Environment.CurrentDirectory = @"C:\Users\McNerdius\Repos\MarkDigger\HighlighterExtension\PrismHighlighter";

    var html = Markdown.ToHtml( markdown, pipeline );

    Console.WriteLine( html );

}


md();
