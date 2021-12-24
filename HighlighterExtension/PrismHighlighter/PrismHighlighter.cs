using System.Diagnostics;

namespace Markdig.CodeBlockHighlighter.PrismHighlighter;

public class PrismHighlighter : ICodeBlockHighlighter
{
    public HighlightedCodeBlock Highlight( string language, HighlightedCodeBlock block )
    {
        var file = Path.GetTempFileName();
        File.WriteAllLines( file, block.Lines );

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

        return block with { Lines = lines };
    }

}