using System.Globalization;
using Microsoft.AspNetCore.Components;

namespace MarkdownViewer.Components;

public partial class MarkBlobView : ComponentBase
{
    [Inject] public IMarkdownContentProvider MarkdownContentProvider { get; set; }

    [EditorRequired]
    [Parameter] public string Blob { get; set; }

    [EditorRequired]
    [Parameter] public RenderFragment BlobLoading { get; set; }

    [EditorRequired]
    [Parameter] public RenderFragment BlobNotFound { get; set; }

    private string? content = null;
    private bool? found { get; set; } = null;

    private string? blob = null;

    protected override async Task OnParametersSetAsync()
    {
        if (string.IsNullOrEmpty(Blob) || blob == Blob)
            return;

        found = null;
        blob = Blob;

        var markdown = await MarkdownContentProvider.GetMarkdownContent(blob);

        if (markdown is not null)
        {
            content = markdown;
            found = true;
        }
        else
        {
            found = false;
        }
    }
}
