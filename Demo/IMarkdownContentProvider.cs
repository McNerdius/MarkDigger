public interface IMarkdownContentProvider
{
    public Task<string?> GetMarkdownContent(string name);
}
