using System.Diagnostics;

namespace Markdig.CodeBlockFormatter.PrismHighlighter;

public class PrismHighlighter : ICodeBlockHighlighter
{
    public FormattedCodeBlock Highlight( string language, FormattedCodeBlock block )
    {
        var file = Path.GetTempFileName();
        File.WriteAllLines( file, block.Lines );

        // var p = new Process
        // {
        //     StartInfo = new ProcessStartInfo( "npm" )
        //     {
        //         Arguments = "install",
        //     }
        // };

        // p.Start();
        // p.WaitForExit();

        var p = new Process
        {
            StartInfo = new ProcessStartInfo( "node" )
            {
                RedirectStandardOutput = true,
                Arguments = $"mcprism.js --file={file} --language={language}",
            }
        };

        p.Start();
        p.WaitForExit();

        var lines = new List<string>();

        while ( p.StandardOutput.ReadLine() is string s )
        {
            lines.Add( s );
        }

        return block with { Lines = lines };
    }

}