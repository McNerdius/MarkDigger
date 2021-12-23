using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.McPrism;

public class PrismExtension : IMarkdownExtension
{
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

            htmlRenderer.ObjectRenderers.AddIfNotAlready( new PrismCodeBlockRenderer( codeBlockRenderer ) );
        }
    }
}

public static class PrismExtensions
{
    public static MarkdownPipelineBuilder UsePrism( this MarkdownPipelineBuilder pipeline )
    {
        pipeline.Extensions.Add( new PrismExtension() );
        return pipeline;
    }
}
