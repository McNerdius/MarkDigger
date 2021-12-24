namespace Markdig.CodeBlockHighlighter;

public interface ICodeBlockHighlighter
{
    public HighlightedCodeBlock Highlight( string language, HighlightedCodeBlock block );
}
