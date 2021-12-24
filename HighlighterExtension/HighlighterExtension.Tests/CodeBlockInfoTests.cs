using System;

using Markdig;
using Markdig.CodeBlockHighlighter;

using Xunit;

namespace McPrism.Tests;

public class BasicTests
{
    static string testBlock =
@"
```csharp:foo.cs
{
+    Console.WriteLine(""Diff Add"");
-    Console.WriteLine(""Diff Remove"");
    Console.WriteLine(""No Diff"");
}
``` 
";

    static MarkdownPipeline pipeline
        => new MarkdownPipelineBuilder().UseCodeBlockHighlighter()/* .UseGenericAttributes() */.Build();

    static string[] htmlLines
        => Markdown.ToHtml( testBlock, pipeline ).Split( '\n' );

    [Fact]
    public void FileName()
    {
        Assert.Equal( @"<div class=""mc-code-filename"">foo.cs</div>", htmlLines[1] );
    }

    // [Fact] // not yet supported
    // public void GenericAttributes() 
    // {
    //     Console.WriteLine( htmlLines );
    // }
}