using System.Diagnostics;

using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.McPrism;

public class PrismCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
{
    private readonly CodeBlockRenderer codeBlockRenderer;

    public PrismCodeBlockRenderer( CodeBlockRenderer? codeBlockRenderer )
    {
        this.codeBlockRenderer = codeBlockRenderer ?? new CodeBlockRenderer();
    }

    protected override void Write( HtmlRenderer renderer, CodeBlock node )
    {
        if ( node is not FencedCodeBlock fencedCodeBlock || node.Parser is not FencedCodeBlockParser parser )
        {
            codeBlockRenderer.Write( renderer, node );
            return;
        }

        var blockInfo = fencedCodeBlock?.Info?.Split( ':' );

        var (language, filename) = blockInfo switch
        {
            null => ("", ""),
            { Length: 1 } => (blockInfo[0], ""),
            { Length: 2 } => (blockInfo[0], blockInfo[1]),
            _ => throw new NotSupportedException()
        };

        if ( string.IsNullOrWhiteSpace( language ) || !PrismSupportedLanguages.IsSupportedLanguage( language ) )
        {
            codeBlockRenderer.Write( renderer, node );
            return;
        }

        var codeBlock = new CodeBlockInfo( filename );

        extractCode( node, ref codeBlock );
        highlightCode( language, ref codeBlock );

        codeBlock.Render( ref renderer );
    }

    internal void extractCode( LeafBlock node, ref CodeBlockInfo codeBlock )
    {
        var lines = node.Lines.Lines;
        var totalLines = lines.Length;

        for ( var i = 0; i < totalLines; i++ )
        {
            var line = lines[i];
            var slice = line.Slice;
            if ( slice.Text == null )
            {
                continue;
            }

            var lineText = slice.Text.Substring( slice.Start, slice.Length );

            (var diff, lineText) = lineText[0] switch
            {
                '-' => (false, lineText[1..]),
                '+' => (true, lineText[1..]),
                _ => (null as bool?, lineText)
            };

            codeBlock.AddLine( diff, lineText );
        }
    }

    internal void highlightCode( string language, ref CodeBlockInfo codeBlock )
    {
        var file = Path.GetTempFileName();
        File.WriteAllLines( file, codeBlock.Lines );

        var a = new Process
        {
            StartInfo = new ProcessStartInfo( "node" )
            {
                RedirectStandardOutput = true,
                Arguments = $"mctest.js --file={file} --language={language}",
            }
        };

        a.Start();
        a.WaitForExit();

        var lines = new List<string>();

        while ( a.StandardOutput.ReadLine() is string s )
        {
            lines.Add( s );
        }

        codeBlock = codeBlock with { Lines = lines };
    }

}
