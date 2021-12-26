namespace Markdig.CodeBlockFormatter;

public interface ICodeBlockHighlighter
{
    public FormattedCodeBlock Highlight( string language, FormattedCodeBlock block );
}
