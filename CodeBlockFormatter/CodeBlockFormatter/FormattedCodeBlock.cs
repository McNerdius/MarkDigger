using Markdig.Renderers;

namespace Markdig.CodeBlockFormatter;

public record FormattedCodeBlock( string FileName = "" )
{
    private List<FormattedCodeLine> lineInfo = new();

    public void Trim() => lineInfo = lineInfo.Trim().ToList();

    public void AddLine( bool? diffState, string content )
        => lineInfo.Add( new( diffState, content ) );

    public IEnumerable<string> Lines
    {
        get => lineInfo.Select( info => info.Content ).Trim();
        init
        {
            var newLines = value.Trim();

            if ( newLines.Count() != Lines.Count() )
            {
                throw new Exception( "Line count mismatch !" );
            }
            else
            {
                lineInfo = lineInfo.Zip
                (
                    value, ( info, content ) => new FormattedCodeLine( info.DiffState, content )
                ).ToList();
            }
        }
    }

    public void Render( HtmlRenderer renderer )
    {
        // mc-code-block: 2*n css grid
        renderer.Write( "<div" );
        renderer.WriteAttributes( CSS( "mc-code-block", "not-prose" ) );
        renderer.WriteLine( ">" );

        // if ( FileName.Length > 0 )
        { // mc-code-filename: colspan 2
            renderer.Write( "<div" );
            renderer.WriteAttributes( CSS( "mc-code-filename" ) );
            renderer.Write( ">" );

            if ( FileName.Length > 0 )
                renderer.Write( FileName );

            renderer.WriteLine( "</div>" );
        }

        renderer.Write( "<div" );
        renderer.WriteAttributes( CSS( "mc-code-lines" ) );
        renderer.WriteLine( ">" );

        { // CodeLineInfo == 2 divs
            foreach ( var line in lineInfo.Trim() )
                line.Render( renderer );
        }

        // </mc-code-lines> </mc-code-block>
        renderer.WriteLine( "</div></div>" );

    }
}
