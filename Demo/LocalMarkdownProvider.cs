using Markdig;

public class LocalMarkdownProvider : IMarkdownContentProvider
{
    private readonly HttpClient httpClient;
    private readonly static MarkdownPipeline markdownPipeline = new MarkdownPipelineBuilder()
                    // .UseAbbreviations()
                    // .UseCustomContainers()
                    // .UseEmphasisExtras()
                    // .UseFootnotes()
                    // .UseGridTables()
                    // .UsePipeTables()
                    //    .UseMediaLinks()
                    // .UseTaskLists()
                    //    .UseAutoLinks()
                    //    .UseEmojiAndSmiley()
                    //    .UseAutoIdentifiers()
                    // .UseGenericAttributes() // Must be last as it is one parser that is modifying other parsers
                    .Build();


    public LocalMarkdownProvider(HttpClient httpClient)
        => this.httpClient = httpClient;

    public async Task<string?> GetMarkdownContent(string name)
    {
        var response = await httpClient.GetAsync($"{httpClient.BaseAddress}/markdown/{name}.md")
                                       .ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var markdown = await response.Content.ReadAsStringAsync()
                                            .ConfigureAwait(false);

            return Markdown.ToHtml(markdown, markdownPipeline);
        }
        else
        {
            return null;
        }
    }
}
