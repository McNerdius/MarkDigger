using DotnetActionsToolkit;
using Markdig;
using Markdig.CodeBlockFormatter;
using Markdig.CodeBlockFormatter.PrismHighlighter;

namespace MarkDigger;

public class Program
{
    static readonly Core core = new();

    static async Task Main(string[] args)
    {
        try
        {
            var path = core.GetInput("path", required: true);
            var extensions = core.GetInput("extensions") ?? "";

            var pipeline = createPipeline(extensions);

            var inputs = new DirectoryInfo(path).GetFiles("*.md", SearchOption.AllDirectories);
            var outputs = new List<string>();

            foreach (var file in inputs)
            {
                var markdown = File.ReadAllText(file.FullName);
                var html = Markdown.ToHtml(markdown, pipeline);

                var newFile = $"{file.Directory}/{file.Name[..^2]}html";
                File.WriteAllText(newFile, html);
                outputs.Add(newFile);
            }

            core.SetOutput("files", string.Join(", ", outputs));
        }
        catch (Exception ex)
        {
            core.SetFailed(ex.Message);
        }

        static MarkdownPipeline createPipeline(string extensions)
        {
            CodeBlockFormatter? formatter = null;

            if (extensions.Contains("mcprism"))
            {
                extensions = extensions.Replace("mcprism", "");
                formatter = new CodeBlockFormatter(new PrismHighlighter());
            }
            else if (extensions.Contains("mcformat"))
            {
                extensions = extensions.Replace("mcformat", "");
                formatter = new CodeBlockFormatter(highlighter: null);
            }

            extensions = extensions.Replace("++", "+");
            extensions = extensions.Trim('+');

            var pipeline = new MarkdownPipelineBuilder();
            pipeline.Configure(extensions);

            if (formatter is not null)
                pipeline.Use(formatter);

            return pipeline.Build();
        }
    }
}
