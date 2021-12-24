using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.CodeBlockHighlighter;

public class PrismExtension : IMarkdownExtension
{
    private ICodeBlockHighlighter? highlighter;

    public PrismExtension( ICodeBlockHighlighter? highlighter )
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

            htmlRenderer.ObjectRenderers.AddIfNotAlready( new HighlightedCodeBlockRenderer( codeBlockRenderer, highlighter ) );
        }
    }
}

public static class PrismExtensions
{
    public static MarkdownPipelineBuilder UseCodeBlockHighlighter( this MarkdownPipelineBuilder pipeline, ICodeBlockHighlighter? highlighter = null )
    {
        pipeline.Extensions.Add( new PrismExtension( highlighter ) );
        return pipeline;
    }
}
