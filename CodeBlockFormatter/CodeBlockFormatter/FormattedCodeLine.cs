using Markdig.Renderers;

namespace Markdig.CodeBlockFormatter;

public record FormattedCodeLine( bool? DiffState = null, string Content = "" )
{
    private string diffAttribute =>
             DiffState switch { null => "", true => "mc-ins", false => "mc-del" };

    public void Render( HtmlRenderer renderer )
    {
        ArgumentNullException.ThrowIfNull( Content );

        renderer.Write( "<div" );
        renderer.WriteAttributes( CSS( "mc-diff", diffAttribute ) );
        renderer.Write( "></div> " );

        renderer.Write( "<pre" );
        renderer.WriteAttributes( CSS( "mc-code", diffAttribute ) );
        renderer.Write( ">" );
        renderer.WriteEscape( Content );
        renderer.WriteLine( "</pre>" );
    }
}
