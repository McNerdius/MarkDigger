using System.Diagnostics;
using System.IO;

using Markdig;
using Markdig.McPrism;

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

    var pipeline = new MarkdownPipelineBuilder().UsePrism().UseGenericAttributes().Build();
    // var pipeline = new MarkdownPipelineBuilder().UseGenericAttributes().Build();

    Environment.CurrentDirectory = @"C:\Users\McNerdius\Documents\Repos\MarkDigger\HighlighterExtension\McPrism";

    var html = Markdown.ToHtml( markdown, pipeline );

    Console.WriteLine( html );

}


md();
