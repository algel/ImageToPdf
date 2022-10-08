using McMaster.Extensions.CommandLineUtils;

using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;

var app = new CommandLineApplication();
app.HelpOption();

var files = app.Option("-f|--file <File>", "Path to the file", CommandOptionType.MultipleValue);
var output = app.Option("-o|--output <FilePath>", "Path to save generated file", CommandOptionType.SingleValue);
output.DefaultValue = "generated.pdf";

var watermark = app.Option("-w|--watermark <Watermark>", "Watermark text", CommandOptionType.SingleValue);
var headerText = app.Option("-h|--header <Header>", "Page header text", CommandOptionType.SingleValue);

var isPreview = app.Option<bool>("--preview", "Show preview without saving", CommandOptionType.NoValue);

app.OnExecute(() =>
{
    var doc = Document.Create(container =>
    {
        foreach (var filePath in files.Values)
        {
            container.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(5);

                if (headerText.HasValue())
                {
                    page.Header().AlignMiddle().Text(headerText.Value());
                }

                page.Content().Layers(layers =>
                {
                    layers.PrimaryLayer().Image(filePath, ImageScaling.FitArea);

                    if(watermark.HasValue())
                    {
                        layers.Layer()
                              .AlignCenter()
                              .AlignMiddle()
                              .Text(watermark.Value())
                              .FontSize(72).Bold().FontColor("#3afafafa");
                    }
                });
            });
        }
    });



    if (isPreview.ParsedValue)
    {
        try
        {
            doc.ShowInPreviewer();
        }
        catch (TaskCanceledException)
        {
        }
    }
    else
    {
        doc.GeneratePdf(output.Value());
    }
});

return app.Execute(args);