using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;

namespace Markdig.CodeBlockFormatter;

public class FormattedCodeBlockRenderer : HtmlObjectRenderer<CodeBlock>
{
    private readonly CodeBlockRenderer codeBlockRenderer;
    private readonly ICodeBlockHighlighter? highlighter;

    public FormattedCodeBlockRenderer(
        CodeBlockRenderer? codeBlockRenderer,
        ICodeBlockHighlighter? highlighter )
    {
        this.codeBlockRenderer = codeBlockRenderer ?? new CodeBlockRenderer();
        this.highlighter = highlighter;
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

        var codeBlock = new FormattedCodeBlock( filename );

        extractCode( node, codeBlock );

        if ( highlighter is not null )
            codeBlock = highlighter.Highlight( language, codeBlock );

        codeBlock.Render( renderer );
    }

    internal void extractCode( LeafBlock node, FormattedCodeBlock codeBlock )
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
}
