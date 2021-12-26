global using static Markdig.CodeBlockFormatter.HtmlAttributeHelper;

using Markdig.Renderers.Html;

namespace Markdig.CodeBlockFormatter;


static class HtmlAttributeHelper
{
    public static HtmlAttributes CSS( params string[] classes )
    {
        var attribute = new HtmlAttributes();
        // pattern match handles nulls too
        classes.Where( s => s?.Trim() is { Length: > 0 } ).ToList().ForEach( attribute.AddClass );
        return attribute;
    }
}

public static class TrimEmptyLinesExtensions
{
    public static IEnumerable<string> Trim( this IEnumerable<string> source )
    {
        return source.Skip( emptyLineCount( source ) )
                .SkipLast( emptyLineCount( source.AsEnumerable().Reverse() ) );

    }

    public static IEnumerable<FormattedCodeLine> Trim( this IEnumerable<FormattedCodeLine> source )
    {
        var strings = source.Select( element => element.Content );

        return source.Skip( emptyLineCount( strings ) )
                         .SkipLast( emptyLineCount( strings.AsEnumerable().Reverse() ) );
    }

    private static int emptyLineCount( IEnumerable<string> source )
        => source.TakeWhile( element => string.IsNullOrWhiteSpace( element ) ).Count();
}
