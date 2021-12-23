using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.McPrism;

public record CodeLineInfo( bool? DiffState = null, string Content = "" )
{
    private HtmlAttributes diffAttribute
        => HtmlAttributeHelper.ClassAttribute( DiffState switch { null => "", true => "mc-ins", false => "mc-del" } );

    public void Render( ref HtmlRenderer renderer )
    {
        ArgumentNullException.ThrowIfNull( Content );

        renderer.Write( "<div" );
        renderer.WriteAttributes( diffAttribute );
        renderer.Write( "></div> " );

        renderer.Write( "<pre" );
        renderer.WriteAttributes( HtmlAttributeHelper.ClassAttribute( "mc-code" ) );
        renderer.Write( ">" );
        renderer.Write( Content );
        renderer.WriteLine( "</pre>" );
    }
}
