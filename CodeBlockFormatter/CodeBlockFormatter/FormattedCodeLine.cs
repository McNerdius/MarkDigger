using Markdig.Renderers;

namespace Markdig.CodeBlockFormatter;

public record FormattedCodeLine( bool? DiffState = null, string Content = "" )
{
    private string diffAttribute =>
             DiffState switch { null => "", true => "mc-ins", false => "mc-del" };

    public void Render( HtmlRenderer renderer, bool escape )
    {
        ArgumentNullException.ThrowIfNull( Content );

        renderer.Write( "<div" );
        renderer.WriteAttributes( CSS( "mc-diff", diffAttribute ) );
        renderer.Write( "></div> " );

        renderer.Write( "<pre" );
        renderer.WriteAttributes( CSS( "mc-code", diffAttribute ) );
        renderer.Write( ">" );

        // only escape if no highlighter is in use
        if ( escape )
            renderer.WriteEscape( Content );
        else
            renderer.Write( Content );

        renderer.WriteLine( "</pre>" );
    }
}
