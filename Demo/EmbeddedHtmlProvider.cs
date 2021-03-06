using System.Reflection;

public sealed class EmbeddedHtmlProvider : IMarkdownContentProvider
{
    Assembly assembly;
    string prefix;
    List<string> resources;

    public EmbeddedHtmlProvider()
    {
        assembly = Assembly.GetExecutingAssembly();
        // TODO: magic string
        prefix = $"Demo.wwwroot.markdown.";
        resources = assembly.GetManifestResourceNames()
                            .Select(name => name.Replace(prefix, ""))
                            .ToList();
    }

    public async Task<string?> GetMarkdownContent(string name)
    {
        if (resources.Contains($"{name}.html") is false)
            return null;
        using var reader = new StreamReader(assembly.GetManifestResourceStream($"{prefix}{name}.html")!);
        return await reader.ReadToEndAsync();
    }
}
