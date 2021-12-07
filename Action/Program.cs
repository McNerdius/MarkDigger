using System.IO;
using DotnetActionsToolkit;
using Markdig;

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

                var newFile = $@"{file.Directory}\{file.Name[..^2]}html";
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
            var pipeline = new MarkdownPipelineBuilder();
            pipeline.Configure(extensions);
            return pipeline.Build();
        }
    }
}
