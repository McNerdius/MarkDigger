using Markdig.Renderers;

namespace Markdig.CodeBlockHighlighter;

public record HighlightedCodeBlock( string FileName = "" )
{
    private List<HighlightedCodeLine> lineInfo = new();

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
                    value, ( info, content ) => new HighlightedCodeLine( info.DiffState, content )
                ).ToList();
            }
        }
    }

    public void Render( HtmlRenderer renderer )
    {
        // mc-code-block: 2*n css grid
        renderer.Write( "<div" );
        renderer.WriteAttributes( HtmlAttributeHelper.ClassAttribute( "mc-code-block" ) );
        renderer.WriteLine( ">" );

        if ( FileName.Length > 0 )
        { // mc-code-filename: colspan 2
            renderer.Write( "<div" );
            renderer.WriteAttributes( HtmlAttributeHelper.ClassAttribute( "mc-code-filename" ) );
            renderer.Write( ">" );

            renderer.Write( FileName );
            renderer.WriteLine( "</div>" );
        }

        { // CodeLineInfo == 2 divs
            foreach ( var line in lineInfo.Trim() )
                line.Render( renderer );
        }

        // /mc-code-block
        renderer.WriteLine( "</div>" );

    }
}
