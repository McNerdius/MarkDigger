using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.CodeBlockFormatter;

public class CodeBlockFormatter : IMarkdownExtension
{
    private ICodeBlockHighlighter? highlighter;

    public CodeBlockFormatter( ICodeBlockHighlighter? highlighter )
    {
        this.highlighter = highlighter;
    }

    public void Setup( MarkdownPipelineBuilder pipeline )
    {
    }

    public void Setup( MarkdownPipeline pipeline, IMarkdownRenderer renderer )
    {
        ArgumentNullException.ThrowIfNull( renderer );

        if ( renderer is TextRendererBase<HtmlRenderer> htmlRenderer )
        {
            var codeBlockRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>();

            if ( codeBlockRenderer is not null )
            {
                htmlRenderer.ObjectRenderers.Remove( codeBlockRenderer );
            }

            htmlRenderer.ObjectRenderers.AddIfNotAlready( new FormattedCodeBlockRenderer( codeBlockRenderer, highlighter ) );
        }
    }
}

public static class PrismExtensions
{
    public static MarkdownPipelineBuilder UseCodeBlockFormatter( this MarkdownPipelineBuilder pipeline, ICodeBlockHighlighter? highlighter = null )
    {
        pipeline.Extensions.Add( new CodeBlockFormatter( highlighter ) );
        return pipeline;
    }
}
